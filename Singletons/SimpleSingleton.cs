using UnityEngine;
using Zephyr.MonoBehaviours;


namespace Zephyr.Singletons
{
    /// <summary>
    /// A simple singleton.  This singleton will not exist through scenes, and will not create itself if it does not exist.
    /// </summary>
    /// <typeparam name="T">The type of singleton</typeparam>
    public class SimpleSingleton<T> : AdvancedMonoBehaviour where T : MonoBehaviour
    {
        protected static T InnerInstance;

        /// <summary>
        /// Returns the instance of this singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (InnerInstance != null)
                    return InnerInstance;

                InnerInstance = (T) FindObjectOfType(typeof (T));

                if (InnerInstance != null) return InnerInstance;

                if (Debug.isDebugBuild)
                    Debug.LogWarning("An instance of " + typeof (T) + " is needed in the scene, but there is none.");

                return null;
            }
        }
    }
}
