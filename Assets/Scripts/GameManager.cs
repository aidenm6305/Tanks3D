using System.Timers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playersAlive = 0;
    public int totalPlayersJoined = 0;
    private bool gameEnded = false;

    [Header("Game State")]
    public bool isGameActive = false;
    public float countdownDuration = 5f;
    private float currentCountdown;
    private bool isCountingDown = false;

    [SerializeField]
    private TMPro.TextMeshProUGUI countdownText;

    private void Awake()
    {
        // Simple Singleton setup
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playersAlive = 0;
        totalPlayersJoined = 0;
        gameEnded = false;
        isGameActive = false;
        currentCountdown = countdownDuration;
        countdownText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (isCountingDown && !isGameActive)
        {
            currentCountdown -= Time.deltaTime;

            countdownText.text = Mathf.Ceil(currentCountdown).ToString();
            
            if (currentCountdown <= 0)
            {
                isGameActive = true;
                isCountingDown = false;
                Debug.Log("Game Started!");
                countdownText.gameObject.SetActive(false);
            }
        }

        // The game requires at least 2 players to join before checking for a single survivor
        if (isGameActive && totalPlayersJoined > 1 && playersAlive == 1 && !gameEnded)
        {
            WinGame();
        }
    }

    public void OnPlayerJoined()
    {
        totalPlayersJoined++;
        // Assuming your PlayerHealth increments playersAlive, otherwise you may want to do it here:
        // playersAlive++;

        if (!isGameActive)
        {
            isCountingDown = true;
            // The countdown resets each time a new player joins during the waiting phase
            currentCountdown = countdownDuration; 
            Debug.Log($"Player joined! Starting in {countdownDuration} seconds...");
        }
    }

    void WinGame() 
    { 
        Debug.Log("You Win!");
        gameEnded = true;

        PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            if (!player.isDead)
            {
                player.ShowWinScreen();
            }
        }
        StartCoroutine(StartNewRound());
    }

    public System.Collections.IEnumerator StartNewRound() {
        float elapsedTime = 0f;
        float duration = 5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            countdownText.gameObject.SetActive(true);
            countdownText.text = $"New Round Starting in {Mathf.Ceil(duration - elapsedTime)}";
            yield return null;
        }
        countdownText.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
