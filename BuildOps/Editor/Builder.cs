using System;
using System.Collections.Generic;
using UnityEditor;
using BuildOps.BuildActions;

namespace Zephyr.BuildOps
{
    public class Builder
    {
        static string[] SCENES = FindEnabledEditorScenes();

        static string _appName = PlayerSettings.productName;
        const string _targetDir = "~/";

        static void PerformBuild()
        {
            GenericBuild(
                SCENES,
                GetArg("-targetPath"),
                (BuildTarget)Enum.Parse(typeof(BuildTarget), GetArg("-buildTarget")),
                (BuildOptions)Enum.Parse(typeof(BuildOptions), GetArg("-buildOptions") ?? "None"));
        }

        public static void IncreaseBuildVersion(bool major = false)
        {
            var version = PlayerSettings.bundleVersion;
            var data = version.Split('.');
            if(major)
            {
                var num = int.Parse(data[1]) + 1;
                data[1] = num + "";
            }
            else
            {
                var num = int.Parse(data[2]) + 1;
                data[2] = num + "";
            }
            version = data[0] + "." + data[1] + "." + data[2];
            PlayerSettings.bundleVersion = version;  

        }

        public static void PerformIOSBuild(string path = _targetDir)
        {
            var tempDirectory = _targetDir + "/tempDir";
            if (System.IO.Directory.Exists(tempDirectory))
            {
                FileUtil.DeleteFileOrDirectory(tempDirectory); 
            }
            
            GenericBuild(SCENES, tempDirectory, BuildTarget.iOS, BuildOptions.None);
        }

        public static void PerformAndroidBuild(string path = _targetDir)
        {
            string target_dir = CreateDirectories("android", path) + ".apk";
            GenericBuild(SCENES, target_dir, BuildTarget.Android, BuildOptions.None);
        }

        public static void PerformWindowBuild(string path = _targetDir)
        {
            string target_dir = CreateDirectories("windows", path) + ".exe";
            GenericBuild(SCENES, target_dir, BuildTarget.StandaloneWindows64, BuildOptions.None);
        }

        private static string CreateDirectories(string platform, string path)
        {
            var appPath = path + "/" + _appName;

            if (!System.IO.Directory.Exists(appPath))
                System.IO.Directory.CreateDirectory(appPath);

           var dateTime = DateTime.Now;
            var date = dateTime.ToString("yyyyMMdd");

            var versionPath = appPath + "/" + PlayerSettings.bundleVersion + "." + date;

            if (!System.IO.Directory.Exists(versionPath))
                System.IO.Directory.CreateDirectory(versionPath);


            var platformPath = versionPath + "/" + platform;

            if (!System.IO.Directory.Exists(platformPath))
                System.IO.Directory.CreateDirectory(platformPath);
  
           
        
            return platformPath + "/" + _appName + "_" + PlayerSettings.bundleVersion + "." + date;


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
