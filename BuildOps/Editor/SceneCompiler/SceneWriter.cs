using System;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zephyr.BuildOps.SceneCompiler.Exceptions;

namespace Zephyr.BuildOps.SceneCompiler
{
    /// <summary>
    /// Compile Xml files from the current scene or project.
    /// </summary>
    public class SceneWriter
    {
        public string Path { get; private set; }

        public SceneWriter(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Collect All scenes in the Build Settings and Place into XML format to read.
        /// </summary>
        public void CompileBuildSettings(string path)
        {
            try
            {
                //Create necessary folders and get file names
                var platform = ParsePlatformFromPath(path);

                //Add Extension to path
                path = AddBuildSettingsExtension(path);

                // Collect Scenes for XML
                var scenesInBuild = CollectScenePathsFromBuildSettings(EditorBuildSettings.scenes);
                var sceneContainers = CompileAllScenes(scenesInBuild);
                var platformContainer = CreatePlatformContainer(sceneContainers, platform);

                //Write xml
                WriteXmlScene(platformContainer, path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Collect Current scene nested scenes, and place into xml format.
        /// </summary>
        /// <param name="path">Path of xml file to save</param>
        public void Write(string path)
        {
            Path = path;
            Write();
        }

        /// <summary>
        /// Collect Current Scene Nested Scenes, and place into xml format.
        /// </summary>
        public void Write()
        {
            try
            {
                // Collect Scenes for XML
                var sceneContainers = CompileCurrentScene();

                //Write xml
                WriteXmlScene(sceneContainers, Path);

                CompileBuildSettings(Path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// Add a given buildsetting extension to identify build settings xmls compiled.
        /// </summary>
        /// <param name="path">Path of given compiled xml, to add extension to</param>
        /// <returns>Path with added extension</returns>
        public static string AddBuildSettingsExtension(string path)
        {
            const string buildSettingsExtension = "_build";
            const string fileExtension = ".xml";
            var platform = ParsePlatformFromPath(path);

            return path.Replace(platform + fileExtension, platform + buildSettingsExtension + fileExtension);
        }

        /// <summary>
        /// Parse the name of the file from the path provided. This file needs to reside in the Resources folder,
        /// and have a .xml extension.
        /// </summary>
        /// <param name="path">Path to save xml to.</param>
        /// <returns>Parsed string for name of xml</returns>
        public static string ParsePlatformFromPath(string path)
        {
            var splitPath = path.Split('/');
            const string delimiter = "Resources";
            const string fileExtension = ".xml";

            //Path must include Resources below Assets/
            if (splitPath.Length == 1 || path.IndexOf(delimiter, StringComparison.Ordinal) < 0)
                throw new IncorrectPathException();

            var unparsedName = splitPath[splitPath.Length - 1];

            //Path must include xml extension
            if (unparsedName.IndexOf(fileExtension, StringComparison.Ordinal) < 0)
                throw new IncorrectExtensionException();

            return unparsedName.Substring(0, unparsedName.IndexOf(fileExtension, StringComparison.Ordinal));
        }

        /// <summary>
        /// Create a Scene IContainer out of the current scene, and return that scene object.
        /// </summary>
        /// <returns>SceneContainer holding data of current scene</returns>
        public SceneContainer CompileCurrentScene()
        {
            var compiledScenes = CompileScene();
            var parsedScenes = ParseScenes(compiledScenes);
            return CreateSceneContainer(parsedScenes);
        }

        /// <summary>
        /// Compile all Scenes for a given Project. This is found through the build settings for the project.
        /// </summary>
        /// <returns></returns>
        public List<SceneContainer> CompileAllScenes(string[] scenePaths)
        {
            var sceneContainers = new List<SceneContainer>();

            for (var i = 0; i < scenePaths.Length; i++)
            {
                EditorSceneManager.OpenScene(scenePaths[i]);
                sceneContainers.Add(CompileCurrentScene());
            }

            return sceneContainers;
        }

        /// <summary>
        /// Collect an array of scene paths from the scenes listed in build settings. This data is passed in for testing
        /// purposes.
        /// </summary>
        /// <param name="scenes">Array of scenes to collect paths from.</param>
        /// <returns>Array of paths</returns>
        public string[] CollectScenePathsFromBuildSettings(EditorBuildSettingsScene[] scenes)
        {
            var scenePaths = new string[scenes.Length];
            for (var i = 0; i < scenes.Length; i++)
            {
                scenePaths[i] = scenes[i].path;
            }

            return scenePaths;
        }

        /// <summary>
        /// Create PlatformContainer, which will be the root xml file for each platform.
        /// </summary>
        /// <param name="scenes">List of all scene containers making up the build settings.</param>
        /// <param name="platform">Platform name this will be built for</param>
        /// <returns>PlatformContainer created</returns>
        public PlatformContainer CreatePlatformContainer(List<SceneContainer> scenes, string platform)
        {
            return new PlatformContainer(scenes, platform);
        }

        /// <summary>
        /// Compile all scenes in the current scene into a list of scenes
        /// </summary>
        /// <returns>List of all Scenes</returns>
        public Scene[] CompileScene()
        {
            var sceneCount = SceneManager.sceneCount;
            var scenes = new Scene[sceneCount];

            //Collect all scenes and place in array.
            for (var i = 0; i < sceneCount; i++)
            {
                scenes[i] = SceneManager.GetSceneAt(i);
            }

            return scenes;
        }

        /// <summary>
        /// Compiles array of scenes into an array of NestedScene objects, useful for exporting to XML
        /// </summary>
        /// <param name="scenes">Array of NestedScene Management scenes.</param>
        /// <returns>Array of Parsed scenes.</returns>
        public List<NestedScene> ParseScenes(Scene[] scenes)
        {
            var parsedScenes = new List<NestedScene>();

            //For each Scene in current scene
            for (var i = 0; i < scenes.Length; i++)
            {
                //Add Scene to list, using our nestedScene xml object
                var scene = scenes[i];
                parsedScenes.Add(new NestedScene(scene.path, scene.name));
            }

            return parsedScenes;
        }

        /// <summary>
        /// Creates a Scene IContainer, with a list of Scenes.
        /// </summary>
        /// <param name="scenes">List of Scenes to compiler container with</param>
        /// <returns>Scene container with list of scenes as xml type</returns>
        public SceneContainer CreateSceneContainer(List<NestedScene> scenes)
        {
            return new SceneContainer(scenes);
        }

        /// <summary>
        /// Export a SceneContainer as an XML, with a path of the users discretion
        /// </summary>
        public void WriteXmlScene(IContainer scene, string path)
        {
            scene.Serialize(path);
        }

        #region Private Methods

        /// <summary>
        /// Write ResourceDevOps folder ("/Resources/BuildOps") if it
        /// doesn't exist already
        /// </summary>
        public static void WriteResourceDevOpsFolder()
        {
            if (Directory.Exists(Settings.ResourceBuildOpsData))
                Directory.CreateDirectory(Settings.ResourceBuildOpsData);
        }

        /// <summary>
        /// Collect Path for the Xml by collection user data
        /// </summary>
        /// <returns>xml path</returns>
        public static string GetPathForXml()
        {
            const string fileExtension = "xml";
            const string saveFileMesage = "Select folder to save xml to";

            return EditorUtility.SaveFilePanel(saveFileMesage, Settings.ResourceBuildOpsData,
                string.Empty, fileExtension);
        }

        #endregion
    }
}