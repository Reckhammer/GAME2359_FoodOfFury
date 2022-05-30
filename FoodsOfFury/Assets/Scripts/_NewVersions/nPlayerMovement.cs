using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV, Jose Villanueva
//
// Description: This class handles the players movement
//----------------------------------------------------------------------------------------

public class nPlayerMovement : MonoBehaviour
{
    public float speed = 10f;                           // speed if player
    public float glideSpeed = 7.0f;                     // movement speed while gliding
    public float jumpHeight = 10.0f;                     // jump force of player
    public float dashForce = 20.0f;                      // force of dash
    public float dashDelay = 1.0f;                      // time before dash can be used again
    public float rotationSpeed = 10.0f;                 // speed of rotation
    public LayerMask ground;                            // layers to check if grounded
    public float extraGravityMultiplier = 2.0f;         // extra gravity multiplier

    [Range(1.0f, 0.0f)]
    public float glideFallRate = 0.9f;                  // falling rate for gliding (how much gravity)

    [Range(0.0f, 1.0f)]
    public float maxSlope = 0.2f;                       // max slope player can move on

    //public GameObject leftEvadeParticles;             // particles for evading left
    //public GameObject rightEvadeParticles;            // particles for evading right

    public float slideForce = 5.0f;                     // downwards force when sliding
    public float slopeDetectDistance = 2.5f;            // distance to detect slopes

    private nHealth health = null;                      // health reference
    private Rigidbody rb = null;                        // rigidbody of player
    private bool inputStopped = false;                  // for stopping input
    private bool rotationStopped = false;               // for stopping rotation
    private Transform groundChecker = null;             // position of groundchecker
    private Vector3 inputs = Vector3.zero;              // inputs from player
    private Vector3 movement = Vector3.zero;            // calculated velocity to move the character
    private const int maxJump = 2;                      // max amount of jumps
    private int currentJump = 0;                        // current jump index
    private bool isGrounded = true;                     // for ground check
    private bool isGliding = false;                     // for gliding check
    private bool inJump = false;                        // for jump delay
    private bool canDash = true;                        // for dash delay check
    private float extraForceTime = 0.0f;                // time to allow extra force to be applied
    private Animator animator = null;                   // reference to animator
    private string overalAnim = null;                   // name of overall animation
    private string idleAnim = null;                     // name of idle animation
    private string runAnim = null;                      // name of run animation
    private string jumpAnim = null;                     // name of jump animation
    private string evadeRightAnim = null;               // name of right evade animation
    private string evadeLeftAnim = null;                // name of left evade animation
    private Vector3 extraVel = Vector3.zero;            // extra force velocity (recorded to lerp from extra to movement)
    private bool isAiming = false;                      // to change camera rotation style
    private bool onMaxSlope = false;                    // for max slope check
    private bool touchingSlope = false;                 // for touching slope check
    private bool isSliding = false;                     // for slide check
    private Vector3 slopeDownDirection = Vector3.zero;  // downwards direction of steep slope
    private RaycastHit slopeHit;                        // raycast hit info of slope

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<nHealth>();
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

                if (isGrounded && !inJump) // grounded movement (we mess with y values (slopes))
                {
                    wantVel = movement - rb.velocity;
                }
                else // air movement/flat ground movement (leave gravity alone (when on))
                {
                    movement.y = 0.0f; // remove y
                    wantVel = movement - Vector3.Scale(rb.velocity, new Vector3(1, 0, 1));
                }

                // slide down slope
                if (!inJump && onMaxSlope)
                {
                    //print("sliding");
                    wantVel = slopeDownDirection * slideForce;
                    isSliding = true;
                    rb.AddForce(wantVel, ForceMode.VelocityChange);
                    // Why does it not go directly downwards (because current velocity is not cancelled out?)
                }
                else
                {
                    isSliding = false;
                    //print("not sliding");
                    rb.AddForce(wantVel, ForceMode.VelocityChange);
                }

            }
            else // extra force calculations.
            {
                if (isGrounded) // grounded movement
                {
                    Vector3 lVel = Vector3.Lerp(movement - rb.velocity, extraVel - rb.velocity, extraForceTime);
                    rb.AddForce(lVel, ForceMode.VelocityChange);
                }
                else // air/ flat ground movement
                {
                    movement.y = 0.0f; // remove y
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

        // when not falling, gliding, or sliding. Add "extra" gravity
        if (!isGrounded && !isGliding && !inJump && !isSliding)
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

        if (isAiming || !canDash)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized); // camera rotation
        }
        else if (dir != Vector3.zero && !rotationStopped) // if direction is zero, don't set
        {
            Quaternion toRotation = Quaternion.LookRotation(dir); // target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.fixedDeltaTime * rotationSpeed); // slerp rotation with rotation speed
        }

        extraForceTime = (extraForceTime > 0.0f) ? extraForceTime - Time.fixedDeltaTime : 0.0f; // decrease timer

        if (extraForceTime <= 0.0)
        {
            extraVel = Vector3.zero;
        }

        // clamp to ground
        if (!inJump && touchingSlope && !isSliding)
        {
            float wantedDistance = slopeHit.point.y + 2; // from slope hit.point to start of ray checks (half of character height)
            Vector3 wantedPos = new Vector3(transform.position.x, wantedDistance, transform.position.z);
            transform.position = wantedPos;
        }
    }

    //private void LateUpdate()
    //{
    //    // undo unwanted rotation (when paranted to a rotating object)
    //    if (transform.up != Vector3.up)
    //    {
    //        //print("reverting unwanted rotation");
    //        Quaternion rotation = transform.rotation;
    //        rotation.x = 0;
    //        rotation.z = 0;
    //        transform.rotation = rotation;
    //    }

    //    //print("local scale x: " + transform.localScale.x);
    //    //print("lossy scale: " + transform.lossyScale);
    //    //print("parent: " + transform.parent);

    //    // fix broken scaling (when parented to an object that breaks the scale rules and gets away with it)
    //    if (transform.parent == null && (transform.localScale.x != 1 || transform.localScale.y != 1 || transform.localScale.z != 1))
    //    {
    //        //print("fixing scale");
    //        transform.localScale = Vector3.one;
    //    }
    //}

    private void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        checkSlope(); // get slope info

        // check ground
        isGrounded = (touchingSlope && !onMaxSlope) ? true : false;

        inputs = Vector3.zero; // reset inputs

        // start glide if 'space' is pressed (when not grounded)
        if (Input.GetButton("Jump") && !isGrounded && rb.velocity.y < 0)
        {
            isGliding = true;
            GetComponent<nPlayerManager>().sendEvent("MovementInterruption");
        }

        // stop glide if 'space' is released or grounded
        if (Input.GetButtonUp("Jump") || isGrounded || rb.velocity.y > 0)
        {
            isGliding = false;
        }

        calculateMovement();                // calculate movement

        // when grounded reset current jump and turn off gravity
        if (isGrounded || isSliding)
        {
            if (inJump == false) // check if in timer (to allow player to get off ground before reseting jumps)
            {
                //if (currentJump >= 1 && rb.velocity.y < -20)
                //{
                //    AudioManager.Instance.playRandom(transform.position, "Rollo_Impact_01", "Rollo_Impact_02").transform.SetParent(transform);
                //}
                currentJump = 0;
            }
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

        // space bar makes the character jump
        if (Input.GetButtonDown("Jump") && !inputStopped && (maxJump > currentJump) && !inJump)
        {
            jump();
        }

        // do dash (change to side step)
        //if (isGrounded && canDash && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
        //{
        //    if (inputs.z != 0) // don't dash if no movement(movement != Vector3.zero)
        //    {
        //        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        //        Vector3 dash = camRight * inputs.z;
        //        applyExtraForce(dash.normalized * dashForce, 0.1f); // apply dash
        //        StartCoroutine(DashDelayTimer());                       // start dash delay timer
        //        health.giveInvincibility(1.0f);
        //        if (inputs.z > 0)
        //        {
        //            animator.SetTrigger(evadeRightAnim);
        //            rightEvadeParticles.SetActive(true);
        //        }
        //        else
        //        {
        //            animator.SetTrigger(evadeLeftAnim);
        //            leftEvadeParticles.SetActive(true);
        //        }
        //    }
        //    else
        //    {
        //        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;
        //        applyExtraForce(camRight * dashForce, 0.1f); // apply dash
        //        StartCoroutine(DashDelayTimer());                       // start dash delay timer
        //        animator.SetTrigger(evadeRightAnim);
        //        health.giveInvincibility(1.0f);
        //        rightEvadeParticles.SetActive(true);
        //    }
        //}

        // DEBUG: GODMODE kinda (delete later)
        //if (Input.GetKeyDown(KeyCode.F5))
        //{
        //    GetComponent<nHealth>().max = 1000;
        //    GetComponent<nHealth>().add(1000);
        //    UIManager.instance.setHealthBarMax(1000);
        //    UIManager.instance.updateHealthBar(1000);
        //}

        doAnimations();
    }

    // calculate movement based on camera rotation and player inputs
    private void calculateMovement()
    {
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

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized; // cam forward without y value
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;     // cam right without y value
        movement = inputs.x * camForward + inputs.z * camRight;                                             // calculate movement (velocity)
        movement = Vector3.ProjectOnPlane(movement, slopeHit.normal);                                          // project movement to ground normal
        movement = Vector3.ClampMagnitude(movement, speed); // clamp magnitiude of vector by speed (stops diagonal movement being faster than hor/ver movement)
        //Debug.DrawRay(transform.position, movement.normalized, Color.red);
    }

    private void jump()
    {
        GetComponent<nPlayerManager>().sendEvent("MovementInterruption");
        StartCoroutine(JumpDelayTimer(0.1f)); // delay jump
        if (currentJump == 0)
        {
            AudioManager.Instance.playRandom(transform.position, "Rollo_Jump_01", "Rollo_Jump_02").transform.SetParent(transform);
        }
        else
        {
            AudioManager.Instance.playRandom(transform.position, "Rollo_Jump_Double_01", "Rollo_Jump_Double_02").transform.SetParent(transform);
        }
        animator.SetTrigger(jumpAnim);
        rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) + Vector3.up * -rb.velocity.y, ForceMode.VelocityChange); // regular jump (+ y velocity canceled)
        currentJump++;
    }

    // checks for steepest slope in normal checker positions
    private void checkSlope()
    {
        float minDistance = slopeDetectDistance;
        onMaxSlope = false;
        touchingSlope = false;
        slopeHit.normal = Vector3.up;
        slopeHit.point = Vector3.zero;

        RaycastHit[] rays;
        // check if hit ground near "feet" (normals are from the capsule cast shape)
        rays = Physics.CapsuleCastAll(transform.position, transform.position, 1.0f, Vector3.down, 2.0f, ground, QueryTriggerInteraction.Ignore);

        if (rays.Length != 0)
        {
            foreach (RaycastHit hit in rays)
            {
                // do another raycast (with a slight offset) from capsule cast point to get intended slope
                float offset = 0.05f;
                Vector3 changedPosition = new Vector3(hit.point.x, 0, hit.point.z);
                changedPosition += (-(new Vector3(transform.position.x, 0, transform.position.z) - changedPosition).normalized * offset);
                changedPosition.y = transform.position.y; // start from middle of gameobject

                // Debug.DrawLine(hit.point, hit.point + (hit.normal * 1.0f), Color.green); // normal of capsulecast
                // Debug.DrawLine(changedPosition, changedPosition + (Vector3.down * slopeDetectDistance), Color.red); // normal of changed position

                RaycastHit ChangedPositionHit;
                if (Physics.Raycast(changedPosition, Vector3.down, out ChangedPositionHit, slopeDetectDistance, ground, QueryTriggerInteraction.Ignore))
                {
                    touchingSlope = true;
                    if (hit.distance < minDistance)
                    {
                        slopeHit = ChangedPositionHit;
                        minDistance = hit.distance;
                    }
                }
            }
            Debug.DrawLine(slopeHit.point, slopeHit.point + (slopeHit.normal * 1.0f), Color.cyan); // normal of closest ground normal
        }

        // if on max slope, calculate downward direction of slope
        if ((1 - maxSlope) > Vector3.Dot(Vector3.up, slopeHit.normal))
        {
            onMaxSlope = true;
            slopeDownDirection = Vector3.Cross(slopeHit.normal, -transform.up);
            slopeDownDirection = Vector3.Cross(slopeDownDirection, slopeHit.normal);
            //Debug.DrawRay(transform.position, slopeDownDirection, Color.cyan); // draw slope affected movement line
            //print("slopeDownDirection: " + slopeDownDirection);
        }
    }

    // applies extra force to gameobject
    public void applyExtraForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.VelocityChange); // applyforce
        extraVel = force;
        extraForceTime = 1.0f; // since we do a velocity change, time to complete extra force is roughly 1 second
    }

    public void resetJump()
    {
        currentJump = 0;
    }

    public void stopPlayerVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void stopInput(bool stopInput)
    {
        inputStopped = stopInput;
    }

    public void stopRotation(bool stopRotation)
    {
        rotationStopped = stopRotation;
    }

    public bool onGround()
    {
        return isGrounded;
    }

    public bool currentlyGliding()
    {
        return isGliding;
    }

    // timer for dash delay (needs to change into dodge timer)
    //private IEnumerator DashDelayTimer()
    //{
    //    float passed = 0.0f;
    //    canDash = false;

    //    while (passed < dashDelay)
    //    {
    //        passed += Time.deltaTime;
    //        yield return null;
    //    }

    //    if (leftEvadeParticles.activeSelf)
    //    {
    //        leftEvadeParticles.SetActive(false);
    //    }
    //    else if (rightEvadeParticles.activeSelf)
    //    {
    //        rightEvadeParticles.SetActive(false);
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

        if (movement.magnitude > 0 && isGrounded) // running
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
        else if (isGrounded) // idle
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
        else // non-grounded
        {
            if (idleAnim != null)
            {
                animator.SetBool(idleAnim, false);
            }

            if (runAnim != null)
            {
                animator.SetBool(runAnim, false);
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

    // set jump animation to be used
    public void setJumpAnim(string anim)
    {
        jumpAnim = anim;
    }

    // set right evade animation to be used
    public void setEvadeRightAnim(string anim)
    {
        evadeRightAnim = anim;
    }

    // set right evade animation to be used
    public void setEvadeLeftAnim(string anim)
    {
        evadeLeftAnim = anim;
    }

    // set aiming
    public void setAiming(bool aim)
    {
        isAiming = aim;
    }

    // returns if inputs stopped
    public bool isInputStopped()
    {
        return inputStopped;
    }

    // reverts to basic animation (set old animations to false)
    public void setBasicAnim()
    {
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

    // changes overall animation set
    public void setOverallAnim(string anim)
    {
        if (overalAnim != null)
        {
            animator.SetBool(overalAnim, false);    // turn off prevous animation set
        }

        overalAnim = anim;                      // set new animation set
        animator.SetBool(overalAnim, true);     // turn on new animation set
    }
}