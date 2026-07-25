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
            pairWithDevice: device
        );

        var splitScreen = player.GetComponent<SplitScreenCameraSetup>();
        splitScreen.SetupPlayer(playerIndex);

        if (spawnPoints.Length > playerIndex)
        {
            Debug.Log($"Spawning player {playerIndex + 1} at spawn point {playerIndex}");
            var spawnPoint = spawnPoints[playerIndex];
            var characterController = player.GetComponent<CharacterController>();

            characterController.enabled = false;

            player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            characterController.enabled = true;

            Debug.Log($"Player {playerIndex + 1} spawned at position: {player.transform.position}");

        }
        else
        {
            Debug.LogWarning($"Not enough spawn points for player {playerIndex + 1}. Using default position.");
        }
        Debug.Log(spawnPoints.Length);
        
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
