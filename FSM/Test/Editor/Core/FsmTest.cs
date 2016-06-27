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
                    _fsm.AddState(new StateOne());
                    break;
                case 1:
                    _fsm.AddState(new StateTwo());
                    break;
                case 2:
                    _fsm.AddState(new StateThree());
                    break;
                case 3:
                    _fsm.AddState(new StateFour());
                    break;
                case 4:
                    _fsm.AddState(new StateFive());
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

        protected void AddSimpleState()
        {
            _fsm.AddState(new StateOne());
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
                public void DoesAddingAGenericStateIncreasetheStateCount(int amountOfStatesAdded)
                {
                    //Arrange

                    //Act
                    AddStates(amountOfStatesAdded);

                    //Assert
                    Assert.That(_fsm.StateCount, Is.EqualTo(amountOfStatesAdded));
                }

                [Test]
                public void DoesAddingAStateThatAlreadyExistsThrowADuplicateStateException()
                {
                    //Arrange
                    AddSimpleState();

                    //Act

                    //Assert
                    Assert.Throws<DuplicateStateException>(AddSimpleState);
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
                    //Arrange
                    const int maxStates = 5;
                    AddStates(maxStates);
                    var expectedRemainingStatesCount = maxStates - stateCount;

                    //Act
                    RemoveStates(stateCount);

                    //Assert
                    Assert.That(_fsm.StateCount, Is.EqualTo(expectedRemainingStatesCount));
                }
            }

            [Test]
            public void DoesRemovingAStateThatIsNotInTheFsmThrowAStateNotFoundException()
            {
                Assert.Throws<StateNotFoundException>(_fsm.RemoveState<StateFive>);
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
                Assert.Throws<RemoveCurrentStateException>(_fsm.RemoveState<StateOne>);
            }
        }

        public class InitalizeTests : FsmTest
        {
            [Test]
            public void DoesInitalStateSetTheInitialState()
            {
                //Arrange
                var initalStateType = typeof(StateOne);
                AddStateAt(0);

                //Act
                _fsm.SetInitialState<StateOne>();

                //Assert
                Assert.That(_fsm.InitalState, Is.EqualTo(initalStateType));
            }

            [Test]
            public void DoesSettingTheInitalStateWhenThatStateIsNotInTheFsmThrowStateNotFoundException()
            {
                //Assert
                Assert.Throws<StateNotFoundException>(_fsm.SetInitialState<StateOne>);
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
                Assert.That(_fsm.State, Is.Null);
            }

            [Test]
            public void DoesCallingStartWithoutAnInitialStateThrowAnInitalStateNullException()
            {
                //Assert
                Assert.Throws<InitalStateNullException>(_fsm.Start);
            }

            [Test]
            public void DoesCallingStartWithAnIntialStateMakeThatStateTheCurrentState()
            {
                //Arrange
                var initialStateType = typeof(StateOne);


                //Act
                _fsm.SetInitialState<StateOne>();
                _fsm.Start();
                var currentStateType = _fsm.State.GetType();

                //Assert
                Assert.That(currentStateType, Is.EqualTo(initialStateType));
            }
        }

        public class CurrentStateTests : FsmTest
        {
            public override void Init()
            {
                base.Init();
                AddStateAt(0);
                _fsm.SetInitialState<StateOne>();
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheSharedMethod()
            {
                //Arrange
                const int returnNumber = 0;

                //Act
                _fsm.Start();
                var sharedMethodWasCalled = returnNumber == _fsm.State.ReturnZero();

                //Assert
                Assert.That(sharedMethodWasCalled);
            }

            [Test]
            public void DoesCallingACurrentStateMethodCallTheOverriddenMethod()
            {
                //Arrange
                const int returnNumber = 1;

                //Act
                _fsm.Start();
                var overiddenMethodWasCalled = returnNumber == _fsm.State.ReturnStateNumber();

                //Assert
                Assert.That(overiddenMethodWasCalled);
            }

            [Test]
            public void DoesSettingToCurrentStateCauseOnEntryToBeCalled()
            {
                //Arrange

                //Act
                _fsm.Start();
                var entryWasCalled = _fsm.State.IsEntryCalled;

                //Assert
                Assert.That(entryWasCalled);
            }
        }

        public abstract class TransitionTests : FsmTest
        {
            protected static bool TriggerCalled;

            public override void Init()
            {
                base.Init();
                TriggerCalled = false;
            }

            public class AddTransitionTests : TransitionTests
            {
                [Test]
                public void DoesAddingATransitionBetweenTwoStatesNotReturnAnError()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    //Act

                    //Assert
                    Assert.DoesNotThrow(AddSimpleTransition);
                }

                private void AddSimpleTransition()
                {
                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                }

                [Test]
                [TestCase(0, TestName = "StateFrom exists, StateTo does not")]
                [TestCase(1, TestName = "StateFrom does not exists, StateTo does")]
                [TestCase(2, TestName = "StateFrom does not exists, StateTo does not exist")]
                public void DoesAddingATransitionBetweenAStateThatExistsAndOneThatDoesNotReturnAStateNotFoundException(
                    int state)
                {
                    //Arrange
                    AddStateAt(state);

                    //Act

                    //Assert
                    Assert.Throws<StateNotFoundException>(AddSimpleTransition);
                }

                [Test]
                public void DoesAddingTheSameTransitionToTheSameStateThrowADuplicateStateTransitionException()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    //Act
                    AddSimpleTransition();

                    //Assert
                    Assert.Throws<DuplicateStateTransitionException>(AddSimpleTransition);
                }
            }

            public class TriggerTransitionTests : TransitionTests
            {
                [Test]
                public void DoesTriggerTransitionCallTheTriggerMethodOfThePassedTrigger()
                {
                    //Arrange
                    AddStateAt(0);
                    AddStateAt(1);

                    _fsm.AddTransition<StateOne, StateTwo>(new StateOneToStateTwoTransition());
                    _fsm.SetInitialState<StateOne>();
                    _fsm.Start();

                    //Act
                    _fsm.TriggerTransition<StateOneToStateTwoTransition>();

                    //Assert
                    Assert.That(TriggerCalled, Is.True);
                }

                [Test]
                public void DoesTriggerTransitionBeforeRunningStartReturnAStateMachineNotStartedException()
                {
                    
                }
            }
            

            public class StateOneToStateTwoTransition : ITransition
            {
                public void Trigger()
                {
                    TriggerCalled = true;
                }
            }
        }


        public abstract class ConcreteState : IState
        {
            public bool IsEntryCalled { get; private set; }

            public int ReturnZero()
            {
                return 0;
            }

            public abstract int ReturnStateNumber();

            public virtual void OnEntry()
            {
                IsEntryCalled = true;
            }
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