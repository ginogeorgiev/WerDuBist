
namespace DataStructures.StateMachineLogic
{
    /// <summary>
    /// A MonoBehaviour who uses a StateMachine decides whether a State
    /// is changed or not based on Events and communicates it to the StateMachine
    /// and if it does it provides a new State (IState Object).
    /// IState Objects have to be created on its own based on the available states and must implement IState!
    /// </summary>
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        public IState PreviousState { get; private set; }

        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            startingState.Enter();
        }

        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            PreviousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter();
        }

        public IState GetCurrentState()
        {
            return CurrentState;
        }

        public void Update()
        {
            CurrentState?.Execute();
        }

        public void SwitchToPreviousState()
        {
            CurrentState.Exit();
            CurrentState = PreviousState;
            CurrentState.Enter();
        }
    }
}