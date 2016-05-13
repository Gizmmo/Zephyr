
using UnityEditor;
using UnityEditor.SceneManagement;
using Zephyr.BuildOps.SceneCompiler;

namespace Zephyr.BuildOps.SceneCompiler
{
    /// <summary>
    /// Reader to compile Scene Containers from their corresponding XML's.
    /// </summary>
    public class SceneReader
    {
        public SceneContainer CurrentScene { get; private set; }
        public PlatformContainer CurrentPlatform { get; private set; }
        public string XmlPath { get; private set; }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xmlPath">Path for a given xml file.</param>
        public SceneReader(string xmlPath)
        {
            XmlPath = xmlPath;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Public API to load a Scene from a given xml path. This will load the xml into a scene container,
        /// and then load the nested scenes out of the container.
        /// </summary>
        public void LoadScene()
        {
            LoadBuildSettings(XmlPath);

            CurrentScene = SceneDeserializer.LoadSceneContainerFromXml(XmlPath);
            LoadSceneContainer(CurrentScene);
        }


        /// <summary>
        /// Public API to load a Scene from a given xml path. This will load the xml into a scene container,
        /// and then load the nested scenes out of the container. 
        /// </summary>
        /// <param name="xmlPath">Location of xml to load scene from</param>
        public void LoadScene(string xmlPath)
        {
            XmlPath = xmlPath;
            LoadScene();
        }

        /// <summary>
        /// Take a scene container object, and load the scene and nested scenes for the given container.
        /// </summary>
        /// <param name="container">Container holding the data of the Scene</param>
        public void LoadSceneContainer(SceneContainer container)
        {
            EditorSceneManager.OpenScene(container.Scenes[0].Path);

            for (var i = 1; i < container.Scenes.Count; i++)
            {
                EditorSceneManager.OpenScene(container.Scenes[i].Path, OpenSceneMode.Additive);
            }
        }

        /// <summary>
        /// Load Build Settings to a given platform. This will be based off a nested scene selected.
        /// </summary>
        /// <param name="path">Path of given scene container</param>
        public void LoadBuildSettings(string path)
        {
            var buildSettingsFile = SceneWriter.AddBuildSettingsExtension(path);
            CurrentPlatform = SceneDeserializer.LoadPlatformContainerFromXml(buildSettingsFile);
            var scenes = CollectBuildSceneSettings(CurrentPlatform);

            EditorBuildSettings.scenes = scenes;
        }

        /// <summary>
        /// Collect all scenes listen in global file for build settings.
        /// </summary>
        /// <param name="container">PlatformContainer with scenes in build</param>
        /// <returns>Array of Scenes to add to build settings</returns>
        public EditorBuildSettingsScene[] CollectBuildSceneSettings(PlatformContainer container)
        {
            var scenes = new EditorBuildSettingsScene[container.Scenes.Count];
            for (var i = 0; i < container.Scenes.Count; i++)
            {
                scenes[i] = new EditorBuildSettingsScene(container.Scenes[i].Scenes[0].Path, true);
            }

            return scenes;
        }



        #endregion
    }
}
