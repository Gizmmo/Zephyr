using UnityEngine;

namespace Zephyr.Extensions
{
    public static class MonobehaviourExtensions
    {
        /// <summary>
        /// Returns a list of all Transforms that are children of this transform
        /// </summary>
        /// <param name="t">The transform to search</param>
        /// <returns>A list of all transforms that are children of this transform</returns>
        public static Transform[] GetChildren(this Transform t)
        {
            var returnArray = new Transform[t.childCount];
            for (var i = 0; i < t.childCount; i++)
            {
                returnArray[i] = t.GetChild(i);
            }
            return returnArray;
        }

        /// <summary>
        /// Returns a list of all gameobjects that are children of this Transform
        /// </summary>
        /// <param name="t">The transform to search</param>
        /// <returns>A List of all gameobjects that are children of this transform</returns>
        public static GameObject[] GetChildrenAsGameObjects(this Transform t)
        {
            var returnArray = new GameObject[t.childCount];
            for (var i = 0; i < t.childCount; i++)
            {
                returnArray[i] = t.GetChild(i).gameObject;
            }
            return returnArray;
        }

        /// <summary>
        /// Finds an immediate child of this transform by a given tag
        /// </summary>
        /// <param name="t">The transform to extend</param>
        /// <param name="tag">The tag to search by</param>
        /// <returns>The found gameobject</returns>
        public static Transform FindChildByTag(this Transform t, string tag)
        {
            for (var i = 0; i < t.childCount; i++)
            {
                var child = t.GetChild(i);
                if (child.CompareTag(tag))
                {
                    return child;
                }
            }
            return null;
        }

        /// <summary>
        /// Find the root of a given component. This will search from the base sent, and to the max depth provided.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        /// <param name="t">Root Component to start search from</param>
        /// <param name="maxDepth">The depth to search for root</param>
        /// <returns>The found root.</returns>
        public static Transform FindRootWithComponent<T>(this Transform t, int maxDepth = 20) where T : Component
        {
            var currentTransform = t;
            var index = 0;
            while (currentTransform != null && index < maxDepth)
            {
                var component = currentTransform.GetComponent<T>();
                if (component != null)
                {
                    return currentTransform;
                }
                currentTransform = currentTransform.parent;
                index++;
            }

            return null;
        }

        /// <summary>
        /// Destroys the passed gameobject as long as it exists
        /// </summary>
        /// <param name="g"></param>
        public static void SafeDestroy(this GameObject g)
        {
            if (g != null)
            {
                Object.Destroy(g);
            }
        }

        /// <summary>
        /// Sets the parent of the given gameobject as the other gameobject passed
        /// </summary>
        /// <param name="t">The gameobject to set the parent of</param>
        /// <param name="parent">The parent gameobject</param>
        public static void SetNewParent(this Transform t, Transform parent)
        {
            t.SetParent(parent, false);
        }

        /// <summary>
        /// Sets the parent of the given gameobject as the other gameobject passed
        /// </summary>
        /// <param name="g">The gameobject to set the parent of</param>
        /// <param name="parent">The parent gameobject</param>
        public static void SetNewParent(this GameObject g, GameObject parent)
        {
            g.transform.SetParent(parent.transform, false);
        }

        /// <summary>
        /// Sets the x position of the transform to the float passed
        /// </summary>
        /// <param name="t">The transform to set</param>
        /// <param name="x">The new x value</param>
        public static void SetPositionX(this Transform t, float x)
        {
            t.position = new Vector3(x, t.position.y, t.position.z);
        }

        /// <summary>
        /// Sets the y position of the transform to the float passed
        /// </summary>
        /// <param name="t">The transform to set</param>
        /// <param name="y">The new y value</param>
        public static void SetPositionY(this Transform t, float y)
        {
            t.position = new Vector3(t.position.x, y, t.position.z);
        }

        /// <summary>
        /// Sets the z position of the transform to the float passed
        /// </summary>
        /// <param name="t">The transform to set</param>
        /// <param name="z">The new z value</param>
        public static void SetPositionZ(this Transform t, float z)
        {
            t.position = new Vector3(t.position.x, t.position.y, z);
        }

        /// <summary>
        /// Gets or add a component. Usage example:
        /// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
        /// </summary>
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            var result = child.GetComponent<T>() ?? child.gameObject.AddComponent<T>();
            return result;
        }
    }
}
