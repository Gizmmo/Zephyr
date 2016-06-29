using System;
using Zephyr.StateMachine.Core;

namespace Zephyr.StateMachine.Example
{
    public class FsmExample
    {
        private IFsm<ConcreteClass> _stateMachine;

        public FsmExample()
        {
            _stateMachine = new Fsm<ConcreteClass>();
        }


        public abstract class ConcreteClass : IState
        {
            protected StateData Data;

            protected ConcreteClass(StateData data)
            {
                Data = data;
            }

            public virtual void OnEntry()
            {
            }

            public virtual void OnExit()
            {
            }

            public void SetUpTransition(Action<Type> transitionMethod)
            {
            }

            public virtual void DoAction()
            {
            }

            public virtual void ReadyAction()
            {
            }
        }

        public class IdleState : ConcreteClass
        {
            public IdleState(StateData data) : base(data)
            {
            }

        }

        public class ActionState : ConcreteClass
        {
            public override void DoAction()
            {
                base.DoAction();
                Data.SomeArbitraryIntValue++;
            }

            public ActionState(StateData data) : base(data)
            {
            }
        }

        public class SimpleTransition : ITransition
        {
            public void Trigger()
            {
            }
        }

        public class StateData
        {
            public int SomeArbitraryIntValue;
        }
    }
}