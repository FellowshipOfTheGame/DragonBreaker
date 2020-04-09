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
        inputAction["Attack"].performed += ctx => breaker.Attack(movementInput);
        inputAction["Jump"].started += ctx => controller.Jump();
        inputAction["Dash"].started += ctx => controller.Dash(movementInput);
    }

    // Moves the player
    private void FixedUpdate() => controller.Move(movementInput.x * runSpeed * Time.fixedDeltaTime);
    //private void OnEnable() => inputAction.Enable();
    //private void OnDisable() => inputAction.Disable();
}
