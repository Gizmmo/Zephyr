using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Zephyr.EventSystem.Core
{
    public class EventManagerModel
    {
        public bool LimitQueueProcesing = false;
        public float QueueProcessTime = 0.0f;

        private readonly Queue _eventQueue = new Queue();

        public delegate void EventDelegate<T>(T e) where T : GameEvent;

        private delegate void EventDelegate(GameEvent e);

        private readonly Dictionary<System.Type, EventDelegate> _delegates = new Dictionary<System.Type, EventDelegate>();

        private readonly Dictionary<System.Delegate, EventDelegate> _delegateLookup =
            new Dictionary<System.Delegate, EventDelegate>();

        private readonly Dictionary<System.Delegate, System.Delegate> _onceLookups =
            new Dictionary<System.Delegate, System.Delegate>();

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
            if (_delegates.TryGetValue(typeof (T), out tempDel))
                _delegates[typeof (T)] = tempDel += internalDelegate;
            else
                _delegates[typeof (T)] = internalDelegate;

            return internalDelegate;
        }

        public void AddListener<T>(EventDelegate<T> del) where T : GameEvent { AddDelegate<T>(del); }

        public void AddListenerOnce<T>(EventDelegate<T> del) where T : GameEvent
        {
            var result = AddDelegate<T>(del);

            if (result != null)
                _onceLookups[result] = del;
        }

        public void RemoveListener<T>(EventDelegate<T> del) where T : GameEvent
        {
            EventDelegate internalDelegate;
            if (_delegateLookup.TryGetValue(del, out internalDelegate))
            {
                if (_onceLookups.ContainsKey(internalDelegate))
                    _onceLookups.Remove(internalDelegate);

                EventDelegate tempDel;
                if (_delegates.TryGetValue(typeof (T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                        _delegates.Remove(typeof (T));
                    else
                        _delegates[typeof (T)] = tempDel;
                }

                _delegateLookup.Remove(del);
            }
        }

        public void RemoveAll()
        {
            _delegates.Clear();
            _delegateLookup.Clear();
            _onceLookups.Clear();
        }

        public bool IsDelegatesEmpty() { return _delegates.Count == 0 && _delegateLookup.Count == 0 && _onceLookups.Count == 0; }

        public bool HasListener<T>(EventDelegate<T> del) where T : GameEvent { return _delegateLookup.ContainsKey(del); }

        public void TriggerEvent(GameEvent e)
        {
            EventDelegate del;

            if (_delegates.ContainsKey(e.GetType()) && _delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);

                if (!_delegates.ContainsKey(e.GetType())) return;


                //remove listeners which should only be called once
                foreach (EventDelegate k in _delegates[e.GetType()].GetInvocationList())
                {
                    if (!_onceLookups.ContainsKey(k)) continue;

                    _delegates[e.GetType()] -= k;

                    //TODO: Test

                    if (_delegates[e.GetType()] == null)
                        _delegates.Remove(e.GetType());


                    _delegateLookup.Remove(_onceLookups[k]);
                    _onceLookups.Remove(k);
                }
            }
            else
                Debug.LogWarning("Event: " + e.GetType() + " has no listeners");
        }

        //Inserts the event in the current queue
        public bool QueueEvent(GameEvent evt)
        {
            if (!_delegates.ContainsKey(evt.GetType()))
            {
                Debug.LogWarning("EventManager: GetEventToQueue failed due to no listeners for event: " + evt.GetType());
                return false;
            }

            _eventQueue.Enqueue(evt);
            return true;
        }

        public int QueueSize() { return _eventQueue.Count; }

        public bool IsQueueEmpty() { return QueueSize() == 0; }

        //Every update cycle the queue is processed, if the queue processing is limited, a maxiumum proccesing time per
        //update can be set after which the events will have to be processed next update loop.
        public void OnUpdate()
        {
            var timer = 0.0f;
            while (_eventQueue.Count > 0)
            {
                if (LimitQueueProcesing)
                    if (timer > QueueProcessTime) return;

                var evt = _eventQueue.Dequeue() as GameEvent;
                TriggerEvent(evt);

                if (LimitQueueProcesing)
                    timer += Time.deltaTime;
            }
        }

        public void OnApplicationQuit()
        {
            RemoveAll();
            _eventQueue.Clear();
        }
    }
}