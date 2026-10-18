using Unity.Netcode;
using UnityEngine;

public class LatePlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }


    private void OnClientConnected(ulong clientId)
    {
        //server player is spawned automatically
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }

        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            return;
        }

        Debug.Log($"Client {clientId} connected to the live game session!");

        //issue is coming from main menu -> host is spawned auto but p2 isn't so we spawn them in when they connect and pray
        GameObject playerInstance = Instantiate(playerPrefab);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}