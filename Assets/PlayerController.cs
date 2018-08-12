﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    /*** Static Public Fields  ***/
    static public PlayerController instance;

    /*** Public Fields ***/
    public BoxCollider2D playerCollider;

    /*** Public Tweakable Fields ***/
    [Range(0.0f, 1.0f)]
    public float acceleration  = 0.5f;  // The horizontal acceleration experienced while riding a line
    [Range(0.0f, 20.0f)]
    public float decay         = 1.0f;  // The acceleration experienced in a jump before the apex
    [Range(0.0f, 40.0f)]
    public float gravity       = 1.0f;  // The acceleration experienced in a jump after the apex
    [Range(0.0f, 5.0f)]
    public float startingSpeed = 0.25f; // The starting horizontal speed of the player
    [Range(0.0f, 5.0f)]
    public float jumpingSpeed  = 3.0f; // The horizontal speed while midjump
    [Range(0.0f, 1.0f)]
    public float apexTime      = 1.0f;  // The time that a jump will be rising
    [Range(0.0f, 30.0f)]
    public float jumpVel       = 2.0f;  // The initial velocity of a jump
    [Range(0.0f, 0.5f)]
    public float grabTime      = 0.25f; // The amount of time a grab is active
    /*** Properties ***/
    public Vector2 Velocity
    {
        get{ return velocity; }
    }

    /*** Private Fields for keeping state ***/
    private bool falling = false;      // Falling after the apex of a jump
    private bool riding  = false;      // Riding a line
    private bool rising  = false;      // Rising in a jump before the apex
    private bool canGrab = false;       // The ability to grab, one chance per jump
    private bool touchingNext = false; // If the player is touching the next platform

    private float jumpTimer = 0.0f; // Time spent in current jump
    private float grabTimer = 0.0f; // Time that grab has been active

    private Vector2 velocity;

    void Awake()
    {
        instance = this;
    }

    void Start() 
    {
        Init();
    }
	
    void Update() 
    {
        if (canGrab) grabTimer += Time.deltaTime;
        UpdateInput();
        UpdateJump();
        UpdateVelocity();
        UpdatePosition();
    }

    void Init()
    {

    }

    public void StartRiding()
    {
        // Set state
        riding = true;
        falling = false;
        touchingNext = false;
        canGrab = false;
        grabTimer = 0.0f;
        if (velocity.x == 0.0f) velocity.Set(startingSpeed, 0.0f);

        // Align vertical position with riding platform
        float platformY = GameManager.instance.ActivePlatform.transform.position.y;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered " + other);
        GameObject collidingObj = other.gameObject.transform.parent.gameObject;
        GameObject nextPlatformObj = GameManager.instance.NextPlatform.gameObject;
        if (collidingObj == nextPlatformObj)
        {
            touchingNext = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited " + other);
        GameObject collidingObj = other.gameObject.transform.parent.gameObject;
        GameObject nextPlatformObj = GameManager.instance.NextPlatform.gameObject;
        if (collidingObj == nextPlatformObj)
        {
            touchingNext = false;
        }
    }

    void StartJump()
    {
        // Set state
        riding = false;
        rising = true;
        jumpTimer = 0.0f;

        // Apply jump velocity
        velocity.Set(velocity.x, jumpVel);
    }

    bool GrabLine()
    {
        if (grabTimer < grabTime)
        {
            Debug.Log("Can grab");
            if (playerCollider.IsTouching(GameManager.instance.NextPlatform.lineCollider))
            {
                Debug.Log("Colliding");
                GameManager.instance.GrabPlatform();
                StartRiding();
                return true;
            }
            else Debug.Log("Not colliding");
        }
        else Debug.Log("Can not grab");
        return false;
    }

    // Updates state due to inputs
    void UpdateInput()
    {
        if (riding)
        {
            if (Input.GetAxis("Jump") == 0.0f && Input.GetAxis("Fire1") == 0.0f)
            {
                StartJump();
            }
        }
        else
        {
            if (Input.GetAxis("Jump") != 0.0f || Input.GetAxis("Fire1") != 0.0f)
            {
                canGrab = true;
                GrabLine();
            }
        }
    }

    // Update jumping state
    void UpdateJump()
    {
        if (!riding)
        {
            jumpTimer += Time.deltaTime;
            if (rising && jumpTimer >= apexTime)
            {
                rising = false;
                falling = true;
            }
        }
    }

    // Update velocity due to 'physics'
    // Utilizes delta time so safe to call in Update()
    void UpdateVelocity()
    {
        // Update Vertical
        if (rising)
        {
            velocity.Set(jumpingSpeed, velocity.y - decay * Time.deltaTime);
        }
        else if (falling)
        {
            velocity.Set(jumpingSpeed, velocity.y - gravity * Time.deltaTime);
        }

        // Update Horizontal
        if (riding)
        {
            velocity.Set(velocity.x + acceleration * Time.deltaTime, 0.0f);
        }
    }

    // Update transform position with current velocity
    // Utilizes delta time so safe to call in Update()
    void UpdatePosition()
    {
        Vector3 newPos = new Vector3(
            transform.position.x + velocity.x * Time.deltaTime,
            transform.position.y + velocity.y * Time.deltaTime,
            0.0f);
        transform.position = newPos;
    }
}
