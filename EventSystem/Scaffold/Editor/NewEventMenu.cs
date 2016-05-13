using UnityEngine;
using System.Collections;
using UnityEditor;
using Zephyr.EventSystem.Scaffold;
using System.Runtime.InteropServices;
using System.IO;

public class NewEventMenu
{
    [MenuItem("Assets/Create/Event")]
    private static void CreateEventFromMenu()
    {
        var path = GetSelectedPathOrFallback() + "/";
        CreateEvent(path);
    }

    private static void CreateEvent(string folderPath)
    {
        var eventDetails = new NewEventInput();
        eventDetails.path = folderPath;

        eventDetails.Show();
    }

    /// <summary>
    /// Retrieves selected folder on Project view.
    /// </summary>
    /// <returns></returns>
    public static string GetSelectedPathOrFallback()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return path;
    }
}
