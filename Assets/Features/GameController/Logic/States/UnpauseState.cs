using DataStructures.StateMachineLogic;
using Features.Input;
using UnityEngine;

namespace Features.GameController.Logic.States
{
    public class UnpauseState : IState
    {
        public void Enter()
        {
            InputController.Unpause();
            Time.timeScale = 1f;
        }

        public void Execute() { }

        public void Exit() { }
    }
}
