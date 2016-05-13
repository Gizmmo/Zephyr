using UnityEngine;
using System.Xml.Serialization;

namespace Zephyr.BuildOps.SceneCompiler
{
    public class NestedScene
    {
        [XmlAttribute("Path")] public string Path;

        [XmlAttribute("Name")] public string Name;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path">Path to given scene in Unity project</param>
        /// <param name="name">Name of the given scene</param>
        public NestedScene(string path, string name)
        {
            Path = path;
            Name = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public NestedScene()
        {
            
        }
    }
}