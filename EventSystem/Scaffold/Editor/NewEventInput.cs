using UnityEngine;
using UnityEditor;
using Zephyr.EventSystem.Scaffold;

class NewEventInput : EditorWindow {

    string _eventName;
    string _categoryName = "";
    public string path;

    void OnGUI() {
        _eventName = EditorGUILayout.TextField("Event Name", _eventName);
        _categoryName = EditorGUILayout.TextField("Category Name", _categoryName);

        if (GUILayout.Button("Save Event")) {
            OnClickSavePrefab();
            GUIUtility.ExitGUI();
        }
    }


    void OnClickSavePrefab() {
        _eventName = _eventName.Trim();
        _categoryName = _categoryName.Trim();

        if (string.IsNullOrEmpty(_eventName)) {
            EditorUtility.DisplayDialog("Unable to save Event", "Please specify a valid event name.", "Close");
            return;
        }

        var fileEvent = new CreateNewEvent(_eventName, _categoryName, path);
        fileEvent.Scaffold();

        Close();
    }

}