using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 30f;
    private GameObject playerWhoShot;
    private ParticleSystem bulletParticleSystem;
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
        if (collision.gameObject == playerWhoShot)
        {
            return;
        }
        bulletParticleSystem.Play();
        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject, bulletParticleSystem.main.duration);
        
    }
}