using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.GameLogic.Logic.States
{
    public class PauseState : IState
    {
        private BoolVariable isGamePaused;
        
        public PauseState(BoolVariable isGamePaused)
        {
            this.isGamePaused = isGamePaused;
        }
        
        public void Enter()
        {
            isGamePaused.Set(true);
            Time.timeScale = 0f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
