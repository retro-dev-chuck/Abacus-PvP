using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Player input;
    [SerializeField] private PlayerMover mover;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask abacusInputLayer; 
    
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
        if (!IsOwner)
        {
            return;
        }

        var stick = GetClickedStick();
        if (stick != null)
        {
            stick.MoveOneUp();
        }
    }
    
    private void HandlePressInteractAlt()
    {
        if (!IsOwner)
        {
            return;
        }

        var stick = GetClickedStick();
        if (stick != null)
        {
            stick.MoveOneDown();
        }
    }

    private StickManager GetClickedStick()
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, abacusInputLayer))
        {
            Transform clickedStick = hit.transform;

            StickManager stickManager = clickedStick.GetComponentInParent<StickManager>();

            if (stickManager != null)
            {
                //don't move other player's stones! no cool
                if (!stickManager.IsOwner)
                {
                    return null;
                }

                return stickManager;

            }
        }
        
        //raycast miss  
        return null;
    } 
}
