using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using UnityEngine;

namespace Features.Player.Logic.States
{
    public class ConversationState : IState
    {
        private Animator playerAnimator;
        private FloatVariable movementSpeed;
        private Rigidbody2D playerRigidbody;
        
        public ConversationState(Animator animator, FloatVariable movementSpeed, Rigidbody2D playerRigidbody)
        {
            this.playerAnimator = animator;
            this.movementSpeed = movementSpeed;
            this.playerRigidbody = playerRigidbody;
        }
        
        //initialization to the beginning
        public void Enter()
        {
            playerAnimator.SetBool("isWalking", false);
            movementSpeed.Set(0f);
            playerRigidbody.velocity = Vector2.zero;
        }

        // doing stuff continuously
        public void Execute()
        {
        }

        // what to do if machine kicks it out
        public void Exit()
        {
        }
    }
}
