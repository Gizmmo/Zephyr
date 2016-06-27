using System;

namespace Zephyr.StateMachine.Core
{
    public interface ITransitionContainer
    {
        Type StateTo { get; }
        ITransition Transition { get; }

    }
}