using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using UnityEditor.VersionControl;
using System;
using System.Globalization;

namespace Zephyr.EventSystem.Scaffold
{
    public class CreateNewEvent
    {
        public static readonly string Templates = Zephyr.Settings.EventSystem + "Scaffold/Templates/";
        public static readonly string ActionListenerTemplate = Templates + "ActionListener.txt";
        public static readonly string EventTemplate = Templates + "Event.txt";
        public static readonly string QueueActionTemplate = Templates + "QueueAction.txt";
        private readonly string _nameKey = "%NAME%";
        private readonly string _categoryKey = "%CATEGORY%";

        public string Name { get; private set; }

        public string Category { get; private set; }

        public string Path { get; private set; }

        public CreateNewEvent(string name, string category, string path)
        {
            if (String.IsNullOrEmpty(name))
                name = "Default";
            Name = CleanName(name);

            if (String.IsNullOrEmpty(category))
                category = "Game Event Listeners";
            
            Category = category;

            if (String.IsNullOrEmpty(path))
                path = "Assets/Scripts/";
            Path = path;
        }

        public void Scaffold()
        {
            CreateScript(ActionListenerTemplate, Path, Name + "Listener");
            CreateScript(EventTemplate, Path, Name + "Event");
            CreateScript(QueueActionTemplate, Path, "Queue" + Name);
        }

        public string CleanName(string str)
        {
            str = str.Trim();

            if (str.IndexOf(" ") != -1)
            {
                str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
            }
            else
            {
                str = char.ToUpper(str[0]) + str.Substring(1);
            }

            return str.Replace(" ", string.Empty);
        }


        public void CreateScript(string templatePath, string savePath, string name)
        {
            var template = ReadTemplate(templatePath);
            template = ReplaceAttributes(template);
            WriteTemplate(savePath, name, template);
        }

        public void WriteTemplate(string path, string scriptName, string template)
        {
            //Let's create a new Script named "`template`.cs"
            using (StreamWriter sw = new StreamWriter(string.Format(path + "{0}.cs", scriptName)))
            {
                sw.Write(template);
            }

            //Refresh the Asset Database
            AssetDatabase.Refresh();
        }

        public string ReplaceAttributes(string text)
        {
            text = text.Replace(_nameKey, Name);
            text = text.Replace(_categoryKey, Category);
            return text;
        }

        public string ReadTemplate(string templatePath)
        {
            TextAsset text = AssetDatabase.LoadAssetAtPath(templatePath, typeof(TextAsset)) as TextAsset;
            return text.text;
        }
    }
}
