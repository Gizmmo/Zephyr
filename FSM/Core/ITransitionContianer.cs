using System;

namespace Zephyr.StateMachine.Core
{
    public interface ITransitionContainer<out TTransition, out TStateTo> where TTransition : ITransition
        where TStateTo : IState
    {
        Type StateTo { get; }
        TTransition Transition { get; }

    }
}