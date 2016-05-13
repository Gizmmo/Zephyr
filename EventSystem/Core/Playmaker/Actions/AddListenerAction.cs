using HutongGames.PlayMaker;
using System.Collections;

namespace Zephyr.EventSystem.Core.Playmaker.Actions
{
    /// <summary>
    /// Abstract class used to create Game Events for the Event Manager.  Use this class to build custom Game Event Actions.
    /// You will need to override AddListener to set up a proper listener in the Event Manager.  If any other functionality is wanted,
    /// you will need to override the OnListenerTrigger method to add further logic.  Lastly, you will need to override the RemoveListner
    /// method and also unregister the listner that has been stored.  This is to prevent memory leaks.
    /// </summary>
    [ActionCategory("Game Event Listeners")]
    public abstract class AddListenerAction : FsmStateAction
    {
        [Tooltip("The Event Transition that will be called when the event is triggered.")]
        public FsmEvent EventToTrigger;

        /// <summary>
        /// Runs on enter into this state.
        /// </summary>
        public override void OnEnter()
        {
            Init();
            Finish();
        }

        /// <summary>
        /// Override to set the type of event used by this action.  Also can be used to set anything else wanted as well.
        ///   ex - SetEventType<SomeGameEvent>();
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Used to set the type of the game event.  Also controls whether the event is only run once.  If you choose to set isOnce to
        /// false, be aware you must manage the removal of your event from the global event manager (using RemoveEvent<T>),
        /// or else you will get memory leaks!
        /// </summary>
        /// <typeparam name="T">The type of event to queue</typeparam>
        /// <param name="isOnce">if true, the listener removes itself, otherwise, it remains a listener.</param>
        protected void SetEventType<T>(bool isOnce = true) where T : GameEvent
        {
            //HACK: This needs to be updated once the EventManager flush is fixed up a bit.
            
//            if (isOnce)
//                EventManager.Instance.AddListenerOnce<T>(Listener);
//            else
                EventManager.Instance.AddListener<T>(Listener);
        }

        /// <summary>
        /// Removes the passed event from the global event manager.
        /// </summary>
        /// <typeparam name="T">The type of GanmeEvent to remove</typeparam>
        protected void RemoveEvent<T>() where T : GameEvent { EventManager.Instance.RemoveListener<T>(Listener); }

        /// <summary>
        /// Runs the methods needed after the event has been triggered.  It will first call the overridable OnListenerTrigger
        /// method, followed up by finishing the event transition.  This is so that overrided code will run before changing
        /// states.
        /// </summary>
        /// <param name="evt"></param>
        protected void Listener(GameEvent evt)
        {
            OnListenerTrigger(evt);
            FinalizeListenerTrigger();
        }

        /// <summary>
        /// This method is overridable.  It does not need to be, but any logic needed once the event is triggered can
        /// be placed in here.
        /// </summary>
        /// <param name="evt">The event object that was triggered.  This event will need to be casted to the actual derived GameEvent class.</param>
        protected virtual void OnListenerTrigger(GameEvent evt) { }

        /// <summary>
        /// Runs at the end of the Triggering of a Global Event.  This will call a Playmaker Event that the was set in the playmaker ui.
        /// </summary>
        private void FinalizeListenerTrigger()
        {
            if (EventToTrigger != null)
                Fsm.Event(EventToTrigger);
        }

        /// <summary>
        /// Runs on Exit from the state.
        /// </summary>
        public override void OnExit() { Exit(); }

        /// <summary>
        /// Called on exit of the state, you can use this to remove your event from the queue manager.
        /// </summary>
        protected virtual void Exit() { }
    }

    
}