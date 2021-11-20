
namespace DataStructures.StateMachineLogic
{
    public interface IStateManaged
    {
        void RequestState(IState requestedState);
    }
}
