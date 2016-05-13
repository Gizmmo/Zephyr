using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Zephyr.BuildOps.SceneCompiler
{
    public class PlatformContainer : IContainer
    {
        [XmlAttribute("Platform")] public string Platform;
        [XmlArray("Scenes")] public List<SceneContainer> Scenes;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformContainer()
        {
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="scenes">A List of Scene Containers from the current build settings</param>
        /// <param name="platform">Name of the platform</param>
        public PlatformContainer(List<SceneContainer> scenes, string platform)
        {
            Scenes = scenes;
            Platform = platform;
        }

        /// <summary>
        /// Serialize Data in the platform container. This will save an xml format of the platform.
        /// </summary>
        /// <param name="path">Path to seralize data to</param>
        public void Serialize(string path)
        {
            var serializer = new XmlSerializer(typeof(PlatformContainer));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }
}