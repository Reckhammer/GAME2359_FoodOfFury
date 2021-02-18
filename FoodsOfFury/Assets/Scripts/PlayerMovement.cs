using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV
 * 
 *  Description: This class handles the players controls
 *  
 *  */

public class PlayerMovement : MonoBehaviour
{

    public float Speed = 5f;
    public float JumpHeight = 2f;
    public float fallRate = 10f;
    private const int maxJump = 1;
    private int currentJump = 0;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;

    private Rigidbody rb;
    private Vector3 inputs = Vector3.zero;
    private bool isGrounded = true;
    private Transform groundChecker;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(0);
    }

    // Moves the character with arrow keys and AWSD
    void FixedUpdate()
    {
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Camera.main.transform.right;
        Vector3 movement = inputs.z * camForward + inputs.x * camRight;
        rb.velocity = new Vector3(movement.x * Speed, rb.velocity.y, movement.z * Speed);
    }


    void Update()
    {
        isGrounded = Physics.CheckSphere(groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (isGrounded == true)
        {
            currentJump = 0;
        }

        // X makes the character glide down
        if (Input.GetKey("x") && (isGrounded == false))
        {
            rb.drag = fallRate;
            currentJump = 1;
        }

        if (Input.GetKeyUp("x"))
        {
            rb.drag = 0;
        }

        inputs = Vector3.zero;
        inputs.x = Input.GetAxis("Horizontal");
        inputs.z = Input.GetAxis("Vertical");
        //if (inputs != Vector3.zero)
            //transform.forward = inputs;

        // space bar makes the character jump.
        if (Input.GetButtonDown("Jump") && (isGrounded || maxJump > currentJump))
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            currentJump++;
        }

    }

}
