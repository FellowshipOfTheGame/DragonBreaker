using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] CharacterController2D controller;      // Reference to a controller2D
    [SerializeField] float runSpeed = 40f;                  // Speed of running
    [SerializeField] float dashTime = 0.1f;                 // Total duration of a dash
    bool jump = false;                                      // Makes the player jump when setted
    float horizontalMove = 0f;                              // Direction of running * runSpeed
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

    void Update()
    {
        // Get controls
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool jumpDown = Input.GetButtonDown("Jump");
        bool dashDown = Input.GetButtonDown("Dash");

        // If jump is pressed, jumps;
        if (jumpDown) jump = true;

        // If arrows are pressed, sets the direction of move;
        horizontalMove = horizontal * runSpeed;

        // If dash is pressed
        if (dashDown && dashDirection == 0)
        {
            // Determines a direction
            if (horizontal > 0 && vertical > 0) dashDirection = 1;      //right&up
            else if (horizontal > 0 && vertical < 0) dashDirection = 2; //right&down
            else if (horizontal < 0 && vertical > 0) dashDirection = 3; //left&up
            else if (horizontal < 0 && vertical < 0) dashDirection = 4; //left&down
            else if (horizontal > 0) dashDirection = 5;                 //right
            else if (horizontal < 0) dashDirection = 6;                 //left
            else if (vertical > 0) dashDirection = 7;                   //up
            else if (vertical < 0) dashDirection = 8;                   //down
        }
    }

    private void FixedUpdate()
    {
        // Moves the player
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;

        // Dashes the player
        if (dashDirection != 0)
        {
            controller.Dash(dashTime, dashDirection);
            dashDirection = 0;
        }
    }
}
