using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 825f;                            // Amount of force added when the player jumps.

    [SerializeField] private int qtdDash = 1;                                       // Velocity when the player dashes.
    [SerializeField] private float dashTime = 0.1f;                                   // Duration of a dash.
    [SerializeField] private float dashSpeed = 25;                                   // Velocity when the player dashes.

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement

    [SerializeField] private bool airControl = true;                         // Whether or not a player can steer while jumping;

    [SerializeField] private LayerMask whatIsGround = 0;                          // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck = null;                           // A position marking where to check if the player is grounded.

    Vector2 dashDirection;
    bool dash = false;
    float dashTimeLeft;                 // How much time is left for the end of the dash
    int remainingDashes;                // Holds how much dashes the character can do 

    const float groundedRadius = .225f;   // Radius of the overlap circle to determine if grounded
    bool grounded;                      // Whether or not the player is grounded.

    Animator animator;

    Rigidbody2D _rigidbody2D;
    public bool facingRight { get; private set; } = true;            // For determining which way the player is currently facing.
    Vector3 velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnDashStartEvent;
    public UnityEvent OnDashEndEvent;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        remainingDashes = qtdDash;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnDashStartEvent == null)
            OnDashStartEvent = new UnityEvent();
        if (OnDashEndEvent == null)
            OnDashEndEvent = new UnityEvent();

        OnLandEvent.AddListener(OnLanding);
        OnDashStartEvent.AddListener(OnDashStart);
        OnDashEndEvent.AddListener(OnDashEnd);
    }

    private void FixedUpdate()
    {
        // If the player is dashing
        if (dash)
        {
            // While there is time for dashing, dashes
            dashTimeLeft -= Time.fixedDeltaTime;
            _rigidbody2D.velocity = dashDirection * dashSpeed;

            // Stops dashing
            if (dashTimeLeft <= 0)
            {
                _rigidbody2D.velocity = Vector2.zero;
                // Invokes a dashEnd
                OnDashEndEvent.Invoke();
                dash = false;
            }
        }

        bool wasGrounded = grounded;
        grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    // If it is supposed to restore dashes when landed;
                    remainingDashes = qtdDash;
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            _rigidbody2D.velocity = Vector3.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move < 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move > 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    public void Jump()
    {
        animator.SetBool("isJumping", true);
        // If the player should jump...
        if (grounded)
        {
            // Add a vertical force to the player.
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    public void Dash(Vector2 direction)
    {
        // If the player is dashing or do not has remaining dashes, does not dash
        if (remainingDashes == 0) return;

        // Removes a dash
        remainingDashes--;

        // Stops the player
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;

        // Starts dash
        dash = true;
        dashTimeLeft = dashTime;
        dashDirection = direction;

        // Invokes a dashStart
        OnDashStartEvent.Invoke();
    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    public void OnDashStart()
    {
        Debug.Log("Starting dash");
        animator.SetBool("isDashing", true);
    }

    public void OnDashEnd()
    {
        Debug.Log("Finishing dash");
        animator.SetBool("isDashing", false);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundedRadius);
    }
}
