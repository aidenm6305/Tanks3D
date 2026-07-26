using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float normalSpeed = 5f;
    
    [SerializeField] private float boostSpeed = 10f;

    [Header("Tank Rotation Settings")]
    [SerializeField] private Transform treadsTransform;
    [SerializeField] private float rotationSpeed = 120f; 

    [Header("Boost Settings")]
    [SerializeField] private float maxBoostAmount = 100f;
    [SerializeField] private float boostDepletionRate = 30f;
    private float currentBoostAmount;
    private bool isTryingToBoost = false;       

    private Rigidbody rb;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput;

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
        rb = GetComponent<Rigidbody>();
        
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
        }
    
        if (moveInput.magnitude > 0)
        {
            AudioManager.Instance.PlayMove(moveSource);
        }
        else
        {
            AudioManager.Instance.StopMove(moveSource);
        }
    }

    void FixedUpdate() 
    {
        if (playerHealth != null && playerHealth.isDead) return;

        if (treadsTransform != null && rb != null)
        {
            Vector3 targetVelocity = treadsTransform.forward * moveInput.y * speed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
        }
    }
}
