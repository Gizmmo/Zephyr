using UnityEngine;
using System.Collections;
using Zephyr.BuildOps.SceneCompiler;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.VersionControl;

namespace BuildOps.BuildActions
{
    public sealed class BuildActionRunner
    {

        public System.Action PreBuild;
        public System.Action OnBuild;
        public System.Action PostBuild;

        private readonly static BuildActionRunner _runner = new BuildActionRunner();

        public void TriggerPreBuild()
        {
            RunBuildType<OnPreBuild>();
        }

        public void TriggerOnBuild()
        {
            RunBuildType<OnBuild>();
        }

        public void TriggerPostBuild()
        {
            RunBuildType<OnPostBuild>();
        }

        public static void RunBuildType<T>() where T : Attribute
        {
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in ass)
            {
                var res = BuildActionRunner.GetTypesWithAttribute<T>(a);

                foreach (var r in res)
                {
                    IBuilderAction instance = (IBuilderAction)Activator.CreateInstance(r);
                    instance.Run();
                }
            }
        }

        public static IEnumerable<Type> GetTypesWithAttribute<T>(Assembly assembly) where T : Attribute {
            foreach(Type type in assembly.GetTypes()) {
                if (type.GetCustomAttributes(typeof(T), true).Length > 0) {
                    yield return type;
                }
            }
        }

        public static BuildActionRunner Instance
        {
            get
            {
                return _runner;
            }
        }

    }
}

