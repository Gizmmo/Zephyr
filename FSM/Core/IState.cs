namespace Zephyr.StateMachine.Core
{
    public interface IState
    {
        void OnEntry();

        void OnExit();
    }
}