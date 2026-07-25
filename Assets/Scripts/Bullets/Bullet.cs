using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField]
    protected Sprite icon;

    [SerializeField]
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
    }
    public void SetPlayerWhoShot(GameObject player)
    {
        playerWhoShot = player;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected void HandleDamage(Collision collision)
    {
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