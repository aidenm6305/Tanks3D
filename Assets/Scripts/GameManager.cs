using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int playersAlive = 0;

    private void Start()
    {
        playersAlive = 0;
    }

    void WinGame() { 
        Debug.Log("You Win!");
    }
}
