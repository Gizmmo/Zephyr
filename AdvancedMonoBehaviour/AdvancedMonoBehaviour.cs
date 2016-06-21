using UnityEngine;

namespace Zephyr.MonoBehaviours
{
    public class AdvancedMonoBehaviour : MonoBehaviour, IUpdateable, IStartable
    {
        /// <summary>
        /// Runs on Start for all objects.
        /// </summary>
        private void Start()
        {
            OnStart();
            GameRunner.Instance.RegisterUpdateableObject(this);
        }

        /// <summary>
        /// Runs when the gameobject or this component is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (GameRunner.IsAlive)
                GameRunner.Instance.UnregisterUpdateableObject(this);
        }

        /// <summary>
        /// Used to enter the update loop.  Anything within this method will be called once a frame.
        /// </summary>
        /// <param name="delta"></param>
        public virtual void OnUpdate(float delta) { }

        /// <summary>
        /// Must be used instead of Start in an AdvancedMonobehaviour.  Acts in the place of start.
        /// </summary>
        public virtual void OnStart() { }
    }
}
