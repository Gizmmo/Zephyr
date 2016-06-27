using System;

namespace Zephyr.StateMachine.Core
{
    public interface IFsm<T> where T : IState
    {
        int StateCount { get; }

        Type InitalState { get; }

        T State { get; }

        void AddState(IState state);

        void RemoveState<TSub>() where TSub : T, new();

        void SetInitialState<TSub>() where TSub : T, new();

        void Start();

        void AddTransition<TStateFrom, TStateTo>(ITransition transition)
            where TStateFrom : T, new()
            where TStateTo : T, new();

        void TriggerTransition<TTransition>() where TTransition : ITransition;

        bool RemoveTransition<TTransition>() where TTransition : ITransition;
    }
}