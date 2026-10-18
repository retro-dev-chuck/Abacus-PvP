using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerListText;
    [SerializeField] private Button startGameButton; 
    [SerializeField] private Button backButton; 

    private void Start()
    {
        // Only the host should be allowed to click the start game button
        if (startGameButton != null)
        {
            startGameButton.gameObject.SetActive(NetworkManager.Singleton.IsHost);
            startGameButton.onClick.AddListener(HandleStartGameClicked);
        }

        // Listen for players joining or leaving
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

        UpdatePlayerListUI();
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        UpdatePlayerListUI();
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        UpdatePlayerListUI();
    }

    private void UpdatePlayerListUI()
    {
        if (playerListText == null) return;

        string playerListString = "Connected Players:\n";
        
        foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            string playerLabel = (clientId == NetworkManager.Singleton.LocalClientId) ? "You" : $"Player {clientId}";
            playerListString += $"- {playerLabel} (ID: {clientId})\n";
        }

        playerListText.text = playerListString;
    }

    private void HandleStartGameClicked()
    {
        if (!NetworkManager.Singleton.IsHost) return;

        var status = NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);

        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogError($"Failed to trigger network scene load! Status: {status}");
        }
        else
        {
            Debug.Log("Network scene load started successfully for all clients.");
        }
    }

    private void HandleBackClicked()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.Shutdown();
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}