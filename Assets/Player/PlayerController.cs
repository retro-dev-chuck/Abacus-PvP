using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Player input;
    [SerializeField] private PlayerMover mover;

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        Vector2 movementInput = input.MovementInput;
        mover.Move(movementInput);
    }
}
