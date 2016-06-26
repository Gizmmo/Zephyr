using System;

namespace Zephyr.StateMachine.Core
{
    public interface IFsm<T> where T : IState
    {
        int StateCount { get; }

        Type InitalState { get; }

        T CurrentState { get; }

        void AddState<TSub>() where TSub : T, new();

        void RemoveState<TSub>() where TSub : T, new();

        void SetInitialState<TSub>() where TSub : T, new();

        void Start();

        void AddTransition<TTransition, TStateFrom, TStateTo>() where TTransition : ITransition, new() where TStateFrom : T, new() where TStateTo : T, new();
    }
}