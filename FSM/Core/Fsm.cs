using System;
using System.Collections.Generic;

namespace Zephyr.StateMachine.Core
{
    public class Fsm<T> where T : IState
    {
        public int StateCount { get { return _states.Count; } }
        public Type InitalState { get; private set; }
        public T CurrentState { get; private set; }

        private Dictionary<Type, T> _states = new Dictionary<Type, T>();

        /// <summary>
        /// Adds a State to the State Machine
        /// </summary>
        /// <typeparam name="TSub">The IState Class to add to the state machine</typeparam>
        public void AddState<TSub>() where TSub : T
        {
            _states.Add(typeof (TSub), Activator.CreateInstance<TSub>());
        }

        /// <summary>
        /// Removes a State from the state machine.
        /// </summary>
        /// <typeparam name="TSub">The IState class to remove from the state machine.</typeparam>
        public void RemoveState<TSub>() where TSub : T
        {
            var key = typeof (TSub);

            if (CurrentState != null && CurrentState.GetType() == key)
                throw new RemoveCurrentStateException();

            if (!_states.ContainsKey(key))
                throw new StateDoesNotExistException();

            _states.Remove(key);
        }

        /// <summary>
        /// Sets the inital state of the state machine for when start is called.
        /// </summary>
        /// <typeparam name="TSub">The Istate class to set as the inital state machine</typeparam>
        public void SetInitialState<TSub>() where TSub : T
        {
            var key = typeof (TSub);
            if (!_states.ContainsKey(key))
                throw new StateDoesNotExistException();

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
        }

        /// <summary>
        /// Sets the current state to the passed generic state.
        /// </summary>
        /// <typeparam name="TSub">The IState class to set as the current state</typeparam>
        internal void SetCurrentState<TSub>() where TSub : T
        {
            CurrentState = Activator.CreateInstance<TSub>();
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
    public class StateDoesNotExistException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state is being removed that is set as the current state.
    /// </summary>
    public class RemoveCurrentStateException : Exception
    {
    }
}