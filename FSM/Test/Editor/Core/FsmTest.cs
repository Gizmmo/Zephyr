using Zephyr.StateMachine.Core;
using NUnit.Framework;

namespace Zephyr.StateMachine.Test.Editor.Core
{
    [TestFixture]
    [Category("FsmCore")]
    public abstract class FsmTest
    {
        private Fsm _fsm;

        [SetUp]
        public virtual void Init()
        {
            _fsm = new Fsm();
        }

        public abstract class AddAndRemovalTests : FsmTest
        {
            private static object[] _stateCases =
            {
                new object[] {new StateOne()},
                new object[] {new StateOne(), new StateTwo()},
                new object[] {new StateOne(), new StateTwo(), new StateThree()},
                new object[] {new StateOne(), new StateTwo(), new StateThree(), new StateFour()},
                new object[] {new StateOne(), new StateTwo(), new StateThree(), new StateFour(), new StateFive()}
            };

            public class AddStateTests : AddAndRemovalTests {
                [Test, TestCaseSource("_stateCases")]
                public void DoesAddingAStateToAnFsmShouldIncreaseTheAmountOfStatesByOne(object[] states)
                {
                    //Arrange
                    var count = 0;
                    
                    //Act
                    foreach (var state in states)
                    {
                        _fsm.AddState(state as StateMachine.Core.IState);
                        count++;

                        //Assert
                        Assert.AreEqual(count, _fsm.StateCount);
                    }
                }
            }

            public class RemoveStateTests : AddAndRemovalTests {
                [Test]
                public void DoesRemovingAStateFromFsmUsingGenericDecreaseTheAmountOfStatesByOne()
                {
                    var currentStateCount = _fsm.StateCount;
                    _fsm.RemoveState<StateOne>();
                    Assert.AreEqual(currentStateCount - 1, _fsm.StateCount);
                }

                [Test, TestCaseSource("_stateCases")]
                public void DoesRemovingAStateFromFsmDecreaseTheAmountOfStatesByOne(object[] states)
                {
                    //Arrange
                    foreach (var state in states)
                        _fsm.AddState(state as StateMachine.Core.IState);

                    var count = states.Length;

                    //Act
                    foreach (var state in states)
                    {
                        _fsm.RemoveState(state.GetType());
                        count--;

                        //Assert
                        Assert.AreEqual(count, _fsm.StateCount);
                    }
                }
            }
        }


        public class AllTests : FsmTest
        {
            public override void Init()
            {
                base.Init();
                _fsm.AddState<StateOne>();
            }

            [Test]
            public void DoesInitalStateSetTheInitialState()
            {
                _fsm.SetInitialState<StateOne>();
                Assert.AreEqual(typeof (StateOne), _fsm.InitalState);
            }

            [Test]
            public void IsCurrentStateNullBeforeStartIsCalled()
            {
                Assert.IsNull(_fsm.CurrentState);
            }

            [Test]
            public void DoesCallingStartWithoutAnInitialStateThrowAnInitalStateNullException()
            {
                Assert.Throws(typeof (InitalStateNullException), _fsm.Start);
            }

            [Test]
            public void DoesCallingStartWithAnIntialStateMakeThatStateTheCurrentState()
            {
                _fsm.SetInitialState<StateOne>();
                _fsm.Start();
                Assert.AreEqual(typeof(StateOne), _fsm.CurrentState.GetType());
            }

//        [Test]
//        public void DoesCallingAnInheritedMethodCallTheCurrectStateMethod()
//        {
//            Assert.AreEqual(0, _fsm.CurrentState.ReturnZero());
//        }
        }

        public class StateOne : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }
        }

        public class StateTwo : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }
        }

        public class StateThree : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }
        }

        public class StateFour : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }
        }

        public class StateFive : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }
        }
    }
}