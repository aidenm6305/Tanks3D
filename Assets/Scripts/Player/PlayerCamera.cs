using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private GameObject cannon;
    [SerializeField] private GameObject turret;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [Header("Camera Settings")]
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float verticalRotationSpeed = 50f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 30f;

    private float horizontalAngle = 0f;
    private float verticalAngle = 0f;
    private Transform cannonTransform;
    private Transform turretTransform;

    private Vector2 lookInput;


    void Start()
    {
        if (cannon == null)
            Debug.LogWarning("No Cannon attached");

        if (cannon != null)
            cannonTransform = cannon.transform;

        if (turret == null)
            Debug.LogWarning("No Turret attached");

        if (turret != null)
            turretTransform = turret.transform;

        if (cinemachineCamera == null)
            cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    void LateUpdate()
    {
        HandleCameraInput(lookInput);
        RotateCannon();
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void HandleCameraInput(Vector2 input)
    {
        float lookX = input.x;
        float lookY = input.y;

        horizontalAngle += lookX * rotationSpeed * Time.deltaTime;
        verticalAngle -= lookY * verticalRotationSpeed * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);
    }

    void RotateCannon()
    {
        if (cannonTransform == null)
            return;

        cannonTransform.rotation = Quaternion.Euler(0, horizontalAngle, 0);


        if (turretTransform == null)
            return;
        
        turretTransform.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
    }
}