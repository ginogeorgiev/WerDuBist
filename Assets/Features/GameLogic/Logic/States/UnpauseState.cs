using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.GameLogic.Logic.States
{
    public class UnpauseState : IState
    {
        private BoolVariable blockInputOnPause;
        
        public UnpauseState(BoolVariable blockInputOnPause)
        {
            this.blockInputOnPause = blockInputOnPause;
        }
        
        public void Enter()
        {
            blockInputOnPause.Set(false);
            Time.timeScale = 1f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
