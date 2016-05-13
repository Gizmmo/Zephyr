using System;
using System.Collections.Generic;
using UnityEditor;
using BuildOps.BuildActions;
using AppBuilder;
using System.Runtime.InteropServices;

namespace Zephyr.BuildOps
{
    public class Builder
    {
        static string[] SCENES = FindEnabledEditorScenes();

        static string _appName = PlayerSettings.productName;
        const string _targetDir = "/unity";

        static void PerformBuild()
        {
            GenericBuild(
                SCENES,
                GetArg("-targetPath"),
                (BuildTarget)Enum.Parse(typeof(BuildTarget), GetArg("-buildTarget")),
                (BuildOptions)Enum.Parse(typeof(BuildOptions), GetArg("-buildOptions") ?? "None"));
        }

        public static void PerformIOSBuild(string path = _targetDir)
        {
            string target_dir = _appName;
            GenericBuild(SCENES, path + "/" + target_dir, BuildTarget.iOS, BuildOptions.None);
        }

        public static void PerformAndroidBuild(string path = _targetDir)
        {
            string target_dir = _appName + ".apk";
            GenericBuild(SCENES, path + "/" + target_dir, BuildTarget.Android, BuildOptions.None);
        }

        public static void PerformWindowBuild(string path = _targetDir)
        {
            string target_dir = _appName + ".exe";
            GenericBuild(SCENES, path + "/" + target_dir, BuildTarget.StandaloneWindows64, BuildOptions.None);
        }

        private static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        private static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
        {

            if (!System.IO.Directory.Exists(target_dir))
                System.IO.Directory.CreateDirectory(target_dir);
            
            BuildActionRunner.Instance.TriggerPreBuild();
            BuildActionRunner.Instance.TriggerOnBuild();
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            if (res.Length > 0)
            {
                throw new Exception("BuildPlayer failure: " + res);
            }
            BuildActionRunner.Instance.TriggerPostBuild();
        }

        // Helper function for getting the command line arguments
        // Taken from: https://effectiveunity.com/articles/making-most-of-unitys-command-line.html
        private static string GetArg(string name)
        {
            var args = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == name && args.Length > i + 1)
                {
                    return args[i + 1];
                }
            }
            return null;
        }
    }
}
