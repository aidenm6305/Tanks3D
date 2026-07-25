using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    private float maxHealth = 100f;
    public Slider healthSlider;
    public bool isDead = false;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject winScreen;

    private PlayerController playerController;
    private PlayerShoot playerShoot;
    private GameManager gameManager;

    private void Start()
    {
        deathScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        health = maxHealth;
        isDead = false;
        UpdateHealthUI();
        playerController = GetComponentInChildren<PlayerController>();
        playerShoot = GetComponentInChildren<PlayerShoot>();
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.playersAlive++;
        gameManager.totalPlayersJoined++;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"Player took {damage} damage. Current health: {health}");
        UpdateHealthUI();
        if (health <= 0f)
        {
            Die();
        }
    }

    public float GetHealthUI()
    {
        var UIHealth = health / 100f;
        return UIHealth;
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = GetHealthUI();
        }
    }

    private void Die()
    {   
        Debug.Log("Player has died.");
        isDead = true;
        deathScreen.SetActive(true);
        gameManager.playersAlive--;
    }

    public void ShowWinScreen()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }
}
