using System.Collections;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.GameLogic.Logic;
using Features.Input;
using Features.Player.Logic.States;
using Features.WorldGrid.Logic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Player.Logic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float sprintSpeed, walkingSpeed;

        [SerializeField] private Animator animator;

        [SerializeField] private FloatVariable playerMovementSpeed;

        [SerializeField] private TransitionData transitionData;

        [SerializeField] private GridElementEnteredEvent onGridElementEntered;

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
            
            if (other.CompareTag($"Wood"))
            {
                other.gameObject.SetActive(false);
            }
            if (other.CompareTag($"Stone"))
            {
                other.gameObject.SetActive(false);
            }
            
            if (other.CompareTag($"GridElement"))
            {
                onGridElementEntered.Raise(int.Parse(other.name));
            }
            
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
        
        public void TeleportPlayer(PlayerTeleportFocus_SO teleportFocus)
        {
            StartCoroutine(TeleportPlayerSequence(teleportFocus));
        }

        private IEnumerator TeleportPlayerSequence(PlayerTeleportFocus_SO teleportFocus)
        {
            transitionData.OnStart.Raise();
            yield return new WaitForSeconds(transitionData.FadeInTime);
            
            transform.position = teleportFocus.focus.position;
            yield return new WaitForSeconds(1f);
            
            transitionData.OnEnd.Raise();
        }
    }
}
