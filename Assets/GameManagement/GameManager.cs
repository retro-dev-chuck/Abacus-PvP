using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    private MultiplayerUi multiplayerUi;

    private void Start()
    {
        if (multiplayerUi != null)
        {
            multiplayerUi.OnHostStarted += StartHost;
            multiplayerUi.OnClientStarted += StartClient;
            multiplayerUi.OnClientDisconnected += DisconnectClient;
        }
    }

    private void DisconnectClient()
    {
        multiplayerUi.DisableConnectButtons();
        NetworkManager.Shutdown();
        Debug.Log("Disconnect client");
    }

    private void StartClient()
    {
        multiplayerUi.DisableConnectButtons();
        NetworkManager.StartClient();
        Debug.Log("Start client");
    }

    private void StartHost()
    {
        NetworkManager.StartHost();
        Debug.Log("Start Host");
    }
}
