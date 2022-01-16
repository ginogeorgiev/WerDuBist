using System;
using DataStructures.Event;
using DataStructures.StateMachineLogic;
using Features.Dialog.Logic;
using Features.GameController.Logic.States;
using Features.Input;
using Features.Map.Logic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.GameController.Logic
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO pauseGame, unpauseGame;

        private StateMachine stateMachine;
        private PauseState pauseState;
        private UnpauseState unpauseState;

        public void PauseGame()
        {
            stateMachine.ChangeState(pauseState);
        }

        public void UnpauseGame()
        {
            stateMachine.ChangeState(unpauseState);
        }

        private void Awake()
        {
            stateMachine = new StateMachine();
            pauseState = new PauseState();
            unpauseState = new UnpauseState();

            playerControls = InputController.playerControls;
        }

        private void Start()
        {
            playerControls.Player.PauseOrResume.started += _ => ChangeGamePauseState();
        }

        private void ChangeGamePauseState()
        {
            if (unpauseGame==null || pauseGame==null) return;
            
            var gameEventToRaise = Time.timeScale == 0f ? unpauseGame : pauseGame;
            gameEventToRaise.Raise();
        }
        
        #region Input related
        
        private PlayerControls playerControls;

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        
        #endregion
    }
}
