using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // get rid of this later

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV, Jose Villanueva
//
// Description: This class handles the players movement
//----------------------------------------------------------------------------------------

public class PlayerMovement : MonoBehaviour
{
    public float speed                  = 5f;           // speed if player
    public float glideSpeed             = 1.0f;         // movement speed while gliding
    public float jumpHeight             = 2f;           // jump force of player
    public float dashForce              = 1.0f;         // force of dash
    public float dashDelay              = 0.5f;         // time before dash can be used again
    public float groundDistance         = 0.2f;         // distance to check for ground
    //public float fallForce              = 1.0f;         // downwards force when falling
    public float rotationSpeed          = 1.0f;         // speed of rotation
    public LayerMask ground;                            // layers to check if grounded

    [Range(0.0f, 1.0f)]
    public float airControll            = 0.5f;         // amount of controll while in air

    [Range(1.0f, 0.0f)]
    public float glideFallRate          = 0.9f;         // falling rate for gliding (how much gravity)

    [Range(0.0f, 1.0f)]
    public float slowDownRate           = 0.1f;         // rate to slow down player when not moving

    private Rigidbody rb                = null;         // rigidbody of player
    private bool inputStopped           = false;        // for stopping input
    private Transform groundChecker     = null;         // position of groundchecker
    private Vector3 inputs              = Vector3.zero; // inputs from player
    private Vector3 movement            = Vector3.zero; // calculated velocity to move the character
    private const int maxJump           = 2;            // max amount of jumps
    private int currentJump             = 0;            // current jump index
    private bool isGrounded             = true;         // for ground check
    private bool isGliding              = false;        // for gliding check
    private bool canJump                = true;         // for jump delay
    private bool canDash                = true;         // for dash delay check
    private Vector3 groundNormal        = Vector3.up;   // normal of the ground
    private Coroutine inputStoppedCr    = null;         // reference to input stop timer coroutine
    private float extraForceTime        = 0.0f;         // time to allow extra force to be applied

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(1);
    }

    private void FixedUpdate()
    {
        if (!inputStopped)
        {
            //rb.AddForce(movement); // apply movement
            rb.AddForce(movement - Vector3.Scale(rb.velocity, new Vector3(1, 0, 1)));

            if (isGliding)
            {
                rb.AddForce(-Physics.gravity * glideFallRate);
            }

            // slow player down when on ground, no input and not in extra force
            if (isGrounded && movement == Vector3.zero && extraForceTime == 0.0)
            {
                if (rb.velocity.magnitude > 0.1f) // stop calculating when not needed (already slowed down)
                {
                    Vector3 velocityChange = Vector3.Scale(rb.velocity, new Vector3(1, 1, 1) * -slowDownRate); // velocityChange = opposite velocity * rate
                    rb.AddForce(velocityChange, ForceMode.VelocityChange);
                }
                else
                {
                    rb.velocity = Vector3.zero; // stop player
                }
            }
            else if (extraForceTime == 0.0 && rb.velocity.magnitude > speed)  // slow down if grounded, not in extra force and going faster than speed
            {
                Vector3 velocityChange = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));  // get velocity without y (leave gravity alone)
                velocityChange -= velocityChange.normalized * speed;                        // calculate difference between velocity and max velocity
                rb.AddForce(-velocityChange, ForceMode.VelocityChange);                     // apply difference to return to normal (unless it was gravity)
            }
            //else if (isGrounded && extraForceTime == 0.0f && rb.velocity.magnitude < speed) // help player reach max speed
            //{
            //    Vector3 velocityChange = rb.velocity;                                       // get velocity
            //    velocityChange -= movement.normalized * speed;                              // calculate difference between velocity and max velocity (using movement direction for quick turn speeds)
            //    rb.AddForce(-velocityChange);                                               // apply difference to try to reach max speed
            //}
        }

        Vector3 dir = Vector3.zero;

        if (isGrounded && extraForceTime == 0.0f) // do input (movement) + velocity affected rotation
        {
            dir = Vector3.Lerp(rb.velocity, movement, 0.5f);            // meet in middle
            dir = Vector3.Scale(dir, new Vector3(1, 0, 1)).normalized;  // normalize (no y value)
        }
        else // do velocity affected rotation
        {
            dir = Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));     // regular velocity normalized (no y value)
        }

        if (dir != Vector3.zero) // direction is zero, don't set
        {
            Quaternion toRotation = Quaternion.LookRotation(dir); // target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * rotationSpeed); // slerp rotation with rotation speed
        }

        extraForceTime = (extraForceTime > 0.0f) ? extraForceTime - Time.fixedDeltaTime : 0.0f; // decrease timer
    }

    private void Update()
    {
        // check ground
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, ground, QueryTriggerInteraction.Ignore);
        inputs = Vector3.zero; // reset inputs

        // start glide if 'x' is pressed (when not grounded)
        if (Input.GetKey(KeyCode.LeftShift) && !isGrounded && rb.velocity.y < 0)
        {
            isGliding = true;
        }

        // stop glide if 'x' is released or grounded
        if (Input.GetKeyUp(KeyCode.LeftShift) || isGrounded || rb.velocity.y > 0)
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

        // when grounded reset current jump and turn off gravity
        if (isGrounded)
        {
            if (canJump == true) // check if in timer (to allow player to get off ground before reseting jumps)
            {
                currentJump = 0;
            }
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

        // space bar makes the character jump
        if (Input.GetButtonDown("Jump") && (maxJump > currentJump) && canJump)
        {
            StartCoroutine(JumpDelayTimer(0.1f)); // delay jump
            AudioManager.Instance.playRandom(transform.position, "Rollo_Jump_1", "Rollo_Jump_2").transform.SetParent(transform);
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            currentJump++;
        }

        // falling & not gliding, add downward force
        //if (!isGrounded && !isGliding && rb.velocity.y < 0)
        //{
        //    //rb.AddForce(fallForce * Vector3.down, ForceMode.Force);
        //}

        // do dash
        if (isGrounded && canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (movement.normalized != Vector3.zero) // don't dash if no movement
            {
                applyExtraForce(movement.normalized * dashForce, 0.1f); // apply dash
                StartCoroutine(DashDelayTimer());                       // start dash delay timer
            }
        }

        // DEBUG: RESET LEVEL (delete later)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        Vector3 start = groundChecker.position + (Vector3.up * 0.1f); // ground position, but a little up (to get ground that is close)
        Vector3 end = groundChecker.position + (Vector3.down * distance);
        Debug.DrawLine(start, end, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(start, Vector3.down, out hit, distance, ground, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(start, start + (hit.normal * 5.0f), Color.red); // draw normal of ground
            groundNormal = hit.normal;

            //float angle = Vector3.Angle(hit.normal, Vector3.up);
            //print("angle: " + angle);

            // flat = 0
            // steep angle = bigger

            //float angle = Vector3.Angle(hit.normal, transform.forward);
            //print("angle: " + angle);
        }
        else
        {
            groundNormal = Vector3.up; // no normal detected, set to world up
        }
    }

    // applies extra force to gameobject with an option of stopping player input for a duration
    public void applyExtraForce(Vector3 force, float inputStopDuration = 0.0f, bool resetJump = false)
    {
        //rb.AddForce(-rb.velocity, ForceMode.VelocityChange);    // cancel current velocity
        stopInput(inputStopDuration);
        rb.AddForce(force, ForceMode.VelocityChange);           // applyforce

        if (resetJump)
        {
            currentJump = 0;
        }

        extraForceTime = 1.0f; // since we do a velocity change, time to complete extra force is roughly 1 second
    }

    public void stopInput(float inputStopDuration = 0.0f, bool stopPlayer = true)
    {
        if (stopPlayer)
        {
            rb.AddForce(-rb.velocity, ForceMode.VelocityChange);    // cancel current velocity
        }

        if (inputStoppedCr != null) // new input stop timer
        {
            StopCoroutine(inputStoppedCr);
            inputStopped = false;
        }

        if (inputStopDuration != 0.0f) // start input stop timer (if duration is not 0)
        {
            inputStoppedCr = StartCoroutine(InputStopTimer(inputStopDuration));
        }
    }

    // timer for stopping input
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

    // timer for dash delay
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

    // jump delay timer
    private IEnumerator JumpDelayTimer(float duration)
    {
        float passed = 0.0f;
        canJump = false;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        canJump = true;
    }

    // returns isGrounded
    public bool onGround()
    {
        return isGrounded;
    }

    // DEBUG: draw ground checker sphere
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        { 
            return;
        }

        if (isGrounded)
        {
            Gizmos.color = Color.cyan;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }

        Gizmos.DrawSphere(groundChecker.position, groundDistance);
    }
}