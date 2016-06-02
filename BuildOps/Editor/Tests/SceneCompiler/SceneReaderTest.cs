using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zephyr.BuildOps.SceneCompiler;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Zephyr.BuildOps.Tests.SceneCompilerTest
{
    [TestFixture]
    public class SceneReaderTest
    {
        private static readonly string TestFolder = Settings.BuildOps + "Editor/Tests/";
        private static readonly string SceneCompilerPath = TestFolder + "Scenes/";
        private static readonly string SceneContainerXml = TestFolder + "Fixtures/Resources/testData.xml";
        private static readonly string PlatformContainerXml = TestFolder + "Fixtures/Resources/testData_build.xml";
        private static readonly string BaseScene = SceneCompilerPath + "Base.unity";
        private SceneReader _reader;

        [SetUp]
        public void Init()
        {
            _reader = new SceneReader(SceneContainerXml);
        }

        [Test]
        [Category("LoadSceneContainerFromXml")]
        public void DoesLoadContainerReadFromXml()
        {
            //Act
            var container = SceneDeserializer.LoadSceneContainerFromXml(SceneContainerXml);

            //Assert
            Assert.AreEqual(2, container.Scenes.Count);
        }

        [Test]
        [Category("LoadSceneContainerFromXml")]
        public void DoesLoadContainerNotReturnNull()
        {
            //Act
            var container = SceneDeserializer.LoadSceneContainerFromXml(SceneContainerXml);

            //Assert
            Assert.IsNotNull(container);
        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateCorrectNumberOfNestedScenes()
        {
            //Arrange
            var container = SceneDeserializer.LoadSceneContainerFromXml(SceneContainerXml);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(2, EditorSceneManager.sceneCount);
        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateBaseScene()
        {
            //Arrange
            var container = SceneDeserializer.LoadSceneContainerFromXml(SceneContainerXml);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(container.Scenes[0].Name, EditorSceneManager.GetSceneAt(0).name);

        }

        [Test]
        [Category("LoadSceneContainer")]
        public void DoesLoadSceneContainerCreateNestedScene()
        {
            //Arrange
            var container = SceneDeserializer.LoadSceneContainerFromXml(SceneContainerXml);

            //Act
            _reader.LoadSceneContainer(container);

            //Assert
            Assert.AreEqual(container.Scenes[1].Name, EditorSceneManager.GetSceneAt(1).name);

        }

        [Test]
        [Category("LoadPlatformContainerFromXML")]
        public void DoesLoadPlatformContainerCreateScenes()
        {
            //Act
            var container = SceneDeserializer.LoadPlatformContainerFromXml(PlatformContainerXml);

            //Assert
            Assert.AreEqual(3, container.Scenes.Count);
            
        }

        [Test]
        [Category("CollectBuildSceneSettings")]
        public void DoesCollectBuildSceneSettingsCreateCorrectAmountOfScenes()
        {
            //Arrage
            var container = SceneDeserializer.LoadPlatformContainerFromXml(PlatformContainerXml);

            //Act
            var scenes = _reader.CollectBuildSceneSettings(container);

            //Assert
            Assert.AreEqual(3, scenes.Length);
        }

        [Test]
        [Category("CollectBuildSceneSettings")]
        public void DoesCollectSettingsHaveCorrectPaths()
        {
            //Arrage
            var container = SceneDeserializer.LoadPlatformContainerFromXml(PlatformContainerXml);

            //Act
            var scenes = _reader.CollectBuildSceneSettings(container);

            //Assert
            Assert.AreEqual(container.Scenes[0].Scenes[0].Path, scenes[0].path);
            Assert.AreEqual(container.Scenes[1].Scenes[0].Path, scenes[1].path);
        }

        [Test]
        [Category("LoadBuildSettings")]
        public void DoesLoadBuildSettingsUpdateBuildSettingScenes()
        {
            //Act
            _reader.LoadBuildSettings(SceneContainerXml);

            //Assert
            Assert.AreEqual(BaseScene, EditorBuildSettings.scenes[0].path);
        }

        [Test]
        [Category("LoadScene")]
        public void DoesLoadSceneLoadCorrectNumberOfScenes()
        {
            //Act
            _reader.LoadScene(SceneContainerXml);

            //Assert
            Assert.AreEqual(2, EditorSceneManager.sceneCount);
        }
    }
}
