using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    protected Sprite icon;

    [SerializeField]
    public Color bulletColor = Color.white;

    [SerializeField] 
    protected Transform visualTransform; 
    protected string bulletName = "Normal Bullet";
    public float speed = 30f;
    protected GameObject playerWhoShot;
    private ParticleSystem bulletParticleSystem;
    protected bool hasDoneDamage = false;
    private bool isBeingDestroyed = false;
    private Rigidbody rb;
    private void Start()
    {
        bulletParticleSystem = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        var meshRender = GetComponentInChildren<MeshRenderer>();
        var mesh = meshRender.materials[0];
        Material clonedTexture = Instantiate(mesh);
        clonedTexture.color = bulletColor;
        meshRender.material = clonedTexture;

    }
    public void SetPlayerWhoShot(GameObject player)
    {
        playerWhoShot = player;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        visualTransform.rotation = Quaternion.LookRotation(rb.linearVelocity.normalized, transform.forward);
        Debug.DrawRay(visualTransform.position, visualTransform.forward * 2f, Color.red);
        if(transform.position.y < -10f)
        {
            DestroyBullet();
        }
    }

    protected void HandleDamage(Collision collision)
    {
        if (isBeingDestroyed) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
                DestroyBullet();
            }
        }
        else if (collision.gameObject.CompareTag("BreakableWall"))
        {
            Debug.Log($"Bullet collided with breakable wall at {collision.transform.position}");
            BreakableWall breakableWall = collision.gameObject.GetComponent<BreakableWall>();
            if (breakableWall != null)
            {
                breakableWall.BreakWall();
            }
            else
            {
                Debug.LogWarning("BreakableWall component not found on the collided object.");
            }
        }
    }

    protected void DestroyBullet()
    {
        if (isBeingDestroyed) return;

        bulletParticleSystem.Play();
        Destroy(gameObject, bulletParticleSystem.main.duration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hasDoneDamage)
            return;
        if (collision.gameObject == playerWhoShot || collision.transform.IsChildOf(playerWhoShot.transform))
            return;

        hasDoneDamage = true;
        HandleDamage(collision);
        DestroyBullet();

    }
}