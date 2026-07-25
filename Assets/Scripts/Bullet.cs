using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 30f;
    private GameObject playerWhoShot;
    private ParticleSystem bulletParticleSystem;
    private bool hasDoneDamage = false;
    private void Start()
    {
        bulletParticleSystem = GetComponent<ParticleSystem>();
    }
    public void SetPlayerWhoShot(GameObject player)
    {
        playerWhoShot = player;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasDoneDamage)
            return;
        if (collision.gameObject == playerWhoShot || collision.transform.IsChildOf(playerWhoShot.transform))
            return;

        hasDoneDamage = true;
        bulletParticleSystem.Play();
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
            }
        }
        if (collision.gameObject.CompareTag("BreakableWall"))
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
        Destroy(gameObject, bulletParticleSystem.main.duration);
    }
}