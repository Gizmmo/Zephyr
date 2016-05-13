using HutongGames.PlayMaker;
using UnityEngine;
using Zephyr.EventSystem.Core;
using Zephyr.EventSystem.Core.Playmaker.Actions;


namespace Zephyr.EventSystem.Demo {

    /// <summary>
    /// This is a demo action to show how to set up a custom action for any global events.
    /// </summary>
    [HutongGames.PlayMaker.Tooltip("Runs when a box is clicked, used for demo only.")]
    public class AddBoxClickListener : AddListenerAction
    {

        /// <summary>
        /// This is a GameObject that will be extracted from the BoxClickEvent returned by
        /// the global event manager.
        /// </summary>
        [UIHint(UIHint.FsmGameObject), HutongGames.PlayMaker.Tooltip("Where the Box that was clicked will be stored."), RequiredField]
        public FsmGameObject BoxObjectClicked;

        /// <summary>
        /// Overrided the Init method in AddListenerAction and Sets the type of event for the event manager.
        /// This must always be done, or else the listner is never added.
        /// </summary>
        protected override void Init()
        {
            SetEventType<BoxClickEvent>();
        }

        /// <summary>
        /// OnListenerTrigger does not need to be overriden, but when it is, the event must be casted to the correct
        /// type, and then any extra logic can be done here.  In this case, we cast to our BoxClickEvent and then abtract
        /// the GameObject Box from the event, and then store it in a user defined playmaker variable.  The user then can
        /// use this in a later state.
        /// </summary>
        /// <param name="evt"></param>
        protected override void OnListenerTrigger(GameEvent evt)
        {
            // We cast the event to its actual event.  We know its safe since we declared it earlier in AddListener.
            var boxEvt = (BoxClickEvent)evt;

            // Abstract the Box GameObject out of the event, and store it in a user defined variable.
            BoxObjectClicked.Value = boxEvt.Box;
        }

        /// <summary>
        /// This is done here, but actually does not do anything in this context.  This is simply just to show how you
        /// could remove manually if needed.
        /// </summary>
//        protected override void Exit() { RemoveEvent<BoxClickEvent>(); }
    }
}
