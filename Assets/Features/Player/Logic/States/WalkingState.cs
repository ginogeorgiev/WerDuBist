using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player.Logic.States
{
    public class WalkingState : IState
    {
        private Animator playerAnimator;
        private FloatVariable movementSpeed;
        private InputAction movementInputAction;
        private Transform playerTransform;
        private Rigidbody2D playerRigidbody;

        public WalkingState(Animator animator, FloatVariable movementSpeed, InputAction movementInputAction,
            Transform playerTransform, Rigidbody2D playerRigidbody)
        {
            this.movementSpeed = movementSpeed;
            this.movementInputAction = movementInputAction;
            this.playerTransform = playerTransform;
            this.playerRigidbody = playerRigidbody;
            this.playerAnimator = animator;
        }
        
        //initialization to the beginning
        public void Enter()
        {
            playerAnimator.SetBool("isWalking", true);
        }

        // doing stuff continuously
        public void Execute()
        {
            Vector2 movementInput = movementInputAction.ReadValue<Vector2>();
            
            var xMovement = (playerTransform.right * movementInput.x * movementSpeed.Get());
            var yMovement = (playerTransform.up  * movementInput.y * movementSpeed.Get());
            playerRigidbody.velocity = xMovement + yMovement;
        }

        // what to do if machine kicks it out
        public void Exit()
        {
        }
    }
}
