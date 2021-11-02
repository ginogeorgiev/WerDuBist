using Features.Input;
using UnityEngine;

namespace Features.Player.Logic
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed;
        
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

        private void HandleKeyboardInput()
        {
            movementInput = playerControls.Player.Movement.ReadValue<Vector2>();
            
            if (movementInput.x < 0)
            {
                transform.position += (transform.right * Time.deltaTime * -movementSpeed);
            }

            if (movementInput.x > 0)
            {
                transform.position += (transform.right * Time.deltaTime * movementSpeed);
            }

            if (movementInput.y < 0)
            {
                transform.position += (transform.up * Time.deltaTime * -movementSpeed);
            }

            if (movementInput.y > 0)
            {
                transform.position += (transform.up * Time.deltaTime * movementSpeed);
            }
        }
    }
}
