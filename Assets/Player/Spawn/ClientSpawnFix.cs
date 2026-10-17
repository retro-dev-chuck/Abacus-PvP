using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ClientSpawnFix : NetworkBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Vector3 player1SpawnPosition = new Vector3(-2f, 1f, 0f);
    [SerializeField] private Vector3 player2SpawnPosition = new Vector3(5f, 1f, 0f);
    [SerializeField] private Vector3 player1Rotation = new Vector3(0f, 90f, 0f);
    [SerializeField] private Vector3 player2Rotation = new Vector3(0f, -90f, 0f);
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
        StartCoroutine(TeleportRoutine());
    }
    
    //straight from ai answer because what even is this bug
    //explanation: player auth network transform node -> overwrites position on connect due to delay in connection
    //and character controller also overwrote the position so we disable any position updater -> set pos -> re-enable
    private IEnumerator TeleportRoutine()
    {
        yield return new WaitForEndOfFrame();

        int currentConnectedCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Vector3 targetPosition = currentConnectedCount == 1 ? player1SpawnPosition : player2SpawnPosition;
        Vector3 targetRotation = currentConnectedCount == 1 ? player1Rotation : player2Rotation;

        CharacterController cc = GetComponent<CharacterController>();
        Rigidbody rb = GetComponent<Rigidbody>();

        if (cc != null)
        {
            cc.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = true; rb.linearVelocity = Vector3.zero;
        }
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(targetRotation);

        yield return null;

        if (cc != null)
        {
            cc.enabled = true;
        }

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        Debug.Log($"[Spawn Fix] Successfully locked client to target position: {targetPosition}");
    }
}