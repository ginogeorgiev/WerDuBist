using Features.Input;
using UnityEngine;

namespace Features.Player.Logic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;

        [SerializeField] private Animator animator;
        
        private PlayerControls playerControls;
        
        private Vector2 movementInput;
        
        private void Awake()
        {
            playerControls = new PlayerControls();
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

            if (movementInput.x != 0 || movementInput.y != 0)
            {
                // Change the player's x-Scale to flip the animation
                // May be solved using the SpriteRenderer or Animator
                if(movementInput.x !=0) transform.localScale = new Vector3(Mathf.Sign(movementInput.x) * -1, 1, 1);
                isWalking();
                var xMovement = (transform.right * Time.deltaTime * movementInput.x * movementSpeed);
                var yMovement = (transform.up * Time.deltaTime * movementInput.y * movementSpeed);
                transform.position += xMovement + yMovement;
            }
        }
    }
}
