using NUnit.Framework;
using Zephyr.StateMachine.Core;

namespace Zephyr.StateMachine.Test.Editor.Core
{
    [TestFixture]
    [Category("FsmStateContainer")]
    public class StateContainerTest
    {
        private StateContainer<TestState> _container;

        [SetUp]
        public virtual void Init()
        {
        }

        protected void AddTransition(int transitionNum)
        {
            switch (transitionNum) {
                case 0:
                    _container.AddTransition<TransitionOne, StateTwo>();
                    break;
                case 1:
                    _container.AddTransition<TransitionTwo, StateTwo>();
                    break;
                case 2:
                    _container.AddTransition<TransitionThree, StateTwo>();
                    break;
            }
        }

        public class InitalizationTests : StateContainerTest
        {
            [Test]
            public void DoesInitalizingAStateContainerWithAStateMakeTheStatePropertyThatState()
            {
                //Arrange
                var stateType = typeof(TestState);
                //Act
                _container = new StateContainer<TestState>();
                var containerStateType = _container.State.GetType();

                //Assert
                Assert.That(containerStateType, Is.EqualTo(stateType));
            }
        }

        public class TransitionTests : StateContainerTest
        {
            public override void Init()
            {
                base.Init();
                _container = new StateContainer<TestState>();
            }

            [Test]
            [TestCase(0, TestName = "ZeroTransitionAdd")]
            [TestCase(1, TestName = "OneTransitionAdd")]
            [TestCase(2, TestName = "TwoTransitionAdd")]
            [TestCase(3, TestName = "ThreeTransitionAdd")]
            public void DoesAddTransitionCauseTheTransitionCountToIncreaseByOne(int amountOfTransitionsToAdd)
            {
                //Arrange

                //Act
                for(var i = 0; i < amountOfTransitionsToAdd; i++)
                    AddTransition(i);

                //Assert
                Assert.That(_container.TransitionCount, Is.EqualTo(amountOfTransitionsToAdd));
            }
        }
//
//                [Test]
//                public void DoesAddingAStateThatAlreadyExistsThrowADuplicateStateException()
//                {
//                    //Arrange
//                    _fsm.AddState<StateOne>();
//
//                    //Act
//
//                    //Assert
//                    Assert.Throws<DuplicateStateException>(_fsm.AddState<StateOne>);
//                }
//            }

        public class TestState : IState
        {
            public int ReturnZero()
            {
                return 0;
            }

            public void OnEntry()
            {
            }
        }

        public class TransitionOne : ITransition
        {
            
        }

        public class TransitionTwo : ITransition {

        }

        public class TransitionThree : ITransition {

        }
    }
}