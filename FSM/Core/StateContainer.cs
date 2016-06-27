using System;
using System.Collections.Generic;

namespace Zephyr.StateMachine.Core
{
    public class StateContainer : IStateContainer
    {
        private Dictionary<Type, ITransitionContainer> _transitions = new Dictionary<Type, ITransitionContainer>();

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="state">The state this container will store</param>
        public StateContainer(IState state)
        {
            State = state;
        }


        /// <summary>
        /// The state that this container holds.
        /// </summary>
        public IState State { get; private set; }

        /// <summary>
        /// The amount of transitions stored in transition Dictionary.
        /// </summary>
        public int TransitionCount
        {
            get { return _transitions.Count; }
        }

        /// <summary>
        /// Adds a transition to the transition dictionary, which will be stored in a transition container, with also the state that
        /// it will go to upon completion.
        /// </summary>
        /// <param name="transition">The transition to store in the transition Dictionary</param>
        /// <param name="stateTo">The state the fsm will change to upon completion of the transition.</param>
        public void AddTransition(ITransition transition, Type stateTo)
        {
            var key = transition.GetType();

            if (_transitions.ContainsKey(key))
                throw new DuplicateStateTransitionException();

            _transitions.Add(transition.GetType(), new TransitionContainer(transition, stateTo));
        }

        /// <summary>
        /// Gets the transition from the transition container of the transtions dictionary
        /// </summary>
        /// <typeparam name="T">The trpe of transition to get</typeparam>
        /// <returns>The transition of the type store</returns>
        public ITransitionContainer GetTransition<T>() where T : ITransition
        {
            var key = typeof(T);
            ITransitionContainer foundTransition;

            if (!_transitions.TryGetValue(key, out foundTransition))
                throw new TransitionNotFoundException();

            return foundTransition;
        }

        /// <summary>
        /// Removes a transition for the transitions array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>true if the transition was removed, otherwise false.</returns>
        public bool RemoveTransition<T>() where T : ITransition
        {
            return _transitions.Remove(typeof(T));
        }

        /// <summary>
        /// Triggers the Transition searched for, and then return the state the fsm should go to.
        /// </summary>
        /// <typeparam name="T">The transition to trigger.</typeparam>
        /// <returns>The state the fsm should switch to in System.Type</returns>
        public Type TriggerTransition<T>() where T : ITransition
        {
            var container = GetTransition<T>();
            container.Transition.Trigger();
            return container.StateTo;
        }
    }
}