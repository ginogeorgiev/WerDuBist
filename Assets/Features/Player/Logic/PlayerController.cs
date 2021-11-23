using System;
using System.Collections.Generic;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.Input;
using Features.Player.Logic.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.Player.Logic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float sprintSpeed, walkingSpeed;

        [SerializeField] private Animator animator;

        [SerializeField] private FloatVariable playerMovementSpeed;

        private PlayerControls playerControls;
        
        private Vector2 movementInput;

        private float sprintInput;

        private StateMachine stateMachine;

        private IdleState idleState = new IdleState();
        private WalkingState walkingState = new WalkingState();
        private SprintingState sprintingState = new SprintingState();
        
        private void Awake()
        {
            playerControls = new PlayerControls();
            stateMachine = new StateMachine();
        }
        
        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        
        private void Update()
        {
            HandleKeyboardInput();
        }

        private void isWalking(){
            animator.SetBool("isWalking", true);
        }

        private void HandleKeyboardInput()
        {
            animator.SetBool("isWalking", false);
            movementInput = playerControls.Player.Movement.ReadValue<Vector2>();
            sprintInput = playerControls.Player.Sprint.ReadValue<float>();

            if (movementInput.x != 0 || movementInput.y != 0)
            {
                if (stateMachine.CurrentState != walkingState && stateMachine.CurrentState != sprintingState)
                {
                    stateMachine.ChangeState(walkingState);
                }
                // Change the player's x-Scale to flip the animation
                // May be solved using the SpriteRenderer or Animator
                if(movementInput.x !=0) transform.localScale = new Vector3(Mathf.Sign(movementInput.x) * -1, 1, 1);
                isWalking();

                var movementSpeed = sprintInput > 0 ? sprintSpeed : walkingSpeed;
                if (playerMovementSpeed.Get() != movementSpeed)
                {
                    playerMovementSpeed.Set(movementSpeed);
                }
                
                var xMovement = (transform.right * Time.deltaTime * movementInput.x * playerMovementSpeed.Get());
                var yMovement = (transform.up * Time.deltaTime * movementInput.y * playerMovementSpeed.Get());
                transform.position += xMovement + yMovement;
            }
            else
            {
                if ( stateMachine.CurrentState != idleState)
                {
                    stateMachine.ChangeState(idleState);
                    playerMovementSpeed.Set(0f);
                }
            }
        }

        // For the GameEvent when the playerMovementSpeed changes
        public void ChangePlayerMovementSpeed()
        {
            if (playerMovementSpeed.Get() == walkingSpeed)
            {
                stateMachine.ChangeState(walkingState);
            }
            
            if (playerMovementSpeed.Get() == sprintSpeed)
            {
                stateMachine.ChangeState(sprintingState);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Pick up stuff idk
        }
    }
}
