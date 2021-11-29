using System;
using System.Collections.Generic;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.Input;
using Features.Player.Logic.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Player.Logic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float sprintSpeed, walkingSpeed;

        [SerializeField] private Animator animator;

        [SerializeField] private FloatVariable playerMovementSpeed;

        private new Rigidbody2D rigidbody2D;

        private PlayerControls playerControls;
        
        private InputAction movementInputAction, sprintInputAction;

        private StateMachine stateMachine;

        private IdleState idleState;
        private WalkingState walkingState;
        private SprintingState sprintingState;
        
        private void Awake()
        {
            playerControls = new PlayerControls();
            stateMachine = new StateMachine();
            rigidbody2D = this.GetComponent<Rigidbody2D>();
            
            movementInputAction = playerControls.Player.Movement;
            sprintInputAction = playerControls.Player.Sprint;

            idleState = new IdleState(animator, playerMovementSpeed, rigidbody2D);
            walkingState = new WalkingState(animator, playerMovementSpeed, movementInputAction, transform, rigidbody2D);
            sprintingState = new SprintingState(animator, playerMovementSpeed, movementInputAction, transform, rigidbody2D);
            
            stateMachine.Initialize(idleState);
        }
        
        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
        
        private void FixedUpdate()
        {
            HandleKeyboardInput();
            stateMachine.Update();
        }

        private void HandleKeyboardInput()
        {
            Vector2 movementInput = movementInputAction.ReadValue<Vector2>();
            float sprintInput = sprintInputAction.ReadValue<float>();

            if (movementInput.x != 0 || movementInput.y != 0)
            {
                if (stateMachine.CurrentState != walkingState && stateMachine.CurrentState != sprintingState)
                {
                    stateMachine.ChangeState(walkingState);
                }
                // Change the player's x-Scale to flip the animation
                // May be solved using the SpriteRenderer or Animator
                if(movementInput.x !=0) transform.localScale = new Vector3(Mathf.Sign(movementInput.x) * -1, 1, 1);

                // Check for sprinting (holding down LSHIFT)
                var movementSpeed = sprintInput > 0 ? sprintSpeed : walkingSpeed;
                if (playerMovementSpeed.Get() != movementSpeed)
                {
                    playerMovementSpeed.Set(movementSpeed);
                }
            }
            else
            {
                // Set the state to idle while not walking and only if not idle already
                if ( stateMachine.CurrentState != idleState)
                {
                    stateMachine.ChangeState(idleState);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Pick up algorithm (depends on the item)
            
            other.gameObject.SetActive(false);
        }


        // For the GameEvent when the playerMovementSpeed changes
        public void ChangePlayerMovementSpeed()
        {
            // Check the movement speed and adjust the state accordingly
            if (playerMovementSpeed.Get() == walkingSpeed)
            {
                stateMachine.ChangeState(walkingState);
            }
            
            if (playerMovementSpeed.Get() == sprintSpeed)
            {
                stateMachine.ChangeState(sprintingState);
            }
        }

        // Teleport function
        public void TeleportPlayer(Vector2 targetCoords)
        {
            transform.position = targetCoords;
        }
    }
}
