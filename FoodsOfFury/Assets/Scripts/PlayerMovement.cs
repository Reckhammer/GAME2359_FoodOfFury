using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV, Jose Villanueva
//
// Description: This class handles the players movement
//----------------------------------------------------------------------------------------

public class PlayerMovement : MonoBehaviour
{
    public float speed              = 5f;           // speed if player
    public float glideSpeed         = 1.0f;         // movement speed while gliding
    public float jumpHeight         = 2f;           // jump force of player
    public float dashForce          = 1.0f;         // force of dash
    public float dashDelay          = 0.5f;         // time before dash can be used again
    public float groundDistance     = 0.2f;         // distance to check for ground
    public float fallForce          = 1.0f;         // downwards force when falling
    public float rotationSpeed      = 1.0f;         // speed of rotation
    public LayerMask ground;                        // layers to check if grounded

    [Range(0.0f, 1.0f)]
    public float airControll = 0.5f;                // amount of controll while in air

    [Range(1.0f, 0.0f)]
    public float glideFallRate = 0.9f;              // falling rate for gliding (how much gravity)

    private Rigidbody rb;                           // rigidbody of player
    private bool inputStopped       = false;        // for stopping input
    private Transform groundChecker;                // position of groundchecker
    private Vector3 inputs          = Vector3.zero; // inputs from player
    private Vector3 movement        = Vector3.zero; // calculated velocity to move the character
    private const int maxJump       = 1;            // max amount of jumps
    private int currentJump         = 0;            // current jump index
    private bool isGrounded         = true;         // for ground check
    private bool isGliding          = false;        // for gliding check
    private bool canDash            = true;         // for dash delay check
    private Vector3 groundNormal    = Vector3.up;   // normal of the ground
    private Coroutine extraForceCr  = null;         // reference to ExtraForeceTime coroutine

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(0);
    }

    private void FixedUpdate()
    {
        if (!inputStopped)
        {
            rb.AddForce(movement); // apply movement

            if (isGliding)
            {
                rb.AddForce(-Physics.gravity * glideFallRate);
            }
        }

        // if object is moving set rotation
        if (rb.velocity != Vector3.zero)
        {
            Vector3 dir = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1).normalized); // direction = velocity (without y) normalized

            if (dir != Vector3.zero) // direction is zero, don't set
            {
                Quaternion toRotation = Quaternion.LookRotation(dir); // target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * rotationSpeed); // slerp rotation with rotation speed
            }
        }
    }

    private void Update()
    {
        if (inputStopped)
        {
            return;
        }

        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        inputs = Vector3.zero;

        // start glide if 'x' is pressed (when not grounded)
        if (Input.GetKey("x") && !isGrounded)
        {
            currentJump = 1;
            isGliding = true;
        }

        // stop glide if 'x' is released or grounded
        if (Input.GetKeyUp("x") || isGrounded)
        {
            isGliding = false;
        }

        // get player horzontal and vertical inputs
        if (isGrounded) // grounded movement
        {
            inputs.x = Input.GetAxisRaw("Vertical") * speed;
            inputs.z = Input.GetAxisRaw("Horizontal") * speed;
        }
        else if (isGliding) // gliding movement
        {
            inputs.x = Input.GetAxisRaw("Vertical") * glideSpeed;
            inputs.z = Input.GetAxisRaw("Horizontal") * glideSpeed;
        }
        else // air movement (not gliding)
        {
            inputs.x = Input.GetAxisRaw("Vertical") * speed * airControll;
            inputs.z = Input.GetAxisRaw("Horizontal") * speed * airControll;
        }

        checkSlope();           // get slope normal
        calculateMovement();    // calculate movement

        // reset currrentJump when grounded
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

        // falling & not gliding, add downward force
        if (!isGrounded && !isGliding && rb.velocity.y < 0)
        {
            rb.AddForce(fallForce * Vector3.down, ForceMode.Force);
        }

        // do dash
        if (!isGliding && canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (movement.normalized != Vector3.zero) // don't dash if no movement
            {
                applyExtraForce(movement.normalized * dashForce, 0.1f); // apply dash
                StartCoroutine(DashDelayTimer());                       // start dash delay timer
            }
        }
    }

    // calculate movement based on camera rotation and player inputs
    private void calculateMovement()
    {
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized; // cam forward without y value
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;     // cam right without y value
        movement = inputs.x * camForward + inputs.z * camRight;                                             // calculate movement (velocity)
        movement = Vector3.ProjectOnPlane(movement, groundNormal);                                          // project movement to ground normal
        movement = Vector3.ClampMagnitude(movement, speed); // clamp magnitiude of vector by speed (stops diagonal movement being faster than hor/ver movement)
    }

    // sets groundNormal to the normal of the ground
    private void checkSlope(float distance = 1.0f)
    {
        Vector3 start = groundChecker.position;
        Vector3 end = groundChecker.position + (Vector3.down * distance);
        Debug.DrawLine(start, end, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(start, Vector3.down, out hit, distance, ground, QueryTriggerInteraction.Ignore))
        {
            //Debug.DrawLine(start, start + (hit.normal * 5.0f), Color.red); // draw normal of ground
            groundNormal = hit.normal;

            //float angle = Vector3.Angle(hit.normal, Vector3.up);
            //print("angle: " + angle);

            // flat = 0
            // steep angle = bigger

            //float angle = Vector3.Angle(hit.normal, transform.forward);
            //print("angle: " + angle);
        }
    }

    public void applyExtraForce(Vector3 force, float inputStopDuration = 0.0f)
    {
        rb.AddForce(-rb.velocity, ForceMode.VelocityChange);    // cancel current velocity
        rb.AddForce(force, ForceMode.VelocityChange);           // applyforce

        if (extraForceCr != null) // new movement stop timer
        {
            StopCoroutine(extraForceCr);
            inputStopped = false;
        }

        if (inputStopDuration != 0.0f) // start new extra force timer (if duration is not 0)
        {
            extraForceCr = StartCoroutine(InputStopTimer(inputStopDuration));
        }
    }

    private IEnumerator InputStopTimer(float duration)
    {
        float passed = 0.0f;
        inputStopped = true;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        inputStopped = false;
    }

    private IEnumerator DashDelayTimer()
    {
        float passed = 0.0f;
        canDash = false;

        while (passed < dashDelay)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        canDash = true;
    }
}