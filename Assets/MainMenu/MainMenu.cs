using System;
using Unity.Netcode;
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
    
    
    
    private const string TutorialSceneName = "Tutorial";
    private const string GameSceneName = "Game";
    
    
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

    private void HandleClientClicked()
    {
    }

    //turns out if you directly load scene after starting host -> you get the player there? instead of loading -> then hosting
    private void HandleHostClicked()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(GameSceneName, LoadSceneMode.Single);
    }
}
