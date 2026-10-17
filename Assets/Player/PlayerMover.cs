using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier = 0.1f;
    [SerializeField] private CharacterController characterController;
    public void Move(Vector2 movementInput)
    {
        Transform t = transform;
        Vector3 movement = t.forward * movementInput.y;
        movement += t.right * movementInput.x;
        characterController.Move( movement * velocityMultiplier);
    }
}
