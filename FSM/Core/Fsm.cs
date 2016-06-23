using System;

namespace Zephyr.StateMachine.Core
{
    public class Fsm
    {
        public int StateCount { get; private set; }
        public object InitalState { get; private set; }
        public IState CurrentState { get; private set; }

        private IState _storedState;

        public void AddState(IState idleState)
        {
            _storedState = idleState;
            StateCount++;
        }

        public void AddState<T>() where T : IState
        {
            _storedState = Activator.CreateInstance<T>();  //This simply creates a new object of type T, looks more complicated then it is
            StateCount++;
        }

        public void RemoveState<T>()
        {
            StateCount--;
        }

        public void RemoveState(Type type) {
            StateCount--;
        }

        public void SetInitialState<T>()
        {
            InitalState = typeof (T);
        }

        public void Start()
        {
            if (InitalState == null)
                throw new InitalStateNullException();

            CurrentState = _storedState;
        }
    }

    public class InitalStateNullException : Exception
    {
    }
}