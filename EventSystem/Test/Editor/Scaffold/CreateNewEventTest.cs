using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Zephyr.EventSystem.Scaffold;
using TreeEditor;
using System.IO;
using System.Runtime.ConstrainedExecution;
using UnityEditor;


namespace Zephyr.EventSystem.Editor.Test.Scaffold
{
    [TestFixture]
    [Category("CreateNewEvent")]
    public class CreateNewEventTest
    {
        private CreateNewEvent _createNewEvent;
        private string _event = "OpenSettings";
        private string _category = "Settings";
        private string TestFolder = Settings.EventSystem + "Test/Editor/Scaffold/TestData/";
        private readonly string _templateDefault = "using UnityEngine;\nusing System.Collections;\nusing Zephyr.EventSystem.Core;\n\npublic class testing\n{\n\t\n}";

        [SetUp]
        public void Init()
        {
            _createNewEvent = new CreateNewEvent(_event, _category, TestFolder);
            if (!Directory.Exists(TestFolder))
                Directory.CreateDirectory(TestFolder);
        }

        public void ClearTestCS()
        {
            var files = Directory.GetFiles(TestFolder);

            foreach (var file in files)
                File.Delete(file);
            
            AssetDatabase.Refresh();
        }

        [Test]
        [Category("Constructor")]
        public void DoesPropertiesGetSetInitially()
        {
            //Assert
            Assert.AreEqual(_event, _createNewEvent.Name);
            Assert.AreEqual(_category, _createNewEvent.Category);
        }

        [Test]
        [Category("ReadTemplate")]
        public void DoesReadTemplateReturnActionTemplateString()
        {
            //Act
            var data = _createNewEvent.ReadTemplate(CreateNewEvent.ActionListenerTemplate);

            //Assert
            Assert.IsNotNullOrEmpty(data);
        }

        [Test]
        [Category("ReadTemplate")]
        public void DoesReadTemplateEventTemplateString()
        {
            //Act
            var data = _createNewEvent.ReadTemplate(CreateNewEvent.EventTemplate);

            //Assert
            Assert.IsNotNullOrEmpty(data);
        }

        [Test]
        [Category("ReadTemplate")]
        public void DoesReadTemplateQueueActionTemplateString()
        {
            //Act
            var data = _createNewEvent.ReadTemplate(CreateNewEvent.QueueActionTemplate);

            //assert
            Assert.IsNotNullOrEmpty(data);
        }

        [Test]
        [Category("ReplaceAttributes")]
        public void DoesReplaceChangeTheNameKey()
        {
            //Arrange
            var data = "something%NAME%here";
            var expectedData = "somethingOpenSettingshere";

            //Act
            var actualData = _createNewEvent.ReplaceAttributes(data);

            //Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        [Category("ReplaceAttributes")]
        public void DoesReplaceChangeTheCategoryKey()
        {
            //Arrange
            var data = "something%CATEGORY%here";
            var expectedData = "somethingSettingshere";

            //Act
            var actualData = _createNewEvent.ReplaceAttributes(data);

            //Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [Test]
        [Category("WriteTemplate")]
        public void DoesWritingTemplatePlaceFileInLocation()
        {
            //Act
            _createNewEvent.WriteTemplate(TestFolder, _event, _templateDefault);

            //Assert
            Assert.IsTrue(File.Exists(TestFolder + _event + ".cs"));

            //Cleanup
            ClearTestCS();
        }

        [Test]
        [Category("WriteTemplate")]
        public void DoesWritingTemplateStoreDataPassed()
        {
            //Arrange
            _createNewEvent.WriteTemplate(TestFolder, _event, _templateDefault);

            //Act
            var text = _createNewEvent.ReadTemplate(TestFolder + _event + ".cs");

            //Assert
            Assert.AreEqual(_templateDefault, text); 

            //Cleanup
            ClearTestCS();
        }

        [Test]
        [Category("Scaffold")]
        public void DoesScaffoldCreateScripts()
        {
            //Act
            _createNewEvent.Scaffold();

            var files = Directory.GetFiles(TestFolder);

            //Assert
            Assert.IsTrue(File.Exists(TestFolder + _event + "Listener.cs"));
            Assert.IsTrue(File.Exists(TestFolder + _event + "Event.cs"));
            Assert.IsTrue(File.Exists(TestFolder + "Queue" + _event + ".cs"));

            ClearTestCS();
        }

        [Test]
        [Category("CleanName")]
        public void DoesNameGetMadeToUpperCase()
        {
            //Act
            var test = new CreateNewEvent("test", _category, TestFolder);

            //Assert
            Assert.AreEqual("Test", test.Name);
        }

        [Test]
        [Category("CleanName")]
        public void DoesNameLoseWhiteSpace()
        {
            //Act
            var test = new CreateNewEvent("test something", _category, TestFolder);

            //Assert
            Assert.AreEqual("TestSomething", test.Name);
        }
    }
}
