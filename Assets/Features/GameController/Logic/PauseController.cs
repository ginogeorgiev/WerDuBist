using System;
using DataStructures.Event;
using Features.Input;
using UnityEngine;

namespace Features.GameController.Logic
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameEvent_SO pauseGame, unpauseGame;
        
        public void PauseGame()
        {
            Time.timeScale = 0f;
        }

        public void UnpauseGame()
        {
            Time.timeScale = 1f;
        }

        private void Awake()
        {
            playerControls = new PlayerControls();
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
