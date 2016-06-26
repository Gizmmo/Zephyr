using NUnit.Framework;
using Zephyr.StateMachine.Core;

namespace Zephyr.StateMachine.Test.Editor.Core
{
    [TestFixture]
    [Category("FsmTransitionContainer")]
    public class TransitionContainerTest
    {
        private TransitionContainer<TransitionOne, StateTwo> _container;

        [SetUp]
        public void Init()
        {
            _container = new TransitionContainer<TransitionOne, StateTwo>();
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheTransitonPropertyToAnIntializedValue()
        {
            //Arrange

            //Act

            //Assert
            Assert.That(_container.Transition, Is.Not.Null);
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheTransitonPropertyToTheCorrectType()
        {
            //Arrange
            var containerTransitionType = _container.Transition.GetType();
            var transitionPassedType = typeof(TransitionOne);

            //Act

            //Assert
            Assert.That(containerTransitionType, Is.EqualTo(transitionPassedType));
        }

        [Test]
        public void DoesInitalizingTheTransitionContiainerSetTheStateToPropertyToTheCorrectType()
        {
            //Arrange
            var stateToPassedType = typeof(StateTwo);

            //Act

            //Assert
            Assert.That(_container.StateTo, Is.EqualTo(stateToPassedType));
        }
    }

    public class StateOne : IState
    {
        public void OnEntry()
        {
        }
    }

    public class StateTwo : IState
    {
        public void OnEntry()
        {
        }
    }

    public class TransitionOne : ITransition
    {
    }

    public class TransitionTwo : ITransition
    {
    }

    public class TransitionThree : ITransition
    {
    }
}