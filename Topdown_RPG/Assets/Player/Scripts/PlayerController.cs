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

        // if shift is pressed, speed increases to sprint value. otherwise, speed stays its original value.
        if (Input.GetKey("left shift"))
        {
            speed = sprint;
        }
        else
        {
            speed = walkingSpeed;
        }

        rb.velocity = new Vector2(Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")).normalized * speed;

    }
}
