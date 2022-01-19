using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.GameLogic.Logic.States
{
    public class UnpauseState : IState
    {
        private BoolVariable isGamePaused;
        
        public UnpauseState(BoolVariable isGamePaused)
        {
            this.isGamePaused = isGamePaused;
        }
        
        public void Enter()
        {
            isGamePaused.Set(false);
            Time.timeScale = 1f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
