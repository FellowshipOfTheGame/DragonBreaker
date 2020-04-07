using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerInputActions inputAction;
    Vector2 movementInput;

    [SerializeField] CharacterController2D controller;      // Reference to a controller2D
    [SerializeField] float runSpeed = 40f;                  // Speed of running
    [SerializeField] float dashTime = 0.1f;                 // Total duration of a dash

    private void Awake()
    {
        inputAction = new PlayerInputActions();
        // If arrows are pressed, sets the direction of movement;
        inputAction.PlayerControls.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.Jump.started += ctx => controller.Jump();
        inputAction.PlayerControls.Dash.started += ctx => Dash();
    }

    // Moves the player
    private void FixedUpdate() => controller.Move(movementInput.x * runSpeed * Time.fixedDeltaTime);

    private void Dash()
    {
        /*
         * Dash direction.
         * 0 - not dashing
         * 1 - right&up
         * 2 - right&down
         * 3 - left&up
         * 4 - left&down
         * 5 - right
         * 6 - left
         * 7 - up
         * 8 - down
         */
        int dashDirection = 0;

        // Determines a direction
        if (movementInput.x > 0 && movementInput.y > 0) dashDirection = 1;        //right&up
        else if (movementInput.x > 0 && movementInput.y < 0) dashDirection = 2;   //right&down
        else if (movementInput.x < 0 && movementInput.y > 0) dashDirection = 3;   //left&up
        else if (movementInput.x < 0 && movementInput.y < 0) dashDirection = 4;   //left&down
        else if (movementInput.x > 0) dashDirection = 5;                     //right
        else if (movementInput.x < 0) dashDirection = 6;                     //left
        else if (movementInput.y > 0) dashDirection = 7;                     //up
        else if (movementInput.y < 0) dashDirection = 8;                     //down

        controller.Dash(dashTime, dashDirection);
    }

    private void OnEnable() => inputAction.Enable();
    private void OnDisable() => inputAction.Disable();
}
