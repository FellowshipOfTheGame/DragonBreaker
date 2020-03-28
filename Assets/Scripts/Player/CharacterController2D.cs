using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 825f;                            // Amount of force added when the player jumps.
    [SerializeField] private float dashSpeed;                                   // Velocity when the player dashes.
    [SerializeField] private float dashTime;                                  // Duration of the player dash *IN MILLISECONDS*.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

    private float dashTimeLeft;         // What time is left for the end of the dash
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
    private int dashDirection = 0;
    float horizontalMove = 0f;
    private bool jump = false;
    private float runSpeed = 40f;
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;

    Vector3 lineStart = Vector3.zero;
    Vector3 lineEnd = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnDashEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        // dashTime /= 1000;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnDashEvent == null)
            OnDashEvent = new UnityEvent();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool jumpDown = Input.GetButtonDown("Jump");
        bool dashDown = Input.GetButtonDown("Dash");

        horizontalMove = horizontal * runSpeed;

        // If jump is pressed;
        if (jumpDown) jump = true;

        // If dash is pressed
        if (dashDown && dashDirection == 0)
        {
            dashTimeLeft = dashTime;
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.angularVelocity = 0;

            if (horizontal > 0 && vertical > 0) dashDirection = 1;      //right&up
            else if (horizontal > 0 && vertical < 0) dashDirection = 2; //right&down
            else if (horizontal < 0 && vertical > 0) dashDirection = 3; //left&up
            else if (horizontal < 0 && vertical < 0) dashDirection = 4; //left&down
            else if (horizontal > 0) dashDirection = 5;                 //right
            else if (horizontal < 0) dashDirection = 6;                 //left
            else if (vertical > 0) dashDirection = 7;                   //up
            else if (vertical < 0) dashDirection = 8;                   //down

            OnDashEvent.Invoke();
        }
    }

    private void FixedUpdate()
    {
        // Basic Movement
        Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        // Dash Movement
        if (dashDirection != 0)
        {
            if (dashTimeLeft <= 0)
            {
                dashDirection = 0;
                m_Rigidbody2D.velocity = Vector2.zero;
            } else
            {
                // While there is time for dashing
                dashTimeLeft -= Time.fixedDeltaTime;
                // Determine the velocity according to the direction
                Dash(dashDirection);
            }
        }

        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (dashDirection == 0 && m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }

    public void Dash(int direction)
    {
        if (direction == 1) m_Rigidbody2D.velocity = (Vector2.right + Vector2.up) * dashSpeed;
        else if (direction == 2) m_Rigidbody2D.velocity = (Vector2.right + Vector2.down) * dashSpeed;
        else if (direction == 3) m_Rigidbody2D.velocity = (Vector2.left + Vector2.up) * dashSpeed;
        else if (direction == 4) m_Rigidbody2D.velocity = (Vector2.left + Vector2.down) * dashSpeed;
        else if (direction == 5) m_Rigidbody2D.velocity = Vector2.right * dashSpeed;
        else if (direction == 6) m_Rigidbody2D.velocity = Vector2.left * dashSpeed;
        else if (direction == 7) m_Rigidbody2D.velocity = Vector2.up * dashSpeed;
        else if (direction == 8 ) m_Rigidbody2D.velocity = Vector2.down * dashSpeed;
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
