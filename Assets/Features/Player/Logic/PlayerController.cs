using System;
using System.Collections;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.Dialog.Logic;
using Features.GameLogic.Logic;
using Features.Input;
using Features.NPCs.Logic;
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

        [SerializeField] private PlayerInventory_SO playerInventory;

        [SerializeField] private FloatVariable playerMovementSpeed;
        
        [SerializeField] private BoolVariable isPlayerInConversation;

        [SerializeField] private TransitionData transitionData;

        [SerializeField] private GridElementEnteredEvent onGridElementEntered;

        private new Rigidbody2D rigidbody2D;

        private PlayerControls playerControls;
        
        private InputAction movementInputAction, sprintInputAction;

        private StateMachine stateMachine;

        private IdleState idleState;
        private WalkingState walkingState;
        private SprintingState sprintingState;
        private ConversationState conversationState;
        
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
            conversationState = new ConversationState(animator, playerMovementSpeed, rigidbody2D);
            
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
            // If the player is in a conversation, the keyboard inputs for walking should be ignored
            if(isPlayerInConversation.Get()) return;

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
                if (Math.Abs(playerMovementSpeed.Get() - movementSpeed) > 0.01f)
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
            if (other.CompareTag("Wood"))
            {
                other.gameObject.SetActive(false);
                playerInventory.Wood.Add(1);
            }
            if (other.CompareTag("Stone"))
            {
                other.gameObject.SetActive(false);
                playerInventory.Stone.Add(1);
            }
            if (other.CompareTag("Starfish"))
            {
                other.gameObject.SetActive(false);
                playerInventory.Starfish.Add(1);
            }
            
            if (other.CompareTag("GridElement"))
            {
                onGridElementEntered.Raise(other.GetComponent<GridElementBehavior>().GridIndex);
            }

            if (other.CompareTag("NPC"))
            {
                other.GetComponent<NpcBehaviour>().SetNpcFocus();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("NPC"))
            {
                other.GetComponent<NpcBehaviour>().RemoveNpcFocus();
            }
        }


        // For the GameEvent when the playerMovementSpeed changes
        public void ChangePlayerMovementSpeed()
        {
            // Check the movement speed and adjust the state accordingly
            if (Math.Abs(playerMovementSpeed.Get() - walkingSpeed) < 0.01f)
            {
                stateMachine.ChangeState(walkingState);
            }
            
            if (Math.Abs(playerMovementSpeed.Get() - sprintSpeed) < 0.01f)
            {
                stateMachine.ChangeState(sprintingState);
            }
        }

        public void ChangePlayerConversationState()
        {
            if (isPlayerInConversation.Get())
            {
                stateMachine.ChangeState(conversationState);
            }
            else
            {
                stateMachine.ChangeState(idleState);
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
            
            transform.position = teleportFocus.Get().position;
            yield return new WaitForSeconds(1f);
            
            transitionData.OnEnd.Raise();
        }
    }
}
