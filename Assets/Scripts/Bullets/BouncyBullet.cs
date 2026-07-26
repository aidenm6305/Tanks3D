using UnityEngine;


public class BouncyBullet : Bullet
{

    [SerializeField]
    private int bounceCount = 3; 
    private void OnCollisionEnter(Collision collision)
    {
        if (playerWhoShot != null)
        {
            if (collision.gameObject == playerWhoShot || collision.transform.IsChildOf(playerWhoShot.transform))
                return;
        }

        transform.forward = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
        HandleDamage(collision);

        bounceCount--;
        
        if (bounceCount <= 0)
        {
            DestroyBullet();
            return;
        }
        hasDoneDamage = false;
        playerWhoShot = null;
    }
}