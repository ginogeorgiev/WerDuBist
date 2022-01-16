using DataStructures.StateMachineLogic;
using Features.Input;
using UnityEngine;

namespace Features.GameController.Logic.States
{
    public class PauseState : IState
    {
        public void Enter()
        {
            InputController.Pause();
            Time.timeScale = 0f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
