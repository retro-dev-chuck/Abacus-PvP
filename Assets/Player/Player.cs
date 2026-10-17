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
    [SerializeField] private Transform playerCamera;
    
    private bool invertY = true;
    
    private const float MaxPitch = 89f;
    [SerializeField]
    private float lookSensitivity = .1f;
    public event Action OnPressedInteract;
    public event Action OnPressedInteractAlt;
    public Vector2 MovementInput { get; private set; }

    
    private Vector2 rawInput;
    private float pitch = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
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
//disable looking around
        //LookUpdate();

    }
    
    private void LookUpdate()
    {
        var lookInput = lookRef.action.ReadValue<Vector2>();

        pitch += lookInput.y * lookSensitivity * (invertY ? -1 : 1);
        pitch = Mathf.Clamp(pitch, -MaxPitch, MaxPitch);

        playerCamera.localRotation = Quaternion.Euler(pitch, 0, 0);
        transform.Rotate(0, lookInput.x * lookSensitivity, 0);
    }
}
