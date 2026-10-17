using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier = 0.1f;
    [SerializeField] private CharacterController characterController;
    public void Move(Vector2 movementInput)
    {
        characterController.Move(new Vector3(-movementInput.x, 0 , -movementInput.y) * velocityMultiplier);
    }
}
