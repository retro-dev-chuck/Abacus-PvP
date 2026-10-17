using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Player input;
    [SerializeField] private PlayerMover mover;
    [SerializeField] private Camera playerCamera;
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector2 movementInput = input.MovementInput;
        mover.Move(movementInput);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner) { return; } // ALL players will read this method, only player owner will execute past this line
        playerCamera.enabled = true; // only enable YOUR PLAYER'S camera, all others will stay disabled
    }
}
