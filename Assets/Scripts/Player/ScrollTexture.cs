using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.5f, 0.0f);
    
    [Header("Tread Materials")]
    [SerializeField] private Material leftTreadMat;
    [SerializeField] private Material rightTreadMat;

    [SerializeField] private MeshRenderer leftTread;
    [SerializeField] private MeshRenderer rightTread;


    [SerializeField]
    private PlayerController player;

    private Vector2 currentLeftOffset = Vector2.zero;
    private Vector2 currentRightOffset = Vector2.zero;
    private Material clonedLeftTexture;
    private Material clonedRightTexture;

    private void Start()
    {
        clonedLeftTexture = new Material(leftTreadMat);
        clonedRightTexture = new Material(rightTreadMat);
        leftTread.material = clonedLeftTexture;
        rightTread.material = clonedRightTexture;
    }

    void Update()
    {
        if (player == null) return;

        if (player.MoveInput.sqrMagnitude > 0.001f)
        {
            float forwardInput = player.MoveInput.y;
            float turnInput = player.MoveInput.x;

            // Tank drive math:
            // Turning Right (turnInput > 0): Left tread moves forward (+), Right tread moves backward (-)
            // Turning Left (turnInput < 0): Right tread moves forward (+), Left tread moves backward (-)
            float leftMovement = forwardInput + turnInput;
            float rightMovement = forwardInput - turnInput;

            currentLeftOffset += scrollSpeed * leftMovement * Time.deltaTime;
            currentRightOffset += scrollSpeed * rightMovement * Time.deltaTime;

            if (clonedLeftTexture != null) clonedLeftTexture.mainTextureOffset = currentLeftOffset;
            if (clonedRightTexture != null) clonedRightTexture.mainTextureOffset = currentRightOffset;
        }
    }
}
