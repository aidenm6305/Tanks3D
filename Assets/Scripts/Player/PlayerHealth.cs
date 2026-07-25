using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    private float maxHealth = 100f;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., reload scene, show game over screen, etc.)
        Debug.Log("Player has died.");
    }
}
