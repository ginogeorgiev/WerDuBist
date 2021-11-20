namespace DataStructures.StateMachineLogic
{
    // 
    public interface IState
    {
        //initialization to the beginning
        void Enter();

        // doing stuff continuously
        void Execute();

        // what to do if machine kicks it out
        void Exit();

    }
}