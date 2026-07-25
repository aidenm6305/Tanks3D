using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    //[SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Tank Rotation Settings")]
    [SerializeField] private Transform treadsTransform;
    [SerializeField] private float rotationSpeed = 10f;

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
    {
        // 1. Calculate movement direction based on input
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        
        // 2. Move the CharacterController
        controller.Move(move * speed * Time.deltaTime);

        // 3. Rotate the treads to face the movement direction
        if (move != Vector3.zero && treadsTransform != null)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            treadsTransform.rotation = Quaternion.Slerp(treadsTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 4. Apply gravity
        velocity.y += gravity * Time.deltaTime;
        
        // Optional: Reset gravity buildup when grounded to avoid issues
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        controller.Move(velocity * Time.deltaTime);
    }
}
