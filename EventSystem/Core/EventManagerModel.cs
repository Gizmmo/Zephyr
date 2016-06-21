using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Zephyr.EventSystem.Core
{
    /// <summary>
    /// Event Manager Model allows users to create possible listeners to given Game Events. If that Game
    /// Event is queued, then triggered, all listeners for the given event will run. This follows a simple
    /// message queue pattern. The key api here is: AddListener and QueueEvent.
    /// </summary>
    public class EventManagerModel
    {
        #region Public Variables

        //Limit queue processing allows us to put a max time on queues triggered per frame.
        //If we go over the time, they are pushed to the front of the next frame flush.
        public bool LimitQueueProcesing = false;
        public float QueueProcessTime = 0.0f;

        #endregion

        #region Private Variables

        private readonly Queue _eventQueue = new Queue();

        //Full list of listeners, waiting to be actioned by event queue. This stores the delegate as actioned calls.
        private readonly Dictionary<System.Type, EventDelegate> _delegates =
            new Dictionary<System.Type, EventDelegate>();

        //Delegate lookup is used to index and search for given delegates.
        private readonly Dictionary<System.Delegate, EventDelegate> _delegateLookup =
            new Dictionary<System.Delegate, EventDelegate>();

        //OnceLookups are stored listeners, similar to delegate lookup, to indicate a listener that can only be actioned once.
        private readonly Dictionary<System.Delegate, System.Delegate> _onceLookups =
            new Dictionary<System.Delegate, System.Delegate>();

        #endregion

        #region Delegates and Events
        //Delegates that are used to store listeners.
        public delegate void EventDelegate<in T>(T e) where T : GameEvent;
        private delegate void EventDelegate(GameEvent e);

        #endregion

        #region Public Methods

        /// <summary>
        /// Add a listener to the dictionary of listeners.  This listener will reside 
        /// in the dictionary until it is removed. If an event is triggered with the 
        /// same name, we will call the desired delegate (or callback)
        /// </summary>
        /// <typeparam name="T">EventType we will listen for, and index dict with.</typeparam>
        /// <param name="del">Delegate, or callback, when event T is triggered</param>
        public void AddListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            AddDelegate(del);
        }

        /// <summary>
        /// Add a listener to the dictionary of listeners. This listener will only reside
        /// in the dictionary for a single call. Once the event type has been triggered,
        /// the delegate (or callback) will be called, and the listener removed from future use.
        /// </summary>
        /// <typeparam name="T">Event type will will listen for, and index dict with</typeparam>
        /// <param name="del">Delegate, or callback when event T is triggered</param>
        public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
        {
            //Result will have delegate if added, else null
            var result = AddDelegate(del);

            //If we collected a result, add to oncelookups
            if (result != null)
                _onceLookups[result] = del;
        }

        /// <summary>
        /// Remove Listener from the delegate listener dictionary. This will require both the same
        /// game event, and the same event delegate.
        /// </summary>
        /// <typeparam name="T">GameEvent to add listener to.</typeparam>
        /// <param name="del">The delegate to tie to listener, and called on event trigger.</param>
        public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            var internalDelegate = RemoveDelegateLookupListener(del);
            RemoveOnceLookUpListener(internalDelegate);
            RemoveDelegateListener<T>(internalDelegate);
        }

        /// <summary>
        /// Remove all listeners from delegate dictionaries.
        /// </summary>
        public void RemoveListeners()
        {
            _delegates.Clear();
            _delegateLookup.Clear();
            _onceLookups.Clear();
        }

        /// <summary>
        /// Do any of the delegate dictionaries have listeners. Check if any count is above 0.
        /// </summary>
        /// <returns>If any delegate dictionary has a value, return true, else false</returns>
        public bool HasListeners()
        {
            return _delegates.Count == 0 && _delegateLookup.Count == 0 && _onceLookups.Count == 0;
        }

        /// <summary>
        /// Check if the delegate lookup contains the passed eventdelegate. True if it exists, else false.
        /// We use the combination of GameEvent type and delegate to determine if lookup exists.
        /// </summary>
        /// <typeparam name="T">GameEvent of the given delegate</typeparam>
        /// <param name="del">Delegate we are going to use to search up delegate</param>
        /// <returns>If the listener already exist, return true, else false.</returns>
        public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            return _delegateLookup.ContainsKey(del);
        }

        /// <summary>
        /// Trigger a passed Game Event. This can be used externally to trigger an event immediatly, avoiding the event
        /// queue. This should only be used if you must action on the current frame, otherwise the queue is recommended.
        /// This method is also used when each individual queue event is called.
        /// </summary>
        /// <param name="e">GameEvent that has been actioned.</param>
        public void TriggerEvent(GameEvent e)
        {
            EventDelegate del;

            if (_delegates.ContainsKey(e.GetType()) && _delegates.TryGetValue(e.GetType(), out del))
            {
                //Run listeners with GameEvent
                del.Invoke(e);

                //Search and remove actioned lookups
                RemoveActionedOnceLookups(e);
            }
            else
                Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }

        /// <summary>
        /// Inserts a GameEvent into the queue event. The queue event will be run on the next processing loop,
        /// or as time is given. If there is no listeners of the passed event type in the delegate list, 
        /// return false, otherwise add the event and return true.
        /// </summary>
        /// <param name="evt">GameEvent to add to the queue event.</param>
        /// <returns>If no listeners in the delegate list with event type, return false, else true.</returns>
        public bool QueueEvent(GameEvent evt)
        {
            // If there is listeners with the passed event type
            if (!_delegates.ContainsKey(evt.GetType()))
            {
                //...Log warning and return false.
                Debug.LogWarning("EventManager: GetEventToQueue failed due to no listeners for event: " + evt.GetType());
                return false;
            }
            //...if there is no listeners enqueue event and return true.
            _eventQueue.Enqueue(evt);
            return true;
        }

        /// <summary>
        /// Return the current event queue size.
        /// </summary>
        /// <returns>int, current queue size</returns>
        public int QueueSize()
        {
            return _eventQueue.Count;
        }

        /// <summary>
        /// Is the current event queue empty. If the queue size is 0, return true, else false.
        /// </summary>
        /// <returns>bool, return true if queue has elements, else false.</returns>
        public bool IsQueueEmpty()
        {
            return QueueSize() == 0;
        }

        /// <summary>
        /// Trigger each event fround in the event queue. This is intended to be called in an update loop,
        /// or in a coroutine. Each update cycle the queue is processed, if the queue processing is limited,
        /// a maximum processing time per update can be set after which the vents will have to be processed
        /// next update loop.
        /// </summary>
        public void ProcessEvents()
        {
            var timer = 0.0f;
            // Iterate through each event in the queue...
            while (_eventQueue.Count > 0)
            {
                //...if we have passed the timer limit, stop processing
                if (LimitQueueProcesing)
                    if (timer > QueueProcessTime) return;

                // ...if we have time in our limit, dequeue and trigger next event...
                var evt = _eventQueue.Dequeue() as GameEvent;
                TriggerEvent(evt);

                //...and add time to our timer.
                if (LimitQueueProcesing)
                    timer += Time.deltaTime;
            }
        }

        /// <summary>
        /// Clear All Events and Listeners.
        /// </summary>
        public void ClearAll()
        {
            RemoveListeners();
            _eventQueue.Clear();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add a given delegate to the delegate dictionary. Once a delegate is added to the 
        /// dictionary, when an event is triggered of type T, the delegate will be called.
        /// If the delegate already exists in our lookup, return a null. If the delegate 
        /// does not exist, return the delegate.
        /// </summary>
        /// <typeparam name="T">GameEvent to store delegates to.</typeparam>
        /// <param name="del">Delegate to be run when event is triggered</param>
        /// <returns>InternalDelegate constrution of eventDelegate, else null</returns>
        private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : GameEvent
        {
            // Early out if we've already registered this delegate 
            if (_delegateLookup.ContainsKey(del))
                return null;

            //Create a new non-generic delegate which calls our generic one.
            // This is the delegate we actually invoke
            EventDelegate internalDelegate = (e) => del((T) e);
            _delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;

            //Find if the delegate with the k of Type T exists...
            if (_delegates.TryGetValue(typeof(T), out tempDel))
            {
                //...if exists, add the internal delegate to delegate stored
                _delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                //...does not exist, add the interal deleagate as the value
                _delegates[typeof(T)] = internalDelegate;
            }

            return internalDelegate;
        }

        /// <summary>
        /// Remove a OnceLookupListener. The internalDelegate is what is used to lookup
        /// the value. If a value is found, remove the System.Delegate, and return the
        /// found value. If nothing is found, return null.
        /// </summary>
        /// <param name="internalDelegate">Delegate used to search up as key in onceLookup dictionary</param>
        /// <returns>System.Delegate of found delegate, else null</returns>
        private System.Delegate RemoveOnceLookUpListener(EventDelegate internalDelegate)
        {
            System.Delegate tempDel;
            if (_onceLookups.TryGetValue(internalDelegate, out tempDel))
                 _onceLookups.Remove(internalDelegate);

            return tempDel;
        }

        /// <summary>
        /// Look up actioned GameEvent in oncelookups, meaning they can only be actioned once. If found, remove
        /// that listener from all possible dictionaries.
        /// </summary>
        /// <param name="e">GameEvent that was actioned.</param>
        private void RemoveActionedOnceLookups(GameEvent e)
        {
            //Get all delegates stored in the delegate dict of the GameEvent type.
            foreach (EventDelegate k in _delegates[e.GetType()].GetInvocationList())
            {
                //if oncelookups does not have found key, skip to next key
                if (!_onceLookups.ContainsKey(k)) continue;

                //Key was found, so remove listener.
                _delegates[e.GetType()] -= k;

                //If this empties the delegate list, remove it from the delegates dictionary
                if (_delegates[e.GetType()] == null)
                    _delegates.Remove(e.GetType());

                //remove the oncelookup from the delegate lookup, then delete the oncelookup.
                _delegateLookup.Remove(_onceLookups[k]);
                _onceLookups.Remove(k);
            }
        }

        /// <summary>
        /// Remove a Delegate Listener from the delegates dictionary. The delegate in the dictionary is
        /// stored with a type T key. If the delegate is found, remove the given internalDelegate from
        /// the list.
        /// </summary>
        /// <typeparam name="T">Type to lookup delegates with in the dictionary.</typeparam>
        /// <param name="internalDelegate">Delegate to remove from listeners.</param>
        /// <returns>Found delegate that was removed from list, else null.</returns>
        private EventDelegate RemoveDelegateListener<T>(EventDelegate internalDelegate) where T : GameEvent
        {
            EventDelegate tempDel;
            //If there is no delegates with Type t, return null.
            if (!_delegates.TryGetValue(typeof(T), out tempDel)) return null;

            //DelegateList is found, remove delegate from list.
            if (internalDelegate != null) tempDel -= internalDelegate;

            //If there no delegates left in the list, remove list from dictionary
            if (tempDel == null)
                _delegates.Remove(typeof(T));
            else
                _delegates[typeof(T)] = tempDel;

            return tempDel;
        }

        /// <summary>
        /// Remove delegate from the lookup listener. The delegate is indexed by the passed delegate.
        /// If the delegate is found, remove the value, and return the removed delegate. Else return null.
        /// </summary>
        /// <typeparam name="T">Type of EventDelegate</typeparam>
        /// <param name="del">Delegate to be removed from lookup list.</param>
        /// <returns>If delegate is removed, return found delegate, else false.</returns>
        private EventDelegate RemoveDelegateLookupListener<T>(EventDelegate<T> del) where T: GameEvent
        {
            EventDelegate internalDelegate;
            if (!_delegateLookup.TryGetValue(del, out internalDelegate)) return null;
            _delegateLookup.Remove(del);

            return internalDelegate;
        }

        #endregion
    }
}