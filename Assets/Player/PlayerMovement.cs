using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputActionAsset inputAction;
    Vector2 movementInput;

    Animator animator;

    [SerializeField] CharacterController2D controller;      // Reference to a controller2D
    [SerializeField] Breaker breaker;
    [SerializeField] float runSpeed = 40f;                  // Speed of running

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputAction = GetComponent<PlayerInput>().actions;

        EnableActions();
    }

    private void OnDrawGizmos()
    {
        float x_offset = movementInput.x, y_offset = movementInput.y;
        if (movementInput == Vector2.zero)
        {
            if (!controller.facingRight) x_offset = 1;
            else x_offset = -1;
        }
        Gizmos.DrawWireSphere(new Vector3(x_offset + transform.position.x, y_offset + transform.position.y, 0), breaker.range);
    }

    // Moves the player
    private void FixedUpdate()
    {
        controller.Move(movementInput.x * runSpeed * Time.fixedDeltaTime);
    }

    private void EnableActions()
    {
        inputAction["Move"].performed += MovePerformed;
        inputAction["Attack"].performed += AttackPerformed;
        inputAction["Jump"].started += JumpStarted;
        inputAction["Dash"].started += DashStarted;
        inputAction.Enable();
    }

    private void DisableActions()
    {
        inputAction["Move"].performed -= MovePerformed;
        inputAction["Attack"].performed -= AttackPerformed;
        inputAction["Jump"].started -= JumpStarted;
        inputAction["Dash"].started -= DashStarted;
        inputAction.Disable();
    }

    private void OnDestroy()
    {
        DisableActions();
    }

    private void MovePerformed(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
        if (animator.GetBool("walking") != true && movementInput.x != 0) animator.SetBool("walking", true);
        else if (animator.GetBool("walking") != false) animator.SetBool("walking", false);
    }
    private void AttackPerformed(InputAction.CallbackContext ctx) => breaker.Attack(movementInput, controller.facingRight);
    private void JumpStarted(InputAction.CallbackContext ctx) => controller.Jump();
    private void DashStarted(InputAction.CallbackContext ctx) => controller.Dash(movementInput);
}
