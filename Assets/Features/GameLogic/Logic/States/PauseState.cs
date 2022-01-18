using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.GameLogic.Logic.States
{
    public class PauseState : IState
    {
        private BoolVariable blockInputOnPause;
        
        public PauseState(BoolVariable blockInputOnPause)
        {
            this.blockInputOnPause = blockInputOnPause;
        }
        
        public void Enter()
        {
            blockInputOnPause.Set(true);
            Time.timeScale = 0f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
