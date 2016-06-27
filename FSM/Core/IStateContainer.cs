namespace Zephyr.StateMachine.Core
{
    public interface IStateContainer<T> where T : IState
    {
        T State { get; }

        TTransition GetTransition<TTransition>() where TTransition : ITransition;

        void AddTransition<TTransition, TStateTo>(TTransition transition) where TTransition : ITransition
            where TStateTo : IState;

        void RemoveTransition<TTransition>() where TTransition : ITransition;
    }
}