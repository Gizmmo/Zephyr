using UnityEngine;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Demo
{
    /// <summary>
    /// The class used to Set up click events on each of the boxes.
    /// </summary>
    public class BoxClick : MonoBehaviour
    {
        /// <summary>
        /// Watches for the onMouseDown event on this cube, and then queue's an event that the box was clicked.
        /// </summary>
        private void OnMouseDown() { EventManager.Instance.QueueEvent(new BoxClickEvent(gameObject)); }
    }

    /// <summary>
    /// The event used for BoxClick.  It will have a refrence to the box clicked.
    /// </summary>
    public class BoxClickEvent : GameEvent {
        public GameObject Box;

        public BoxClickEvent(GameObject box) { Box = box; }
    }

}

