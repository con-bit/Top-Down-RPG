using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In charge of controlling player movement
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // the player's speed in units/second
    [SerializeField] private float walkingSpeed;
    // the player's increased speed in units/second
    [SerializeField] private float sprint;
    // speed to be changed in our code
    private float speed;

    [SerializeField] private float rollSpeed = 10f;        // Speed of the roll
    [SerializeField] private float rollDuration = 0.2f;    // How long the roll lasts
    [SerializeField] private float rollCooldown = 1f;      // Cooldown time before the player can roll again
    private bool isRolling = true;
    private float rollEndTime;
    private float lastRollTime;

    // reference to game object's rigidbody behavior
    private Rigidbody2D rb;

    // called when script is loaded into memory
    private void Awake()
    {
        speed = walkingSpeed; 
        rb = GetComponent<Rigidbody2D>();
    }

    // called once a game tick
    private void Update()
    {
        // note: using GetAxis() allows use to have some smoothing and damping 
        // built into the input manager, can change to GetAxisRaw() for snappier movement
        if (!isRolling)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")).normalized * speed;
        }

        //// sprinting
        // if shift is pressed, speed increases to sprint value. otherwise, speed stays its original value.
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprint;
        }
        else
        {
            speed = walkingSpeed;
        }

        //// rolling
        // Check if the player presses the roll key and roll is off cooldown and player is moving
        if (Input.GetKeyDown(KeyCode.Space) && (Time.time > lastRollTime + rollCooldown) && !isRolling && ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)))
        {
            StartRoll();
        }
        // If rolling, check if the roll duration has ended
        if (isRolling && Time.time > rollEndTime)
        {
            EndRoll();
        }
        

    }

    private void StartRoll()
    {
        isRolling = true;
        lastRollTime = Time.time; // grabs time of roll based on game time
        rollEndTime = Time.time + rollDuration;

        // Set roll velocity in the direction of player movement
        Vector2 rollDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb.velocity = rollDirection * rollSpeed;
    }
    private void EndRoll()
    {
        isRolling = false;
        rb.velocity = Vector2.zero;  // Stop roll by resetting velocity
    }
}
