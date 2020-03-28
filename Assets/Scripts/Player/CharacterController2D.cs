using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 825f;                            // Amount of force added when the player jumps.

    [SerializeField] private int qtdDash;                                       // Velocity when the player dashes.
    [SerializeField] private float dashSpeed;                                   // Velocity when the player dashes.
    [SerializeField] private float dashCooldown;                                // Velocity when the player dashes.
    [SerializeField] private bool restoreDashOnLand = true;                     // Should dashes be restored when landed?

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool airControl = false;                         // Whether or not a player can steer while jumping;

    [SerializeField] private LayerMask whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform groundCheck;                           // A position marking where to check if the player is grounded.

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
    float dashTimeLeft;                 // How much time is left for the end of the dash
    int remainingDashes;                // Holds how much dashes the character can do 

    const float groundedRadius = .2f;   // Radius of the overlap circle to determine if grounded
    bool grounded;                      // Whether or not the player is grounded.

    Rigidbody2D rigidbody2D;
    bool facingRight = true;            // For determining which way the player is currently facing.
    Vector3 velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnDashStartEvent;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        remainingDashes = qtdDash;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnDashStartEvent == null)
            OnDashStartEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        // While there is time for dashing
        dashTimeLeft -= Time.fixedDeltaTime;
        // If dashTimeLeft is negativa and less than dashCoolDown, restores one dash and resets it
        if (remainingDashes < qtdDash && Mathf.Abs(dashTimeLeft) > dashCooldown)
        {
            dashTimeLeft += dashCooldown;
            remainingDashes++;
        }
        // If the player is dashing
        if (dashDirection != 0)
        {
            // Dashes
            if (dashDirection == 1) rigidbody2D.velocity = (Vector2.right + Vector2.up) * dashSpeed * Mathf.Sqrt(2) / 2;
            else if (dashDirection == 2) rigidbody2D.velocity = (Vector2.right + Vector2.down) * dashSpeed * Mathf.Sqrt(2) / 2;
            else if (dashDirection == 3) rigidbody2D.velocity = (Vector2.left + Vector2.up) * dashSpeed * Mathf.Sqrt(2) / 2;
            else if (dashDirection == 4) rigidbody2D.velocity = (Vector2.left + Vector2.down) * dashSpeed * Mathf.Sqrt(2) / 2;
            else if (dashDirection == 5) rigidbody2D.velocity = Vector2.right * dashSpeed;
            else if (dashDirection == 6) rigidbody2D.velocity = Vector2.left * dashSpeed;
            else if (dashDirection == 7) rigidbody2D.velocity = Vector2.up * dashSpeed;
            else if (dashDirection == 8) rigidbody2D.velocity = Vector2.down * dashSpeed;

            // Stops dashing
            if (dashTimeLeft <= 0)
            {
                rigidbody2D.velocity = Vector2.zero;
                dashDirection = 0;
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
                    if (restoreDashOnLand) remainingDashes = qtdDash;
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (dashDirection == 0 && (grounded || airControl))
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            grounded = false;
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    public void Dash(float time, int direction)
    {
        // If the player is dashing or do not has remaining dashes, does not dash
        if (dashDirection != 0 || remainingDashes == 0) return;

        // Removes a dash
        remainingDashes--;

        // Stops the player
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0;

        // Sets timeLeft and direction to the passed arguments
        dashTimeLeft = time;
        dashDirection = direction;

        // Invokes a dashStart
        OnDashStartEvent.Invoke();
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
}
