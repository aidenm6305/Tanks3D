using UnityEngine;
using Unity.Cinemachine;

public class SplitScreenCameraSetup : MonoBehaviour
{
    [SerializeField] private CinemachineBrain playerBrain;
    [SerializeField] private CinemachineCamera playerVirtualCam;

    private bool isConfigured;
    private int assignedPlayerIndex = -1;

    public void SetupPlayer(int playerIndex)
    {
        if (isConfigured && assignedPlayerIndex == playerIndex)
        {
            return;
        }

        if (isConfigured)
        {
            return;
        }

        if (playerIndex < 0 || playerIndex >= 31)
        {
            return;
        }

        // Convert player index (0, 1, 2...) into unique channel bitmasks.
        // Channel 0 = P1, Channel 1 = P2, etc.
        int channelBit = 1 << playerIndex;

        OutputChannels channel = (OutputChannels)channelBit;

        playerBrain.ChannelMask = channel;
        playerVirtualCam.OutputChannel = channel;

        assignedPlayerIndex = playerIndex;
        isConfigured = true;
    }
}
