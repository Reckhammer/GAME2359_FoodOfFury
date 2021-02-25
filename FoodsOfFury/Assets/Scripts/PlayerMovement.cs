﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV, Jose Villanueva
 * 
 *  Description: This class handles the players controls
 *  
 *  */

public class PlayerMovement : MonoBehaviour
{
    public float speed          = 5f;           // speed if player
    public float jumpHeight     = 2f;           // jump force of player
    public float glideRate      = 10f;          // drag rate when gliding
    public float groundDistance = 0.2f;         // distance to check for ground
    public LayerMask ground;                    // layers to check if grounded

    [Range(0.0f, 1.0f)]
    public float airControll = 0.5f;            // amount of controll while in air

    private Rigidbody rb;                       // rigidbody of player
    private Transform groundChecker;            // position of groundchecker
    private Vector3 inputs      = Vector3.zero; // inputs from player
    private float originalDrag  = 1.0f;         // original drag of object
    private const int maxJump   = 1;            // max amount of jumps
    private int currentJump     = 0;            // current jump index
    private bool isGrounded     = true;         // for ground check

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(0);
        originalDrag = rb.drag;
    }

    void FixedUpdate()
    {
        // get movement based on camera rotation and player inputs
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized; // cam forward without y value
        Vector3 camRight   = Camera.main.transform.right;
        Vector3 movement   = inputs.z * camForward + inputs.x * camRight;
        movement = Vector3.ClampMagnitude(movement, speed); // clamp magnitiude of vector by speed (stops diagonal movement being faster than hor/ver movement)
        rb.AddForce(movement); // apply movement
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        inputs = Vector3.zero;

        // get player horzontal and vertical inputs
        if (isGrounded) // normal movement
        {
            inputs.x = Input.GetAxisRaw("Horizontal") * speed;
            inputs.z = Input.GetAxisRaw("Vertical") * speed;
        }
        else // in air, use air modifier
        {
            inputs.x = Input.GetAxisRaw("Horizontal") * speed * airControll;
            inputs.z = Input.GetAxisRaw("Vertical") * speed * airControll;
        }

        //if (inputs != Vector3.zero)
        //transform.forward = inputs;

        // start glide if 'x' is pressed (when not grounded)
        if (Input.GetKey("x") && !isGrounded)
        {
            rb.drag = glideRate;
            currentJump = 1;
        }

        // stop glide if 'x' is released or grounded
        if (Input.GetKeyUp("x") || isGrounded)
        {
            rb.drag = originalDrag;
        }

        if (isGrounded)
        {
            currentJump = 0;
        }

        // space bar makes the character jump
        if (Input.GetButtonDown("Jump") && (isGrounded || maxJump > currentJump))
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            currentJump++;
        }
    }
}
