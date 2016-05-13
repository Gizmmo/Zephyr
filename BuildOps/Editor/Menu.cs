using Zephyr.BuildOps.SceneCompiler;
using UnityEditor;
using Zephyr.BuildOps.SceneCompiler;
using BuildOps.BuildActions;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using HutongGames.PlayMaker;
using Zephyr.BuildOps;

namespace Zephyr.BuildOps
{
    public class Menu
    {
        /// <summary>
        /// Save Scene as a SceneContainer xml. Used to load and save nested scenes
        /// </summary>
        [MenuItem("Tools/Build Ops/Save")]
        private static void SaveCurrentScene()
        {
            //Create necessary folders and get file names
            var path = SceneWriter.GetPathForXml();
            SceneWriter.WriteResourceDevOpsFolder();

            var sc = new SceneWriter(path);
            sc.Write();
        }

        [MenuItem("Tools/Build Ops/Build/Build For iOS")]
        private static void BuildForIOS()
        {
            var path = EditorUtility.OpenFolderPanel("Select Build Location", "", "");
            Builder.PerformIOSBuild(path);   
        }

        [MenuItem("Tools/Build Ops/Build/Build For Android")]
        private static void BuildForAndroid()
        {
            var path = EditorUtility.OpenFolderPanel("Select Build Location", "", "");
            Builder.PerformAndroidBuild(path);   
        }

        [MenuItem("Tools/Build Ops/Build/Build For Windows")]
        private static void BuildForWindows()
        {
            var path = EditorUtility.OpenFolderPanel("Select Build Location", "", "");
            Builder.PerformWindowBuild(path);   
        }

        /// <summary>
        /// Load Scene. This will allow the user to select an xml to load scene container from.
        /// </summary>
        [MenuItem("Tools/Build Ops/Load File")]
        private static void LoadScene()
        {
            var path = EditorUtility.OpenFilePanel("Select Scene Xml", Settings.ResourceBuildOpsData, ".xml");
        }

        /// <summary>
        /// Load Mobile will load an assumed mobile file. This setting can be found in the settings file.
        /// </summary>
        [MenuItem("Tools/Build Ops/Load Mobile")]
        private static void LoadMobile()
        {
            LoadScenePath(Settings.MobileBuildOpsFile);
        }

        /// <summary>
        /// Load standalone xml from an assumed file location. This is set in the settings file.
        /// </summary>
        [MenuItem("Tools/Build Ops/Load Standalone")]
        private static void LoadStandalone()
        {
            LoadScenePath(Settings.StandaloneBuildOpsFile);
        }

        /// <summary>
        /// Run the reader class to load a designated scene.
        /// </summary>
        /// <param name="path"></param>
        private static void LoadScenePath(string path)
        {
            var reader = new SceneReader(path);
            reader.LoadScene(path);
        }
    }
}
