using UnityEngine;
using System.Collections.Generic;
using Zephyr.Singletons;

namespace Zephyr.MonoBehaviours
{

    /// <summary>
    /// In charge of running the update loop.  This update loop is more efficient then the unity built update loop.  Later this class should be extended to include high priority update items adnd such to cusomize the level of priority
    /// items in the update loop have.
    /// </summary>
    public class GameRunner : SingletonAsComponent<GameRunner>
    {
        public static GameRunner Instance { get { return (GameRunner) _Instance; } set { _Instance = value; } }

        private List<IUpdateable> _updateableObjects = new List<IUpdateable>();

        /// <summary>
        /// Registers an updateable object into the Update loop
        /// </summary>
        /// <param name="obj">The object to register into the update loop</param>
        public void RegisterUpdateableObject(IUpdateable obj)
        {
            if (!_updateableObjects.Contains(obj))
                _updateableObjects.Add(obj);
        }
        
        /// <summary>
        /// Unregisters an updateable component from the update loop
        /// </summary>
        /// <param name="obj">The object to remove from the Update loop</param>
        public void UnregisterUpdateableObject(IUpdateable obj)
        {
            if (_updateableObjects.Contains(obj))
                _updateableObjects.Remove(obj);
        }

        /// <summary>
        /// Updates once a frame.
        /// </summary>
        private void Update()
        {
            var delta = Time.deltaTime;
            for (var i = 0; i < Instance._updateableObjects.Count; ++i)
                _updateableObjects[i].OnUpdate(delta);
        }
    }
}
