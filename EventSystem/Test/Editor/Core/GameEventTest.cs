using NUnit.Framework;
using Zephyr.EventSystem.Core;

namespace Zephyr.EventSystem.Editor.Test.Core
{
  [TestFixture]
  [Category("Game Event")]
  public class GameEventTest
  {
    private GameEvent _gameEvent;

    [SetUp]
    public void Init()
    {
      _gameEvent = new GameEvent();
    }

    [Test]
    public void IsNameEqualsTheTypeName()
    {
      Assert.AreEqual(_gameEvent.Name, _gameEvent.GetType().Name);
    }
  }
}