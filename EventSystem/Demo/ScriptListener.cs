using UnityEngine;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Demo
{
    /// <summary>
    /// Used to schow how to connect to hte global event manager using script.
    /// </summary>
    public class ScriptListener : MonoBehaviour
    {
        /// <summary>
        /// Adds a Listener when the object with this script becomes active.
        /// </summary>
        private void OnEnable() { EventManager.Instance.AddListener<BoxClickEvent>(OnBoxClick); }

        /// <summary>
        /// Removes a Listner when this object becomes unactive.
        /// </summary>
        private void OnDisable() { EventManager.Instance.RemoveListener<BoxClickEvent>(OnBoxClick); }

        /// <summary>
        /// The method that will be called if the BoxClickEvent is triggered.  This is a rudementary method, the logic may not make see.
        /// </summary>
        /// <param name="clickEvent">The click event passed when the event was triggered.</param>
        private void OnBoxClick(BoxClickEvent clickEvent)
        {
            //Changes the scale, just to show it got the correct reference
            clickEvent.Box.transform.localScale = new Vector3(2, 2, 2);
        }
    }
}

