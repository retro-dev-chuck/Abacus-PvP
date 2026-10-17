using System;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerUi : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] private Button clientDisconnectButton;

    
    public event Action OnHostStarted;
    public event Action OnClientStarted;
    public event Action OnClientDisconnected;

    private void OnNetworkSpawn ()
    {
        hostButton.onClick.AddListener(HandleHostClicked);
        clientButton.onClick.AddListener(HandleClientClicked);
        clientDisconnectButton.onClick.AddListener(HandleClientDisconnectClicked);
        EnableConnectButtons();
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
        OnClientDisconnected?.Invoke();
    }

    private void HandleClientClicked()
    {
        OnClientStarted?.Invoke();
    }

    private void HandleHostClicked()
    {
        OnHostStarted?.Invoke();
    }
}
