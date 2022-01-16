using System;
using UnityEngine;

namespace Features.Input
{
    public class InputController : MonoBehaviour
    {
        public static PlayerControls playerControls;

        private void Awake()
        {
            playerControls = new PlayerControls();
        }

        public static void Unpause()
        {
            playerControls.Player.Enable();
        }
        
        public static void Pause()
        {
            var inputActions = playerControls.Player.Get();
            foreach (var inputAction in inputActions)
            {
                if (inputActions["PauseOrResume"] != inputAction)
                {
                    inputAction.Disable();
                }
            }
        }
    }
}
