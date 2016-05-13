using System.IO;
using System.Xml.Serialization;
using Zephyr.BuildOps.SceneCompiler;

namespace Zephyr.BuildOps.SceneCompiler
{
  public static class SceneDeserializer
  {
    /// <summary>
    /// Load scene container from xml file.
    /// </summary>
    /// <param name="xmlPath">Path of xml to load data from.</param>
    /// <returns>SceneContainer loaded from file</returns>
    public static SceneContainer LoadSceneContainerFromXml(string xmlPath)
    {
      return Deserialize<SceneContainer>(xmlPath);
    }

    /// <summary>
    /// Load platform container from xml file.
    /// </summary>
    /// <param name="xmlPath">Path of xml to load data from.</param>
    /// <returns>PlatformContainer loaded from file</returns>
    public static PlatformContainer LoadPlatformContainerFromXml(string xmlPath)
    {
      return Deserialize<PlatformContainer>(xmlPath);
    }


    /// <summary>
    /// Deserialize a given xml serialized file at a given path.
    /// </summary>
    /// <typeparam name="T">Type of Container</typeparam>
    /// <param name="xmlPath">Path of xml to read from</param>
    /// <returns>Container with collected data</returns>
    private static T Deserialize<T>(string xmlPath)
    {
      var serializer = new XmlSerializer(typeof(T));
      var stream = new FileStream(xmlPath, FileMode.Open);
      var container = (T) serializer.Deserialize(stream);
      stream.Close();

      return container;
    }
  }
}

