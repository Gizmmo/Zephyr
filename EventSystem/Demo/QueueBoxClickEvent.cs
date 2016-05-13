using HutongGames.PlayMaker;
using UnityEngine;
using Zephyr.EventSystem.Core;
using Zephyr.EventSystem.Core.Playmaker.Actions;

namespace Zephyr.EventSystem.Demo
{
    /// <summary>
    /// This is strictly a demo class, used to show how to construct custom playmaker actions for the Event
    /// Manager class.  This will simple queue an event when clicked.
    /// </summary>
    public class QueueBoxClickEvent : QueueEventAction
    {
        [UIHint(UIHint.FsmGameObject), RequiredField, HutongGames.PlayMaker.Tooltip("The gameObject that will was clicked.")]
        public GameObject ObjectClicked;

        /// <summary>
        /// Overrides the derived method, this method must return the event that will be passed to all
        /// listeners.
        /// </summary>
        /// <returns>The event that will be passed to all listeners.</returns>
        protected override GameEvent GetEventToQueue() { return new BoxClickEvent(ObjectClicked); }
    }
}