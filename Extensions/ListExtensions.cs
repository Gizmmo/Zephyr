using System;
using System.Collections.Generic;

namespace Zephyr.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Adds a object to the List if the item does not already exist in it
        /// </summary>
        /// <typeparam name="T">The Type of object to add</typeparam>
        /// <param name="coll">The collection to add the item to</param>
        /// <param name="item">The item to check and add to the list.</param>
        public static void AddIfNotExists<T>(this ICollection<T> coll, T item)
        {
            if (!coll.Contains(item)) coll.Add(item);
        }

        /// <summary>
        /// Return a Deep Clone of a list of items that implement the ICloneable interface.
        /// </summary>
        /// <typeparam name="T">Type of nodes, that implement ICloneable</typeparam>
        /// <param name="listToClone">List, that contains nodes</param>
        /// <returns>A deepcloned list of items.</returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            var list = new List<T>();

            foreach (T t in listToClone)
            {
                list.Add((T)t.Clone());
            }

            return list;
        }
    }
}
