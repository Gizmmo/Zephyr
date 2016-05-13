using UnityEditor;

namespace Zephyr
{
  public static class Settings
  {
		public static string Zephyr { get { return "Assets/Zephyr/"; } }
        public static string EventSystem { get { return Zephyr + "EventSystem/"; } }
        public static string BuildOps { get { return Zephyr + "BuildOps/"; } }
        public static string ResourceBuildOpsData { get { return "Assets/Resources/BuildOpsData/"; } }
        public static string MobileBuildOpsFile { get { return ResourceBuildOpsData + "mobile.xml"; } }
        public static string StandaloneBuildOpsFile { get { return ResourceBuildOpsData + "standalone.xml"; } }
    }
}
