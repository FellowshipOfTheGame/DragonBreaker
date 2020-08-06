using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    InputActionAsset inputAction;
    Vector2 movementInput;

    Animator animator;

    [SerializeField] CharacterController2D controller = null;      // Reference to a controller2D
    [SerializeField] Breaker breaker = null;
    [SerializeField] float runSpeed = 40f;                  // Speed of running

    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputAction = GetComponent<PlayerInput>().actions;
        Debug.Log(inputAction);

        EnableActions();
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
        if (animator.GetBool("walking") == false && movementInput.x != 0) animator.SetBool("walking", true);
        else if (animator.GetBool("walking") == true && movementInput.x == 0) animator.SetBool("walking", false);
    }
    private void AttackPerformed(InputAction.CallbackContext ctx) => breaker.Attack(movementInput, controller.facingRight);
    private void JumpStarted(InputAction.CallbackContext ctx) => controller.Jump();
    private void DashStarted(InputAction.CallbackContext ctx) => controller.Dash(movementInput);
}
