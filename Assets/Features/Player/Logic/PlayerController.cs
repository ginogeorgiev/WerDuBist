using System;
using System.Collections;
using DataStructures.Event;
using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.Dialog.Logic;
using Features.GameLogic.Logic;
using Features.Input;
using Features.NPCs.Logic;
using Features.Player.Logic.States;
using Features.Tutorial.Logic;
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

        [SerializeField] private TutorialData_SO tutorialData;

        [SerializeField] private GridElementEnteredEvent onGridElementEntered;
        
        [SerializeField] private BoolVariable isGamePaused;

        [SerializeField] private AudioSource footstepSound, collectSound;

        private bool isPlayerTeleporting=false;

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

            idleState = new IdleState(animator, playerMovementSpeed, rigidbody2D, tutorialData);
            walkingState = new WalkingState(animator, playerMovementSpeed, movementInputAction, transform, rigidbody2D, footstepSound);
            sprintingState = new SprintingState(animator, playerMovementSpeed, movementInputAction, transform, rigidbody2D, footstepSound);
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
            if(isGamePaused.Get()) return;
            
            // If the player is in a conversation, the keyboard inputs for walking should be ignored
            if(isPlayerInConversation.Get()) return;

            Vector2 movementInput = movementInputAction.ReadValue<Vector2>();
            float sprintInput = sprintInputAction.ReadValue<float>();

            if ((movementInput.x != 0 || movementInput.y != 0) && !isPlayerTeleporting)
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
                collectSound.Play();
            }
            if (other.CompareTag("Stone"))
            {
                other.gameObject.SetActive(false);
                playerInventory.Stone.Add(1);
            }
            if (other.CompareTag("AppleRed"))
            {
                other.gameObject.SetActive(false);
                playerInventory.AppleRed.Add(1);
                collectSound.Play();
            }
            if (other.CompareTag("Tube"))
            {
                other.gameObject.SetActive(false);
                playerInventory.Tube.Add(1);
                collectSound.Play();
            }
            if (other.CompareTag("MetalPlate1"))
            {
                other.gameObject.SetActive(false);
                playerInventory.MetalPlate1.Add(1);
            }
            if (other.CompareTag("MetalPlate2"))
            {
                other.gameObject.SetActive(false);
                playerInventory.MetalPlate2.Add(1);
            }
            
            if (other.CompareTag("GridElement"))
            {
                onGridElementEntered.Raise(other.GetComponent<GridElementBehavior>().GridIndex);
            }

            if (other.CompareTag("NPC"))
            {
                other.GetComponent<NpcBehaviour>().SetNpcFocus();
                tutorialData.OnActivateInteractInfo.Raise();
            }

            if (other.CompareTag("DialogTrigger"))
            {
                other.GetComponentInParent<NpcBehaviour>().SetNpcFocus();
                DialogTrigger dialogTrigger = other.GetComponent<DialogTrigger>();
                if (dialogTrigger.TeleportPlayerForTrigger)
                {
                    dialogTrigger.SetTeleportFocus();
                    StartCoroutine(TeleportPlayerSequence(dialogTrigger.TeleportFocus, dialogTrigger));
                }
                else
                {
                    dialogTrigger.StartConversation();
                }
                tutorialData.OnDeActivateInteractInfo.Raise();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("NPC"))
            {
                other.GetComponent<NpcBehaviour>().RemoveNpcFocus();
                tutorialData.OnDeActivateInteractInfo.Raise();
            }
            
            if (other.CompareTag("DialogTrigger"))
            {
                tutorialData.OnDeActivateInteractInfo.Raise();

                if (other.GetComponent<DialogTrigger>().TeleportPlayerForTrigger) return;
                other.GetComponentInParent<NpcBehaviour>().RemoveNpcFocus();
                other.gameObject.SetActive(false);
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

        private IEnumerator TeleportPlayerSequence(PlayerTeleportFocus_SO teleportFocus, DialogTrigger dialogTrigger=null)
        {
            isPlayerTeleporting = true;
            transitionData.OnStart.Raise();
            
            yield return new WaitForSeconds(transitionData.FadeInTime);
            transform.position = teleportFocus.Get().position;
            if (dialogTrigger != null)
            {
                dialogTrigger.StartConversation();
                dialogTrigger.OnConversationOver();
            }
            
            yield return new WaitForSeconds(1f);
            isPlayerTeleporting = false;
            transitionData.OnEnd.Raise();
        }
    }
}
