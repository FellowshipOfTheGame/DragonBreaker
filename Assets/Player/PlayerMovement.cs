using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputActionAsset inputAction;
    Vector2 movementInput;

    [SerializeField] CharacterController2D controller;      // Reference to a controller2D
    [SerializeField] Breaker breaker;
    [SerializeField] float runSpeed = 40f;                  // Speed of running

    private void Awake()
    {
        inputAction = GetComponent<PlayerInput>().actions;

        // If arrows are pressed, sets the direction of movement;
        inputAction["Move"].performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction["Attack"].performed += ctx => breaker.Attack(movementInput, controller.facingRight);
        inputAction["Jump"].started += ctx => controller.Jump();
        inputAction["Dash"].started += ctx => controller.Dash(movementInput);
    }

    private void OnDrawGizmos()
    {
        float x_offset = 0;
        if (movementInput == Vector2.zero)
        {
            if (controller.facingRight) x_offset = 1;
            else x_offset = -1;
        }
        else x_offset = movementInput.x;
        Gizmos.DrawWireSphere(new Vector3(x_offset + transform.position.x, movementInput.y + transform.position.y, 0), breaker.range);
    }

    // Moves the player
    private void FixedUpdate() => controller.Move(movementInput.x * runSpeed * Time.fixedDeltaTime);
    //private void OnEnable() => inputAction.Enable();
    //private void OnDisable() => inputAction.Disable();
}
