using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 50f;
    private float maxHealth = 50f;
    public Slider healthSlider;
    public bool isDead = false;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TMPro.TextMeshProUGUI countdownText;

    private PlayerController playerController;
    private PlayerShoot playerShoot;
    private GameManager gameManager;

    [Header("Self Destruct")]
    private float upsideDownTimer = 0f;
    private const float selfDestructTime = 5f;

    private void Start()
    {
        deathScreen.SetActive(false);
        if (winScreen != null) winScreen.SetActive(false);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
        
        health = maxHealth;
        isDead = false;
        UpdateHealthUI();
        playerController = GetComponentInChildren<PlayerController>();
        playerShoot = GetComponentInChildren<PlayerShoot>();
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.playersAlive++;
        gameManager.totalPlayersJoined++;
    }

    private void Update()
    {
        if (isDead) return;

        // If the map/game hasn't started, don't tick the timer
        if (gameManager != null && !gameManager.isGameActive) return;

        // Check if the tank is upside down or on its side (Y is near 0 or less)
        if (transform.up.y < 0.1f)
        {
            upsideDownTimer += Time.deltaTime;
            
            if (countdownText != null)
            {
                countdownText.gameObject.SetActive(true);
                float timeLeft = Mathf.Max(0, selfDestructTime - upsideDownTimer);
                countdownText.text = $"Self Destruct: {Mathf.Ceil(timeLeft)}";
            }

            if (upsideDownTimer >= selfDestructTime)
            {
                TakeDamage(health); // Self destruct
            }
        }
        else
        {
            // Reset the timer if they are upright
            upsideDownTimer = 0f;
            
            if (countdownText != null && countdownText.gameObject.activeSelf)
            {
                countdownText.gameObject.SetActive(false);
            }
        }
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
        var UIHealth = health / 50f;
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
        if (isDead) return;
        
        Debug.Log("Player has died.");
        isDead = true;
        
        if (countdownText != null) 
            countdownText.gameObject.SetActive(false);

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
