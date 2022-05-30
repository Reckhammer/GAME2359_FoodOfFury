using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nPlayerAnimations : MonoBehaviour
{
    private Animator animator = null;                   // reference to animator
    private nPlayerMovement playerMovement;             // reference to player movement script
    private Rigidbody rb;                               // reference to rigidbody
    private string overalAnim = null;                   // name of overall animation
    private string idleAnim = null;                     // name of idle animation
    private string runAnim = null;                      // name of run animation
    private string jumpAnim = null;                     // name of jump animation
    private string evadeLeftAnim = null;                // name of left evade animation
    private string evadeRightAnim = null;               // name of right evade animation

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<nPlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (playerMovement.currentlyGliding())
        {
            playGlide();
        }
        else
        {
            playGlide(false);

            if (rb.velocity.y < 0 && !playerMovement.onGround())
            {
                playFall();
            }
            else
            {
                playFall(false);
            }
        }

        if (playerMovement.getMovement().magnitude > 0 && playerMovement.onGround()) // running
        {
            playIdle(false);
            playRun();
        }
        else if (playerMovement.onGround()) // idle
        {
            playIdle();
            playRun(false);
        }
        else // non-grounded
        {
            playIdle(false);
            playRun(false);
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

        animator.SetTrigger("Restart");
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

    public void playJump()
    {
        animator.SetTrigger(jumpAnim);
    }

    public void playRun(bool play = true)
    {
        animator.SetBool(runAnim, play);
    }

    public void playIdle(bool play = true)
    {
        animator.SetBool(idleAnim, play);
    }

    public void playFall(bool play = true)
    {
        animator.SetBool("isFalling", play);
    }

    public void playGlide(bool play = true)
    {
        animator.SetBool("isGliding", play);
    }
}