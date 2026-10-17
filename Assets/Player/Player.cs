using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : NetworkBehaviour
{
    [Header("Input Refs")]
    [SerializeField] private InputActionReference moveRef;
    [SerializeField] private InputActionReference lookRef;
    
    [Header("Movement")]
    [SerializeField] private float movementSmoothTime = 0.1f;
    [SerializeField] private float sensitivityMultiplier = 1.0f;
    
    public event Action OnPressedInteract;
    public event Action OnPressedInteractAlt;

    private Vector2 rawInput;
    public Vector2 MovementInput { get; private set; }
    
    private void Update()
    {
        rawInput = moveRef.action.ReadValue<Vector2>();
        MovementInput = Vector2.MoveTowards(MovementInput, rawInput, Time.deltaTime / movementSmoothTime);
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnPressedInteract?.Invoke();
            Debug.LogWarning("Pressed LMB");
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            OnPressedInteractAlt?.Invoke();
            Debug.LogWarning("Pressed LMB");
        }
    }
}
