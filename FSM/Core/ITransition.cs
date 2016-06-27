namespace Zephyr.StateMachine.Core
{
    public interface ITransition
    {
        /// <summary>
        /// Called when a Transition is triggered.
        /// </summary>
        void Trigger();
    }
}