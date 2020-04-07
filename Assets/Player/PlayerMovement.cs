using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputActions inputAction;
    Vector2 movementInput;

    [SerializeField] CharacterController2D controller;      // Reference to a controller2D
    [SerializeField] float runSpeed = 40f;                  // Speed of running

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        // If arrows are pressed, sets the direction of movement;
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Jump.started += ctx => controller.Jump();
        inputAction.PlayerControls.Dash.started += ctx => controller.Dash(movementInput);
    }

    // Moves the player
    private void FixedUpdate() => controller.Move(movementInput.x * runSpeed * Time.fixedDeltaTime);
    private void OnEnable() => inputAction.Enable();
    private void OnDisable() => inputAction.Disable();
}
