using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Vector3 player1SpawnPosition = new Vector3(-5f, 1f, 0f);
    [SerializeField] private Vector3 player2SpawnPosition = new Vector3(5f, 1f, 0f);
    [SerializeField] private Vector3 player1Rotation = new Vector3(0f, 90f, 0f);
    [SerializeField] private Vector3 player2Rotation = new Vector3(0f, -90f, 0f);

    private void Start()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        response.Approved = true;
        response.CreatePlayerObject = true;

        int currentConnectedCount = NetworkManager.Singleton.ConnectedClientsIds.Count;

        if (currentConnectedCount == 0)
        {
            response.Position = player1SpawnPosition;
            response.Rotation = Quaternion.Euler(player1Rotation);
        }
        else
        {
            response.Position = player2SpawnPosition;
            response.Rotation = Quaternion.Euler(player2Rotation);
        }

        response.Pending = false;
    }
}