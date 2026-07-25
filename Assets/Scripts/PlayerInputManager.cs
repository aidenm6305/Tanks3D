using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxPlayers = 4;

    private bool wasdJoined = false;
    //private bool arrowsJoined = false;
    private readonly HashSet<Gamepad> joinedGamepads = new();

    private int nextPlayerIndex = 0;

    void Update()
    {
        if (Keyboard.current == null) return;

        if (!wasdJoined && Keyboard.current.spaceKey.wasPressedThisFrame && nextPlayerIndex < maxPlayers)
        {
            SpawnPlayer(Keyboard.current, "WASD");
            wasdJoined = true;
        }

        foreach (var gamePad in Gamepad.all)
        {
            if (nextPlayerIndex >= maxPlayers) break;

            if (gamePad.buttonSouth.wasPressedThisFrame && joinedGamepads.Add(gamePad))
            {
                SpawnPlayer(gamePad, "Gamepad");
            }
        }
    }

    private void SpawnPlayer(InputDevice device, string controlScheme)
    {
        int playerIndex = nextPlayerIndex;

        var player = PlayerInput.Instantiate(
            playerPrefab,
            controlScheme: controlScheme,
            pairWithDevice: device);

        var splitScreen = player.GetComponent<SplitScreenCameraSetup>();
        splitScreen.SetupPlayer(playerIndex);

        if (spawnPoints.Length > playerIndex)
        {
            player.transform.position = spawnPoints[playerIndex].position;
        }

        player.GetComponent<Renderer>().material.color = GetRandomColor();
        var color = GetRandomColor();
        foreach (var mesh in player.GetComponentsInChildren<MeshRenderer>())
        {
            mesh.material.color = color;
        }
        nextPlayerIndex++;
    }

    private static Color GetRandomColor()
    {
        return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }
}
