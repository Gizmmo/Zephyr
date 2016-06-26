namespace Zephyr.StateMachine.Core
{
    public interface IStateContainer<T> where T : IState
    {
        T State { get; }

        TTransition GetTransition<TTransition>() where TTransition : ITransition;

        void AddTransition<TTransition, TStateTo>() where TTransition : class, ITransition, new()
            where TStateTo : class, IState, new();

        void RemoveTransition<TTransition>() where TTransition : ITransition;
    }
}