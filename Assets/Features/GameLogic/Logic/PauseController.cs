using DataStructures.Event;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.GameLogic.Logic.States;
using Features.Input;
using UnityEngine;

namespace Features.GameLogic.Logic
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO pauseGame, unpauseGame;
        [SerializeField] private GameEvent_SO togglePause;
        [SerializeField] private BoolVariable isGamePaused;

        [SerializeField] private BoolVariable inGame;

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
            inGame.SetFalse();
            
            stateMachine = new StateMachine();
            pauseState = new PauseState(isGamePaused);
            unpauseState = new UnpauseState(isGamePaused);

            playerControls = new PlayerControls();
        }

        private void Start()
        {
            playerControls.Player.PauseOrResume.started += _ =>
            {
                togglePause.Raise();
            };
        }

        public void TogglePause()
        {
            if (inGame.Get()) ChangeGamePauseState();
        }

        private void ChangeGamePauseState()
        {
            if (unpauseGame==null || pauseGame==null) return;
            
            GameEvent_SO gameEventToRaise = Time.timeScale == 0f ? unpauseGame : pauseGame;
            gameEventToRaise.Raise();
        }

        public void EnablePauseUIActivation()
        {
            inGame.SetTrue();
        }
        
        public void DisablePauseUIActivation()
        {
            inGame.SetFalse();
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
