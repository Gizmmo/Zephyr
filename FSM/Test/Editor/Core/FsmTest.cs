using System;
using Zephyr.StateMachine.Core;
using NUnit.Framework;

namespace Zephyr.StateMachine.Test.Editor.Core
{
    [TestFixture]
    [Category("FsmCore")]
    public abstract class FsmTest
    {
        private Fsm<ConcreteState> _fsm;

        [SetUp]
        public virtual void Init()
        {
            _fsm = new Fsm<ConcreteState>();
        }

        protected void AddStateAt(int state)
        {
            switch (state)
            {
                case 0:
                    _fsm.AddState<StateOne>();
                    break;
                case 1:
                    _fsm.AddState<StateTwo>();
                    break;
                case 2:
                    _fsm.AddState<StateThree>();
                    break;
                case 3:
                    _fsm.AddState<StateFour>();
                    break;
                case 4:
                    _fsm.AddState<StateFive>();
                    break;
            }
        }

        protected void RemoveStateAt(int state)
        {
            switch (state)
            {
                case 0:
                    _fsm.RemoveState<StateOne>();
                    break;
                case 1:
                    _fsm.RemoveState<StateTwo>();
                    break;
                case 2:
                    _fsm.RemoveState<StateThree>();
                    break;
                case 3:
                    _fsm.RemoveState<StateFour>();
                    break;
                case 4:
                    _fsm.RemoveState<StateFive>();
                    break;
            }
        }

        public abstract class AddAndRemovalTests : FsmTest
        {
            protected void AddStates(int stateCount)
            {
                //Act
                for (var i = 0; i < stateCount; i++)
                    AddStateAt(i);
            }

            protected void RemoveStates(int stateCount)
            {
                //Act
                for (var i = 0; i < stateCount; i++)
                    RemoveStateAt(i);
            }

            public class AddStateTests : AddAndRemovalTests
            {
                [Test]
                [TestCase(1, TestName = "OneStateAdd")]
                [TestCase(2, TestName = "TwoStateAdds")]
                [TestCase(3, TestName = "ThreeStateAdds")]
                [TestCase(4, TestName = "FourStateAdds")]
                [TestCase(5, TestName = "FiveStateAdds")]
                public void DoesAddingAGenericStateIncreasetheStateCount(int stateCount)
                {
                    //Arrange

                    //Act
                    AddStates(stateCount);

                    //Assert
                    Assert.AreEqual(stateCount, _fsm.StateCount);
                }
            }

            public class RemoveStateTests : AddAndRemovalTests
            {

                [Test]
                [TestCase(1, TestName = "OneStateRemove")]
                [TestCase(2, TestName = "TwoStateRemoves")]
                [TestCase(3, TestName = "ThreeStateRemoves")]
                [TestCase(4, TestName = "FourStateRemoves")]
                [TestCase(5, TestName = "FiveStateRemoves")]
                public void DoesRemovingAStateFromFsmDecreaseTheStateCount(int stateCount)
                {
                    var maxStates = 5;
                    //Arrange
                    AddStates(maxStates);

                    var count = maxStates - stateCount;


                    //Act
                    RemoveStates(stateCount);
                    Assert.AreEqual(count, _fsm.StateCount);
                }
            }

            [Test]
            public void DoesRemovingAStateThatIsNotInTheFsmThrowAStateDoesNotExistException()
            {
                Assert.Throws(typeof (StateDoesNotExistException), _fsm.RemoveState<StateFive>);
            }

            [Test]
            public void DoesRemovingTheCurrentStateThrowARemoveCurrentStateException()
            {
                //Arrange
                AddStateAt(0);
                _fsm.SetInitialState<StateOne>();

                //Act
                _fsm.Start();

                //Assert
                Assert.Throws(typeof (RemoveCurrentStateException), _fsm.RemoveState<StateOne>);
            }
        }

        public class InitalizeTests : FsmTest
        {
            [Test]
            public void DoesInitalStateSetTheInitialState()
            {
                //Arrange
                AddStateAt(0);

                //Act
                _fsm.SetInitialState<StateOne>();

                //Assert
                Assert.AreEqual(typeof (StateOne), _fsm.InitalState);
            }

            [Test]
            public void DoesSettingTheInitalStateWhenThatStateIsNotInTheFsmThrowStateDoesNotExistException()
            {
                //Assert
                Assert.Throws(typeof (StateDoesNotExistException), _fsm.SetInitialState<StateOne>);
            }
        }

        public class StartTests : FsmTest
        {
            public override void Init()
            {
                base.Init();
                AddStateAt(0);
            }

            [Test]
            public void IsCurrentStateNullBeforeStartIsCalled()
            {
                //Assert
                Assert.IsNull(_fsm.CurrentState);
            }

            [Test]
            public void DoesCallingStartWithoutAnInitialStateThrowAnInitalStateNullException()
            {
                //Assert
                Assert.Throws(typeof (InitalStateNullException), _fsm.Start);
            }

            [Test]
            public void DoesCallingStartWithAnIntialStateMakeThatStateTheCurrentState()
            {
                //Arrange
                _fsm.SetInitialState<StateOne>();

                //Act
                _fsm.Start();

                //Assert
                Assert.AreEqual(typeof (StateOne), _fsm.CurrentState.GetType());
            }
        }

        public class CurrestStateTests : FsmTest
        {
            public override void Init()
            {
                base.Init();
                AddStateAt(0);
                _fsm.SetInitialState<StateOne>();
                _fsm.Start();
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheSharedMethod()
            {
                //Assert
                Assert.AreEqual(0, _fsm.CurrentState.ReturnZero());
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheOverriddenMethod()
            {
                //Assert
                Assert.AreEqual(1, _fsm.CurrentState.ReturnStateNumber());
            }

            [Test]
            public void DoesSetCurrentStateChangeTheStateToThePassedType()
            {
                //Arrange
                _fsm.AddState<StateTwo>();

                //Act
                _fsm.SetCurrentState<StateTwo>();

                //Assert
                Assert.AreEqual(typeof (StateTwo), _fsm.CurrentState.GetType());
            }
        }

        public abstract class ConcreteState : StateMachine.Core.IState
        {
            public int ReturnZero()
            {
                return 0;
            }

            public abstract int ReturnStateNumber();
        }

        public class StateOne : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 1;
            }
        }

        public class StateTwo : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 2;
            }
        }

        public class StateThree : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 3;
            }
        }

        public class StateFour : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 4;
            }
        }

        public class StateFive : ConcreteState
        {
            public override int ReturnStateNumber()
            {
                return 5;
            }
        }
    }
}