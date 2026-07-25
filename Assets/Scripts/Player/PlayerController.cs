using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Tank Rotation Settings")]
    [SerializeField] private Transform treadsTransform;
    // Increased default rotation speed because it is now treated as degrees per second
    [SerializeField] private float rotationSpeed = 120f; 

    //[SerializeField] private TextMeshProUGUI label;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    //public void Jump(InputAction.CallbackContext context)
    //{
    //    if (context.performed && controller.isGrounded)
    //    {
    //        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    //    }
    //}

    //public void SetLabel(string label)
    //{
    //    this.label.text = label;
    //}

    void Update()
        //possibly add two variants of controls
    {
        if (treadsTransform != null)
        {
            float turn = moveInput.x * rotationSpeed * Time.deltaTime;
            treadsTransform.Rotate(Vector3.up, turn);

            Vector3 move = treadsTransform.forward * moveInput.y;
            controller.Move(move * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
