using HutongGames.PlayMaker;

namespace Zephyr.EventSystem.Core.Playmaker.Actions
{
    /// <summary>
    /// The QueueEventAction is used to queue an event into the global event manager.  This queued event will then be picked up by all listeners
    /// and performed.  The event must be instantiated beforehand, and passed into
    /// </summary>
    [ActionCategory("Game Event Triggers")]
    public abstract class QueueEventAction : FsmStateAction
    {

        /// <summary>
        /// Runs on Enter into this state.  This will get the event from the overridding class, and then
        /// use that to queue the event.
        /// </summary>
        public override void OnEnter()
        {
            var evt = GetEventToQueue();
            EventManager.Instance.QueueEvent(evt);
            Finish();
        }

        /// <summary>
        /// Abstract method that must be overridden.  This method must return the event object that will be passed to
        /// all listeners of the type of GameEvent used.
        /// </summary>
        protected abstract GameEvent GetEventToQueue();
    }
}