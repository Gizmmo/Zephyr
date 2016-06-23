using Zephyr.StateMachine.Core;
using NUnit.Framework;

namespace Zephyr.StateMachine.Test.Editor.Core
{
    [TestFixture]
    [Category("FsmCore")]
    public class FsmTest
    {
        private Fsm _fsm;

        [SetUp]
        public void Init() { _fsm = new Fsm(); }

        [Test]
        public void AddingAStateToAnFsmShouldIncraseTheAmountOfStatesByOne()
        {
            var currentStateCount = _fsm.StateCount;
            _fsm.AddState(new IdleState());
            Assert.AreEqual(currentStateCount + 1, _fsm.StateCount);
        }
    }

    public class IdleState : StateMachine.Core.IState
    {
    }
}