using System;

namespace Zephyr.StateMachine.Core
{
    public class TransitionContainer : ITransitionContainer
    {
        public TransitionContainer(ITransition transition, Type stateTo)
        {
            StateTo = stateTo;
            Transition = transition;
        }

        public Type StateTo { get; private set; }

        public ITransition Transition { get; private set; }
    }
}