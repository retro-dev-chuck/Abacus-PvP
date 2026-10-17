using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Player input;
    [SerializeField] private PlayerMover mover;
    [SerializeField] private Camera playerCamera;

    [SerializeField] private BottomStickManager[] bottomStickManagers;
     
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
        if (!IsOwner)
        {
            return;
        }
        
        playerCamera.enabled = true;
        input.OnPressedInteract += HandlePressInteract;
        input.OnPressedInteractAlt += HandlePressInteractAlt;
    }


    private void HandlePressInteract()
    {
        foreach (BottomStickManager bottomStickManager in bottomStickManagers)
        {
            bottomStickManager.MoveOneUp();
        }
    }
    
    private void HandlePressInteractAlt()
    {
        foreach (BottomStickManager bottomStickManager in bottomStickManagers)
        {
            bottomStickManager.MoveOneDown();
        }
    }
}
