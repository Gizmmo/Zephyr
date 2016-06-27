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
        public T State { get; private set; }
        private StateContainer _currentStateContainer;

        private Dictionary<Type, StateContainer> _states = new Dictionary<Type, StateContainer>();

        /// <summary>
        /// Adds a State to the State Machine 
        /// </summary>
        /// <typeparam name="TSub">The IState Class to add to the state machine</typeparam>
        public void AddState(IState state)
        {
            var key = state.GetType();

            if (_states.ContainsKey(key))
                throw new DuplicateStateException();

            _states.Add(key, new StateContainer(state));
        }

        /// <summary>
        /// Removes a State from the state machine.
        /// </summary>
        /// <typeparam name="TSub">The IState class to remove from the state machine.</typeparam>
        public void RemoveState<TSub>() where TSub : T, new()
        {
            var key = typeof(TSub);

            if (State != null && State.GetType() == key)
                throw new RemoveCurrentStateException();

            if (!_states.ContainsKey(key))
                throw new StateNotFoundException();

            _states.Remove(key);
        }

        /// <summary>
        /// Sets the inital state of the state machine for when start is called.
        /// </summary>
        /// <typeparam name="TSub">The Istate class to set as the inital state machine</typeparam>
        public void SetInitialState<TSub>() where TSub : T, new()
        {
            var key = typeof(TSub);

            if (!IsStateFound(key))
                StateNotFound();

            InitalState = key;
        }

        /// <summary>
        /// Starts the state machine on the inital state.
        /// </summary>
        public void Start()
        {
            if (InitalState == null)
                throw new InitalStateNullException();
            _currentStateContainer = _states[InitalState];
            State = (T) _currentStateContainer.State;
            State.OnEntry();
        }

        public void AddTransition<TConcreteFrom, TConcreteTo>(ITransition transition)
            where TConcreteFrom : T, new()
            where TConcreteTo : T, new()
        {
            var foundStateFromContainer = GetStateContiainer(typeof(TConcreteFrom));

            var key = typeof(TConcreteTo);
            if (!IsStateFound(key))
                StateNotFound();

            foundStateFromContainer.AddTransition(transition, key);
        }

        public void TriggerTransition<TTransition>() where TTransition : ITransition
        {
            var stateTo = _currentStateContainer.TriggerTransition<TTransition>();
            SetCurrentState(stateTo);
        }

        public bool RemoveTransition<TTransition>() where TTransition : ITransition
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the current state to the passed state Type.
        /// </summary>
        internal void SetCurrentState(Type state)
        {
            var foundStateContainer = GetStateContiainer(state);

            _currentStateContainer = foundStateContainer;
            State = (T) _currentStateContainer.State;
        }

        internal StateContainer GetStateContiainer(Type state)
        {
            StateContainer foundState;
            if (!_states.TryGetValue(state, out foundState))
                StateNotFound();

            return foundState;
        }

        internal bool IsStateFound(Type key)
        {
            return _states.ContainsKey(key);
        }

        internal void StateNotFound()
        {
            throw new StateNotFoundException();
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

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class DuplicateStateTransitionException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class TransitionNotFoundException : Exception
    {
    }

    /// <summary>
    /// Thrown when a state of the type has already been added to the state machine
    /// </summary>
    public class StateMachineNotStartedException : Exception
    {
    }
}