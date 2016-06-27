using System;

namespace Zephyr.StateMachine.Core
{
    public interface ITransitionContainer<TTransition, TStateTo> where TTransition : ITransition
        where TStateTo : IState
    {
        Type StateTo { get; }
        TTransition Transition { get; }

    }
}