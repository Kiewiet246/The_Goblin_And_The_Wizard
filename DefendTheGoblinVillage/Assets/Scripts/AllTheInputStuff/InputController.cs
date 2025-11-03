using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    public Vector2 movement;
    public bool attacking;
    public Vector2 zoom;
    public bool rotateLeft;
    public bool rotateRight;
    public bool changePhase;
    public bool cycling;


    public void PlayerMovement(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void PlayerZoom(InputAction.CallbackContext context)
    {
        zoom = context.ReadValue<Vector2>();
    }

    public void PlayerRotateLeft(InputAction.CallbackContext context)
    {
        rotateLeft = context.ReadValueAsButton();
    }

    public void PlayerRotateRight(InputAction.CallbackContext context)
    {
        rotateRight = context.ReadValueAsButton();
    }

    public void PlayerAttacking(InputAction.CallbackContext context)
    {
        attacking = context.ReadValueAsButton();
    }

    public void PlayerChangePhase(InputAction.CallbackContext context)
    {
        changePhase = context.ReadValueAsButton();
    }

    public void PlayerCycle(InputAction.CallbackContext context)
    {
        cycling = context.ReadValueAsButton();
    }
} 
