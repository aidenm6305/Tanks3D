using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playersAlive = 0;
    public int totalPlayersJoined = 0;
    private bool gameEnded = false;

    private void Start()
    {
        playersAlive = 0;
        totalPlayersJoined = 0;
        gameEnded = false;
    }

    void Update()
    {
        // The game requires at least 2 players to join before checking for a single survivor
        if (totalPlayersJoined > 1 && playersAlive == 1 && !gameEnded)
        {
            WinGame();
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
    }
}
