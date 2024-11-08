using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 maxSpeed;        // Max speed the player can reach
    [SerializeField] private Vector2 timeToFullSpeed; // Time to reach full speed
    [SerializeField] private Vector2 timeToStop;      // Time to stop completely
    [SerializeField] private Vector2 stopClamp;       // Minimum velocity before stopping

    private Vector2 moveDirection;   // Direction of movement
    private Vector2 moveVelocity;    // Velocity applied to Rigidbody2D
    private Vector2 moveFriction;    // Friction applied while moving
    private Vector2 stopFriction;    // Friction applied when stopping
    private Rigidbody2D rb;          // Reference to Rigidbody2D

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Calculate movement-related values based on the input settings
        moveVelocity = 2 * maxSpeed / timeToFullSpeed;
        moveFriction = -2 * maxSpeed / (timeToFullSpeed * timeToFullSpeed);
        stopFriction = -2 * maxSpeed / (timeToStop * timeToStop);
    }
    public void Move()
    {
        // Calculate move direction based on input
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        moveDirection = new Vector2(inputX, inputY).normalized;

        Vector2 friction = GetFriction();
        Vector2 velocity = moveDirection * maxSpeed;
        velocity -= friction * Time.fixedDeltaTime;

        rb.velocity = velocity;

        // Clamp velocity to max speed on each axis
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed.x, maxSpeed.x);
        velocity.y = Mathf.Clamp(velocity.y, -maxSpeed.y, maxSpeed.y);

        // Apply stopClamp to halt the player at low speeds
        if (Mathf.Abs(rb.velocity.x) < stopClamp.x && Mathf.Abs(rb.velocity.y) < stopClamp.y)
        {
            rb.velocity = Vector2.zero;
        }

        MoveBound();
    }


    Vector2 GetFriction()
    {
        // Return friction based on movement state
        return moveDirection != Vector2.zero ? moveFriction : stopFriction;
    }

    void MoveBound()
    {   
        float boundaryPadding = 0.5f;
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        // Calculate position with boundary padding applied
        Vector3 playerPos = transform.position;
        playerPos.x = Mathf.Clamp(playerPos.x, -camWidth + boundaryPadding, camWidth - boundaryPadding);
        playerPos.y = Mathf.Clamp(playerPos.y, -camHeight + boundaryPadding, camHeight - boundaryPadding);

        // Apply clamped position back to player
        transform.position = playerPos;
    }

    public bool IsMoving()
    {
        // Check if the player is moving by evaluating the velocity
        return rb.velocity != Vector2.zero;
    }

}