using System;
using System.Collections.Generic;

namespace Zephyr.StateMachine.Core
{
    public class Fsm<T> : IFsm<T> where T : IState
    {
        public int StateCount
        {
            get { return _states.Count; }
        }

        public Type InitalState { get; private set; }
        public T CurrentState { get; private set; }

        private Dictionary<Type, StateContainer<T>> _states = new Dictionary<Type, StateContainer<T>>();

        /// <summary>
        /// Adds a State to the State Machine 
        /// </summary>
        /// <typeparam name="TSub">The IState Class to add to the state machine</typeparam>
        public void AddState<TSub>() where TSub : T, new() {
            var key = typeof(TSub);

            if (_states.ContainsKey(key))
                throw new DuplicateStateException();

            _states.Add(typeof(TSub), Activator.CreateInstance<TSub>());
        }

        /// <summary>
        /// Removes a State from the state machine.
        /// </summary>
        /// <typeparam name="TSub">The IState class to remove from the state machine.</typeparam>
        public void RemoveState<TSub>() where TSub : T, new() {
            var key = typeof(TSub);

            if (CurrentState != null && CurrentState.GetType() == key)
                throw new RemoveCurrentStateException();

            if (!_states.ContainsKey(key))
                throw new StateNotFoundException();

            _states.Remove(key);
        }

        /// <summary>
        /// Sets the inital state of the state machine for when start is called.
        /// </summary>
        /// <typeparam name="TSub">The Istate class to set as the inital state machine</typeparam>
        public void SetInitialState<TSub>() where TSub : T, new() {
            var key = typeof(TSub);
            if (!_states.ContainsKey(key))
                throw new StateNotFoundException();

            InitalState = key;
        }

        /// <summary>
        /// Starts the state machine on the inital state.
        /// </summary>
        public void Start()
        {
            if (InitalState == null)
                throw new InitalStateNullException();

            CurrentState = _states[InitalState];
            CurrentState.OnEntry();
        }

        public void AddTransition<TTransition, TConcreteFrom, TConcreteTo>() where TTransition : ITransition, new() where TConcreteFrom : T, new() where TConcreteTo : T, new() {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the current state to the passed generic state.
        /// </summary>
        /// <typeparam name="TSub">The IState class to set as the current state</typeparam>
        internal void SetCurrentState<TSub>() where TSub : T
        {
            T foundState;
            if (!_states.TryGetValue(typeof(TSub), out foundState))
                throw new StateNotFoundException();

            CurrentState = foundState;
        }
    }

    /// <summary>
    /// Thrown when an inital state is not set for start up.
    /// </summary>
    public class InitalStateNullException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state does not exist is trying to be accessed.
    /// </summary>
    public class StateNotFoundException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state is being removed that is set as the current state.
    /// </summary>
    public class RemoveCurrentStateException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateException : Exception
    {
    }
}