using System;
using Zephyr.StateMachine.Core;

namespace Zephyr.StateMachine.Core
{
    public abstract class FsmState : IState
    {
        private Action transitionAction;

        public virtual void OnEntry()
        {
        }

        public virtual void OnExit()
        {
        }

        public void SetUpTransition(Action transitionMethod)
        {
            transitionAction = transitionMethod;
        }


        protected void TriggerTransition<T>() where T : ITransition, new()
        {
            transitionAction<T>();
        }
    }
}