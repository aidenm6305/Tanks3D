using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f;
    
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float gravity = -9.8f;

    [Header("Tank Rotation Settings")]
    [SerializeField] private Transform treadsTransform;
    [SerializeField] private float rotationSpeed = 120f; 

    [Header("Boost Settings")]
    [SerializeField] private float maxBoostAmount = 100f;
    [SerializeField] private float boostDepletionRate = 30f;
    private float currentBoostAmount;
    private bool isTryingToBoost = false;       

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity;

    [SerializeField]
    private AudioSource moveSource;

    [SerializeField]
    private Slider boostSlider;

    private float speed;
    private PlayerHealth playerHealth;
    
    void Awake()
    {
        playerHealth = GetComponentInParent<PlayerHealth>();
        speed = normalSpeed;
        currentBoostAmount = maxBoostAmount;
        controller = GetComponent<CharacterController>();
        
        if (boostSlider != null)
        {
            boostSlider.value = currentBoostAmount / maxBoostAmount;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    public void Boost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isTryingToBoost = true;
        }
        else if (context.canceled)
        {
            isTryingToBoost = false;
        }
    }

    private void HandleBoost()
    {
        if (isTryingToBoost && currentBoostAmount > 0)
        {
            speed = boostSpeed;
            currentBoostAmount -= boostDepletionRate * Time.deltaTime;
            
            if (currentBoostAmount <= 0)
            {
                currentBoostAmount = 0;
                speed = normalSpeed;
            }
        }
        else
        {
            speed = normalSpeed;
            if (currentBoostAmount < maxBoostAmount)
            {
                if (currentBoostAmount > maxBoostAmount)
                {
                    currentBoostAmount = maxBoostAmount;
                }
            }
        }

        if (boostSlider != null)
        {
            boostSlider.value = currentBoostAmount / maxBoostAmount;
        }
    }

    void Update()
    {
        if (playerHealth != null && playerHealth.isDead)
        {
            AudioManager.Instance.StopMove(moveSource);
            return;
        }

        HandleBoost();

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

        if (moveInput.magnitude > 0)
        {
            AudioManager.Instance.PlayMove(moveSource);
        }
        else
        {
            AudioManager.Instance.StopMove(moveSource);
        }
    }
}
