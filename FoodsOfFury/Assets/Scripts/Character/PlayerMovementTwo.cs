﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // get rid of this later

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV, Jose Villanueva
//
// Description: This class handles the players movement
//----------------------------------------------------------------------------------------

public class PlayerMovementTwo : MonoBehaviour
{
    public float speed                  = 10f;          // speed if player
    public float glideSpeed             = 5.0f;         // movement speed while gliding
    public float jumpHeight             = 5.0f;         // jump force of player
    //public float dashForce              = 20.0f;        // force of dash
    //public float dashDelay              = 0.5f;         // time before dash can be used again
    public float groundDetectRadius     = 0.3f;         // radius of sphere to check for ground
    public float rotationSpeed          = 10.0f;        // speed of rotation
    public LayerMask ground;                            // layers to check if grounded
    public float extraGravityMultiplier = 1.0f;         // extra gravity multiplier

    [Range(1.0f, 0.0f)]
    public float glideFallRate          = 0.9f;         // falling rate for gliding (how much gravity)

    private Rigidbody rb                = null;         // rigidbody of player
    private bool inputStopped           = false;        // for stopping input
    private Transform groundChecker     = null;         // position of groundchecker
    private Vector3 inputs              = Vector3.zero; // inputs from player
    private Vector3 movement            = Vector3.zero; // calculated velocity to move the character
    private const int maxJump           = 2;            // max amount of jumps
    private int currentJump             = 0;            // current jump index
    private bool isGrounded             = true;         // for ground check
    private bool isGliding              = false;        // for gliding check
    private bool inJump                 = false;        // for jump delay
    //private bool canDash                = true;         // for dash delay check
    private Vector3 groundNormal        = Vector3.up;   // normal of the ground
    private Coroutine inputStoppedCr    = null;         // reference to input stop timer coroutine
    private float extraForceTime        = 0.0f;         // time to allow extra force to be applied
    private Animator animator           = null;         // reference to animator
    private string idleAnim             = null;         // name of idle animation
    private string runAnim              = null;         // name of run animation
    private Vector3 extraVel            = Vector3.zero; // extra force velocity (recorded to lerp from extra to movement)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = transform.GetChild(1);
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!inputStopped)
        {
            if (extraForceTime <= 0.0f)
            {
                Vector3 wantVel = Vector3.zero;

                if (groundNormal != Vector3.up && !inJump) // grounded movement (we mess with y values (slopes))
                {
                    wantVel = movement - rb.velocity;
                }
                else // air movement/flat ground movement (leave gravity alone (when on))
                {
                    wantVel = movement - Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));
                }

                rb.AddForce(wantVel, ForceMode.VelocityChange);
            }
            else // extra force calculations. NOTE: bounce pads/damaging needs to call PlayerMovementTwo (for now)
            {
                if (groundNormal != Vector3.up) // grounded movement
                {
                    Vector3 lVel = Vector3.Lerp(movement - rb.velocity, extraVel - rb.velocity, extraForceTime);
                    rb.AddForce(lVel, ForceMode.VelocityChange);
                }
                else // air/ flat ground movement
                {
                    Vector3 lVel = Vector3.Lerp(movement - rb.velocity, extraVel - rb.velocity, extraForceTime);
                    lVel.y = 0; // cut out y (y force was added in function. Allow gravity in extray force)
                    rb.AddForce(lVel, ForceMode.VelocityChange);
                }
            }

            if (isGliding)
            {
                rb.AddForce(-Physics.gravity * glideFallRate);
            }
        }

        // falling & not gliding, add downward force
        if (!isGrounded && !isGliding && !inJump)
        {
            Vector3 extraGrav = Physics.gravity * extraGravityMultiplier;
            rb.AddForce(extraGrav);
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

        if (dir != Vector3.zero) // if direction is zero, don't set
        {
            Quaternion toRotation = Quaternion.LookRotation(dir); // target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * rotationSpeed); // slerp rotation with rotation speed
        }

        extraForceTime = (extraForceTime > 0.0f) ? extraForceTime - Time.fixedDeltaTime : 0.0f; // decrease timer

        if (extraForceTime <= 0.0)
        {
            extraVel = Vector3.zero;
        }
    }

    private void Update()
    {
        // check ground
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDetectRadius, ground, QueryTriggerInteraction.Ignore);
        inputs = Vector3.zero; // reset inputs

        // start glide if 'shift' is pressed (when not grounded)
        if (Input.GetKey(KeyCode.LeftShift) && !isGrounded && rb.velocity.y < 0)
        {
            isGliding = true;
        }

        // stop glide if 'shift' is released or grounded
        if (Input.GetKeyUp(KeyCode.LeftShift) || isGrounded || rb.velocity.y > 0)
        {
            isGliding = false;
        }


        if (isGliding) // gliding movement
        {
            inputs.x = Input.GetAxis("Vertical") * glideSpeed;
            inputs.z = Input.GetAxis("Horizontal") * glideSpeed;
        }
        else // regular movement
        {
            inputs.x = Input.GetAxis("Vertical") * speed;
            inputs.z = Input.GetAxis("Horizontal") * speed;
        }

        checkSlope();           // get slope normal
        calculateMovement();    // calculate movement

        // when grounded reset current jump and turn off gravity
        if (isGrounded)
        {
            if (inJump == false) // check if in timer (to allow player to get off ground before reseting jumps)
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
        if (Input.GetButtonDown("Jump") && (maxJump > currentJump) && !inJump)
        {
            StartCoroutine(JumpDelayTimer(0.1f)); // delay jump
            AudioManager.Instance.playRandom(transform.position, "Rollo_Jump_1", "Rollo_Jump_2").transform.SetParent(transform);
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) + Vector3.up * -rb.velocity.y, ForceMode.VelocityChange); // regular jump (+ y velocity canceled)
            currentJump++;
        }

        // do dash
        //if (isGrounded && canDash && Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    if (movement.normalized != Vector3.zero) // don't dash if no movement
        //    {
        //        applyExtraForce(movement.normalized * dashForce, 0.1f); // apply dash
        //        StartCoroutine(DashDelayTimer());                       // start dash delay timer
        //    }
        //}

        // DEBUG: RESET LEVEL (delete later)
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        doAnimations();
    }

    // calculate movement based on camera rotation and player inputs
    private void calculateMovement()
    {
        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized; // cam forward without y value
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;     // cam right without y value
        movement = inputs.x * camForward + inputs.z * camRight;                                             // calculate movement (velocity)
        movement = Vector3.ProjectOnPlane(movement, groundNormal);                                          // project movement to ground normal
        movement = Vector3.ClampMagnitude(movement, speed); // clamp magnitiude of vector by speed (stops diagonal movement being faster than hor/ver movement)
        Debug.DrawRay(transform.position, movement.normalized, Color.red);
    }

    // sets groundNormal to the normal of the ground
    private void checkSlope(float distance = 1.0f)
    {
        Vector3 start = groundChecker.position + (Vector3.up * 0.1f); // ground position, but a little up (to get ground that is close)
        Vector3 end = groundChecker.position + (Vector3.down * distance);
        Debug.DrawLine(start, end, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(start, Vector3.down, out hit, distance, ground, QueryTriggerInteraction.Ignore) && isGrounded)
        {
            Debug.DrawLine(start, start + (hit.normal * 5.0f), Color.red); // draw normal of ground
            groundNormal = hit.normal;
        }
        else
        {
            groundNormal = Vector3.up; // no normal detected, set to world up
        }
    }

    // applies extra force to gameobject with an option of stopping player input for a duration
    public void applyExtraForce(Vector3 force, float inputStopDuration = 0.0f, bool resetJump = false)
    {
        stopInput(inputStopDuration);
        rb.AddForce(force, ForceMode.VelocityChange);           // applyforce
        extraVel = force;

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
    //private IEnumerator DashDelayTimer()
    //{
    //    float passed = 0.0f;
    //    canDash = false;

    //    while (passed < dashDelay)
    //    {
    //        passed += Time.deltaTime;
    //        yield return null;
    //    }

    //    canDash = true;
    //}

    // jump delay timer
    private IEnumerator JumpDelayTimer(float duration)
    {
        float passed = 0.0f;
        inJump = true;

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        inJump = false;
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

        Gizmos.DrawSphere(groundChecker.position, groundDetectRadius);
    }

    // do animations based on movement
    private void doAnimations()
    {
        if (isGliding)
        {
            animator.SetBool("isGliding", true);
        }
        else
        {
            animator.SetBool("isGliding", false);

            if (rb.velocity.y < 0 && !isGrounded)
            {
                animator.SetBool("isFalling", true);
            }
            else
            {
                animator.SetBool("isFalling", false);
            }
        }

        if (movement.magnitude > 0) // running
        {
            if (idleAnim != null)
            {
                animator.SetBool(idleAnim, false);
            }

            if (runAnim != null)
            {
                animator.SetBool(runAnim, true);
            }
        }
        else // idle
        {
            if (runAnim != null)
            {
                animator.SetBool(runAnim, false);
            }

            if (idleAnim != null)
            {
                animator.SetBool(idleAnim, true);
            }
        }
    }

    // set run animation to be used
    public void setIdleAnim(string anim)
    {
        idleAnim = anim;
    }

    // set idle animation to be used
    public void setRunAnim(string anim)
    {
        runAnim = anim;
    }

    // reverts to basic animation (set old animations to false)
    public void setBasicAnim(bool isBasic)
    {
        animator.SetBool("BasicAnim", isBasic);

        if (idleAnim != null)
        {
            animator.SetBool(idleAnim, false);
        }

        if (runAnim != null)
        {
            animator.SetBool(runAnim, false);
        }

        idleAnim = null;
        runAnim = null;
    }
}