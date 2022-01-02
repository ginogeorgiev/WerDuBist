using DataStructures.StateMachineLogic;
using DataStructures.Variables;
using Features.Tutorial.Logic;
using UnityEngine;

namespace Features.Player.Logic.States
{
    public class IdleState : IState
    {
        private Animator playerAnimator;
        private FloatVariable movementSpeed;
        private Rigidbody2D playerRigidbody;

        private TutorialData_SO tutorialData;

        public IdleState(Animator animator, FloatVariable movementSpeed, Rigidbody2D playerRigidbody, TutorialData_SO tutorialData)
        {
            this.playerAnimator = animator;
            this.movementSpeed = movementSpeed;
            this.playerRigidbody = playerRigidbody;
            this.tutorialData = tutorialData;
        }
        
        //initialization to the beginning
        public void Enter()
        {
            playerAnimator.SetBool("isWalking", false);
            movementSpeed.Set(0f);
            playerRigidbody.velocity = Vector2.zero;
            tutorialData.OnDelayedActivateAllInfos.Raise();
        }

        // doing stuff continuously
        public void Execute()
        {
        }

        // what to do if machine kicks it out
        public void Exit()
        {
            tutorialData.OnDelayedDeActivateAllInfos.Raise();
        }
    }
}
