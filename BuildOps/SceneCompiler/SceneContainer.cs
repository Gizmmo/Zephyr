using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Zephyr.BuildOps.SceneCompiler
{
    /// <summary>
    /// Scene Container, holding nested data for a given scene.
    /// </summary>
    public class SceneContainer : IContainer
    {
        [XmlArray("NestedScenes")] public List<NestedScene> Scenes;

        /// <summary>
        /// Constructor
        /// </summary>
        public SceneContainer()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenes">List of Nested Scenes in a given scene</param>
        public SceneContainer(List<NestedScene> scenes)
        {
            Scenes = scenes;
        }

        /// <summary>
        /// Serialize the data to a given xml path.
        /// </summary>
        /// <param name="path">Path to store xml</param>
        public void Serialize(string path)
        {
            var serializer = new XmlSerializer(typeof(SceneContainer));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }
}