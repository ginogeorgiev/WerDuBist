using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player.Logic.States
{
    public class SprintingState : IState
    {
        private Animator playerAnimator;
        private FloatVariable movementSpeed;
        private InputAction movementInputAction;
        private Transform playerTransform;
        private Rigidbody2D playerRigidbody;
        private AudioSource audioSource;

        public SprintingState(Animator animator, FloatVariable movementSpeed, InputAction movementInputAction,
            Transform playerTransform, Rigidbody2D playerRigidbody, AudioSource audioSource)
        {
            this.movementSpeed = movementSpeed;
            this.movementInputAction = movementInputAction;
            this.playerTransform = playerTransform;
            this.playerRigidbody = playerRigidbody;
            this.playerAnimator = animator;
            this.audioSource = audioSource;
        }
        
        //initialization to the beginning
        public void Enter()
        {
            playerAnimator.SetBool("isWalking", true);
            audioSource.Play();
            audioSource.pitch = 1.5f;
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
            audioSource.pitch = 1f;
            audioSource.Stop();
        }
    }
}