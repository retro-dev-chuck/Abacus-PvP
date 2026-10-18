using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using Unity.Services.Multiplayer.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private SessionConnectorBehaviour sessionConnector;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button clientDisconnectButton;
    [SerializeField] private TMP_InputField codeInputField;
    
    
    
    private const string TutorialSceneName = "Tutorial";
    private const string GameSceneName = "Game";
    private const string LobbySceneName = "Lobby";
    
    private void Awake ()
    {
        hostButton.onClick.AddListener(HandleHostClicked);
        clientButton.onClick.AddListener(HandleClientClicked);
        clientDisconnectButton.onClick.AddListener(HandleClientDisconnectClicked);
        tutorialButton.onClick.AddListener(HandleTutorialButtonClicked);
        EnableConnectButtons();
    }

    private void HandleTutorialButtonClicked()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(TutorialSceneName, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(TutorialSceneName);
        }
    }

    private void OnDestroy()
    {
        hostButton.onClick.AddListener(HandleHostClicked);
        clientButton.onClick.AddListener(HandleClientClicked);
        clientDisconnectButton.onClick.AddListener(HandleClientDisconnectClicked);
    }

    public void EnableConnectButtons()
    {
        hostButton.gameObject.SetActive(true);
        clientButton.gameObject.SetActive(true);
        clientDisconnectButton.gameObject.SetActive(false);
    }
    
    public void DisableConnectButtons()
    {
        hostButton.gameObject.SetActive(false);
        clientButton.gameObject.SetActive(false);
        clientDisconnectButton.gameObject.SetActive(true);
    }

    private void HandleClientDisconnectClicked()
    {
    }

    private async void HandleClientClicked()
    {
        clientButton.interactable = false;
        await UnityServices.InitializeAsync();
        while (AuthenticationService.Instance == null || !AuthenticationService.Instance.IsSignedIn)
        {
            await Task.Delay(200);
        }
        
        if (codeInputField == null || string.IsNullOrEmpty(codeInputField.text))
        {
            Debug.LogWarning("Please enter a room code first!");
            return;
        }

        string code = codeInputField.text;
        var sessionConnectorBehaviour = FindObjectOfType<SessionConnectorBehaviour>();
        if (sessionConnectorBehaviour != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            Debug.Log($"Configuring session connector to join code: {code}");
            sessionConnectorBehaviour.SessionConnector.WithJoinByCode(code);
            sessionConnectorBehaviour.Execute();
        }
        else
        {
            Debug.LogError("SessionConnector could not be found in the scene!");
        }

        clientButton.interactable = true;
    }
    private void OnClientConnected(ulong clientId)
    {
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            Debug.Log("Client connected successfully! Moving to Lobby Scene...");
            
            SceneManager.LoadScene(LobbySceneName, LoadSceneMode.Single);
        }
    }

    //turns out if you directly load scene after starting host -> you get the player there? instead of loading -> then hosting
    private async void HandleHostClicked()
    {
        hostButton.interactable = false;
        await UnityServices.InitializeAsync();
        while (AuthenticationService.Instance == null || !AuthenticationService.Instance.IsSignedIn)
        {
            await Task.Delay(200);
        }

        var sessionConnectorBehaviour = FindObjectOfType<SessionConnectorBehaviour>();
        if (sessionConnectorBehaviour != null)
        {
            Debug.Log("Creating host session...");
            
            sessionConnectorBehaviour.SessionConnector.SuccessfulExecution.AddListener(OnHostSessionCreated);

            sessionConnectorBehaviour.SessionConnector.ConnectorType = SessionConnectorType.CreateOrJoin;
            sessionConnectorBehaviour.Execute();
            
            DisableConnectButtons();
        }
        else
        {
            Debug.LogError("SessionConnector could not be found in the scene!");
            hostButton.interactable = true;
        }
    }

    private void OnHostSessionCreated(ISession session)
    {
        var sessionConnectorBehaviour = FindObjectOfType<SessionConnectorBehaviour>();
        if (sessionConnectorBehaviour != null)
        {
            sessionConnectorBehaviour.SessionConnector.SuccessfulExecution.RemoveListener(OnHostSessionCreated);
        }

        Debug.Log("Host session created successfully! Now loading Lobby scene via Netcode...");

        NetworkManager.Singleton.SceneManager.LoadScene(LobbySceneName, LoadSceneMode.Single);
    }
}
