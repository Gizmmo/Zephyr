using System.Collections.Generic;
using System.IO;
using Zephyr.BuildOps.SceneCompiler;
using UnityEditor.SceneManagement;
using NUnit.Framework;
using UnityEditor;
using Zephyr.BuildOps.SceneCompiler.Exceptions;

namespace Zephyr.BuildOps.Tests.SceneCompilerTest
{
    [TestFixture]
    [Category("NestedScene Compiler Tests")]
    public class SceneCompilerTest
    {
        private static readonly string SceneCompilerPath = Settings.BuildOps + "Editor/Tests/Scenes/";
        private static readonly string XmlTestFile = SceneCompilerPath + "testFile.xml";
        private static readonly string BaseScene = SceneCompilerPath + "Base.unity";
        private static readonly string SceneWithCube = SceneCompilerPath + "SceneWithCube.unity";
        private static readonly string SceneWithSphere = SceneCompilerPath + "SceneWithSphere.unity";
        private const string Path = "Assets/Resources/BuildOpsData/mobile.xml";
        private const string Platform = "mobile";
        private SceneCompiler.SceneWriter _writer;

        [SetUp]
        public void Init()
        {
            _writer = new SceneCompiler.SceneWriter(Path);
            EditorSceneManager.OpenScene(BaseScene);
        }

        [Test]
        [Category("CompileScene")]
        public void DoesCompileSceneReadBaseScene()
        {
            var scenes = _writer.CompileScene();
            Assert.AreEqual(1, scenes.Length);
            Assert.AreEqual("Base", scenes[0].name);
        }

        [Test]
        [Category("CompileScene")]
        public void IsCompileSceneNotNull()
        {
            var scenes = _writer.CompileScene();
            Assert.IsNotNull(scenes);
        }

        [Test]
        [Category("CompileScene")]
        public void DoesCompileSceneReadNestedScenes()
        {
            EditorSceneManager.OpenScene(SceneWithCube, OpenSceneMode.Additive);
            var scenes = _writer.CompileScene();
            Assert.AreEqual(2, scenes.Length);
        }

        [Test]
        [Category("CompileScene")]
        public void IsFirstIndexTheRootSceneInCompilerScene()
        {
            EditorSceneManager.OpenScene(SceneWithCube, OpenSceneMode.Additive);
            var scenes = _writer.CompileScene();
            Assert.AreEqual("Base", scenes[0].name);
        }

        [Test]
        [Category("ParsedScene")]
        public void DoesParsedSceneReturnEqualScenesToCompiled()
        {
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);
            Assert.AreEqual(scenes.Length, parsedScenes.Count);
        }

        [Test]
        [Category("ParsedScene")]
        public void IsParseSceneNotNull()
        {
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);
            Assert.IsNotNull(parsedScenes);
        }

        [Test]
        [Category("ParsedScene")]
        public void IsParsedScenesOfTypeScene()
        {
            //Arrage
            var scenes = _writer.CompileScene();

            //Act
            var parsedScenes = _writer.ParseScenes(scenes);

            //Assert
            Assert.AreEqual(typeof(NestedScene), parsedScenes[0].GetType());
        }

        [Test]
        [Category("CreateSceneContainer")]
        public void DoesReturnSceneContainer()
        {
            //Arrange
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);

            //Act
            var container = _writer.CreateSceneContainer(parsedScenes);

            //Assert
            Assert.AreEqual(typeof(SceneContainer), container.GetType());
        }

        [Test]
        [Category("CreateSceneContainer")]
        public void SceneContainerIsNotNull()
        {
            //Arrange
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);

            //Act
            var container = _writer.CreateSceneContainer(parsedScenes);

            //Assert
            Assert.IsNotNull(container);
        }

        [Test]
        [Category("CreatePlatformContainer")]
        public void PlatformContainerIsNotNull()
        {
            //Arrage
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);
            var sceneContainer = _writer.CreateSceneContainer(parsedScenes);

            //Act
            var pContainer = _writer.CreatePlatformContainer(new List<SceneContainer>() {sceneContainer}, Platform);

            //Assert
            Assert.IsNotNull(pContainer);
        }

        [Test]
        [Category("CreatePlatformContainer")]
        public void PlatformContainerReturnCorrectType()
        {
            //Arrage
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);
            var sceneContainer = _writer.CreateSceneContainer(parsedScenes);

            //Act
            var pContainer = _writer.CreatePlatformContainer(new List<SceneContainer>() { sceneContainer }, Platform);

            //Assert
            Assert.AreEqual(typeof(PlatformContainer), pContainer.GetType());
        }

        [Test]
        [Category("WriteXmlScene")]
        public void DoesWriteToGivenDirectory()
        {
            //Arrange
            var scenes = _writer.CompileScene();
            var parsedScenes = _writer.ParseScenes(scenes);
            var container = _writer.CreateSceneContainer(parsedScenes);
            var platformContainer = _writer.CreatePlatformContainer(new List<SceneContainer>() {container}, Platform);

            //Act
            _writer.WriteXmlScene(platformContainer, XmlTestFile);

            //Assert
            Assert.IsTrue(File.Exists(XmlTestFile));
            File.Delete(XmlTestFile);
        }

        [Test]
        [Category("CompileCurrentScene")]
        public void DoesCompileCurrentSceneReturnSceneContainer()
        {
            //Act
            var container = _writer.CompileCurrentScene();

            //Assert
            Assert.AreEqual(typeof(SceneContainer), container.GetType());
        }

        [Test]
        [Category("CompileCurrentScene")]
        public void DoesCompileReturnCorrectScenes()
        {
            //Arrange
            EditorSceneManager.OpenScene(SceneWithCube, OpenSceneMode.Additive);

            //Act
            var container = _writer.CompileCurrentScene();

            //Assert
            Assert.AreEqual(2, container.Scenes.Count);
        }

        [Test]
        [Category("ParsePlatformFromPath")]
        public void DoesParseNameFromPath()
        {
            //Act
            var name = SceneCompiler.SceneWriter.ParsePlatformFromPath(Path);

            //Assert
            Assert.AreEqual("mobile", name);
        }

        [Test]
        [Category("ParsePlatformFromPath")]
        public void ExceptionThrownOnShortPath()
        {
            //Arrange
            const string path = "mobile.xml";

            //Assert
            Assert.Throws<IncorrectPathException>(() => { SceneCompiler.SceneWriter.ParsePlatformFromPath(path); });
        }

        [Test]
        [Category("ParsePlatformFromPath")]
        public void ExceptionThrownOnNoResourcesInPath()
        {
            //Arrange
            const string path = "Assets/someplace/mobile.xml";

            //Assert
            Assert.Throws<IncorrectPathException>(() => { SceneCompiler.SceneWriter.ParsePlatformFromPath(path); });
        }

        [Test]
        [Category("ParsePlatformFromPath")]
        public void ExceptionThrownOnNoExtension()
        {
            //Arrange
            const string path = "Assets/Resources/mobile";

            //Assert
            Assert.Throws<IncorrectExtensionException>(() => { SceneCompiler.SceneWriter.ParsePlatformFromPath(path); });
        }

        [Test]
        [Category("ParsePlatformFromPath")]
        public void ExceptionThrownOnNoXml()
        {
            //Arrange
            const string path = "Assets/Resources/mobile.something";

            //Assert
            Assert.Throws<IncorrectExtensionException>(() => { SceneCompiler.SceneWriter.ParsePlatformFromPath(path); });
        }

        [Test]
        [Category("AddBuildSettingsExtension")]
        public void DoesBuildGetAddedToPath()
        {
            //Arrange
            const string path = "Assets/Resources/someplace/mobile.xml";
            const string expectedPath = "Assets/Resources/someplace/mobile_build.xml";

            //Act
            var newPath = SceneCompiler.SceneWriter.AddBuildSettingsExtension(path);

            //Assert
            Assert.AreEqual(expectedPath, newPath);
        }

        [Test]
        [Category("CollectScenePathsFromBuildSettings")]
        public void DoesCollectScenePathsReturnAnArrayOfNamesFromPath()
        {
            //Arrage
            var scenes = new EditorBuildSettingsScene[2];
            scenes[0] = new EditorBuildSettingsScene(SceneWithCube, false);
            scenes[1] = new EditorBuildSettingsScene(BaseScene, false);

            //Act
            var paths = _writer.CollectScenePathsFromBuildSettings(scenes);

            //Assert
            Assert.AreEqual(SceneWithCube, paths[0]);
        }

        [Test]
        [Category("CollectScenePathsFromBuildSettings")]
        public void DoesCollectSceneReturnData()
        {
            //Arrage
            var scenes = new EditorBuildSettingsScene[2];
            scenes[0] = new EditorBuildSettingsScene(SceneWithCube, false);
            scenes[1] = new EditorBuildSettingsScene(BaseScene, false);

            //Act
            var paths = _writer.CollectScenePathsFromBuildSettings(scenes);

            //Assert
            Assert.IsNotNull(paths);
            Assert.AreEqual(2, paths.Length);
        }

        [Test]
        [Category("CollectAllScenes")]
        public void DoesCompileAllMakeAListWithCorrectSizeToScenes()
        {
            //Arrange
            var paths = new string[3];
            paths[0] = BaseScene;
            paths[1] = SceneWithCube;
            paths[2] = SceneWithSphere;

            //Act
            var container = _writer.CompileAllScenes(paths);

            //Assert
            Assert.AreEqual(3, container.Count);
        }

        [Test]
        [Category("CollectAllScenes")]
        public void DoesCompileAllStoreCorrectPathData()
        {
            //Arrange
            var paths = new string[3];
            paths[0] = BaseScene;
            paths[1] = SceneWithCube;
            paths[2] = SceneWithSphere;

            //Act
            var container = _writer.CompileAllScenes(paths);

            //Assert
            Assert.AreEqual(BaseScene, container[0].Scenes[0].Path);
        }
    }
}