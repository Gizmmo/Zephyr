using System;
using System.Collections.Generic;

namespace Zephyr.StateMachine.Core
{
    public class StateContainer<T> : IStateContainer<T> where T : IState
    {
        private Dictionary<Type, TransitionContainer<ITransition, IState>> _transitions =
            new Dictionary<Type, TransitionContainer<ITransition, IState>>();

        public StateContainer()
        {
            State = Activator.CreateInstance<T>();
        }

        public T State { get; private set; }

        public int TransitionCount
        {
            get { return _transitions.Count; }
        }


        public void AddTransition<TTransition, TStateTo>() where TTransition : class, ITransition, new()
            where TStateTo : class, IState, new()
        {
            var val = new TransitionContainer<TTransition, TStateTo>();

            _transitions.Add(typeof(TTransition), val);
        }

        public TTransition GetTransition<TTransition>() where TTransition : ITransition
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTransition<TTransition>() where TTransition : ITransition
        {
            throw new NotImplementedException();
        }
    }
}