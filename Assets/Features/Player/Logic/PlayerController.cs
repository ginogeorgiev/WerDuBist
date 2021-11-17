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
            
            if (movementInput.x < 0)
            {
                isWalking();
                transform.position += (transform.right * Time.deltaTime * -movementSpeed);
            }

            if (movementInput.x > 0)
            {
                isWalking();
                transform.position += (transform.right * Time.deltaTime * movementSpeed);
            }

            if (movementInput.y < 0)
            {
                isWalking();
                transform.position += (transform.up * Time.deltaTime * -movementSpeed);
            }

            if (movementInput.y > 0)
            {
                isWalking();
                transform.position += (transform.up * Time.deltaTime * movementSpeed);
            }
        }
    }
}
