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

            // 1. Disable CharacterController if it was accidentally left on the prefab
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            // 2. Teleport the transform
            player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            // 3. Force Rigidbody to update immediately (fixes physics snap-back/interpolation bugs)
            var rb = player.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.position = spawnPoint.position;
                rb.rotation = spawnPoint.rotation;
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // 1b. Re-enable CharacterController if it existed
            if (cc != null) cc.enabled = true;

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
            if (mesh.name == "SM_TankTread" || mesh.name == "sm_TreadWheelGroup" || mesh.name == "SM_TankTread (1)" || mesh.name == "sm_TreadWheelGroup (1)")
            {
                continue; // Skip the tank wheels
            }
            mesh.material.color = color;
        }
        
        nextPlayerIndex++;

        // Notify GameManager to start/reset the countdown
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerJoined();
        }
    }


    private static Color GetRandomColor()
    {
        return new Color(Random.Range(0.25f, 0.5f), Random.Range(0.25f, 0.5f), Random.Range(0.25f, 0.5f));
    }
}
