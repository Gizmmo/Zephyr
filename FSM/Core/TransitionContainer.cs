﻿using System;

namespace Zephyr.StateMachine.Core
{
    public class TransitionContainer<TTransition, TStateTo> :
        ITransitionContainer<TTransition, TStateTo>
        where TTransition : ITransition
        where TStateTo : IState
    {
        public TransitionContainer(TTransition transition)
        {
            StateTo = typeof(TStateTo);
            Transition = transition;
        }

        public Type StateTo { get; private set; }

        public TTransition Transition { get; private set; }
    }
}