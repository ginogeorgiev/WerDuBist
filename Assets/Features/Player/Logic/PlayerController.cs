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

        [SerializeField] private bool rigidbodyTestBool = false;

        private new Rigidbody2D rigidbody2D;

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
            rigidbody2D = this.GetComponent<Rigidbody2D>();
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

                // Check for sprinting (holding down LSHIFT)
                var movementSpeed = sprintInput > 0 ? sprintSpeed : walkingSpeed;
                if (playerMovementSpeed.Get() != movementSpeed)
                {
                    playerMovementSpeed.Set(movementSpeed);
                }

                if (rigidbodyTestBool)
                {
                    var xMovement = (transform.right * movementInput.x * playerMovementSpeed.Get());
                    var yMovement = (transform.up  * movementInput.y * playerMovementSpeed.Get());
                    // Temporary solution, objects with rigidbodies should use it's rigidbody
                    rigidbody2D.velocity = xMovement + yMovement;
                }
                else
                {
                    var xMovement = (transform.right * Time.deltaTime * movementInput.x * playerMovementSpeed.Get());
                    var yMovement = (transform.up * Time.deltaTime * movementInput.y * playerMovementSpeed.Get());
                    // Temporary solution, objects with rigidbodies should use it's rigidbody
                    transform.position += xMovement + yMovement;
                }
                
            }
            else
            {
                // Set the state to idle while not walking and only if not idle already
                if ( stateMachine.CurrentState != idleState)
                {
                    stateMachine.ChangeState(idleState);
                    playerMovementSpeed.Set(0f);
                }

                if (rigidbodyTestBool)
                {
                    rigidbody2D.velocity = Vector2.zero;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Pick up algorithm (depends on the item)
            Debug.Log(other);
            
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
        public void TeleportPlayer(Vector2 targetCoords, string targetSceneName=null)
        {
            if (targetSceneName != null)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(targetSceneName));
            }

            transform.position = targetCoords;
        }
    }
}
