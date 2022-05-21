using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Handles logic for onion weapon
//----------------------------------------------------------------------------------------

public class nOnionWeapon : MonoBehaviour
{
    public Animation triggerAnimation;

    private GameObject player;
    private bool attackOneFollowup = false;
    private bool inAttack = false;

    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && GetComponentInParent<nPlayerMovement>().onGround())
        {
            if (!attackOneFollowup && !inAttack)
            {
                AttackOne();
                attackOneFollowup = true;
            }
            else if(attackOneFollowup && inAttack)
            {
                FollowUpAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !GetComponentInParent<nPlayerMovement>().onGround() && !GetComponentInParent<nPlayerMovement>().isGliding)
        {
            FallingAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && !inAttack && GetComponentInParent<nPlayerMovement>().onGround())
        {
            AttackTwo();
        }
    }

    void AttackOne()
    {
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");    // play audio clip 
        GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);                // add delay to weapon switch
        //GetComponentInParent<nPlayerMovement>().stopInput(0.7f);                  // stop player for a bit
        GetComponentInParent<Animator>().SetTrigger("OnionAttack_01");              // play visual attack animation
        GetComponentInParent<Animator>().SetBool("Attack01_Followup", false);       // Set Attack01_Followup false
        triggerAnimation.Play("GreenOnion_Attack_01");                              // trigger animation
    }

    void FollowUpAttack()
    {
        attackOneFollowup = false;
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");    // play audio clip 
        GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);                // add delay to weapon switch
        GetComponentInParent<Animator>().SetBool("Attack01_Followup", true);        // play visual attack animation
        //GetComponentInParent<nPlayerMovement>().stopInput(0.7f);                  // stop player for a bit
        triggerAnimation.Play("GreenOnion_Attack_FollowUp");                        // trigger animation
    }

    void AttackTwo()
    {
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");    // play audio clip 
        GetComponentInParent<nPlayerManager>().addSwitchDelay(1.11f);               // add delay to weapon switch
        GetComponentInParent<nPlayerMovement>().stopInput(1.11f, true, true);       // stop player for a bit
        GetComponentInParent<Animator>().SetTrigger("OnionAttack_02");              // play visual attack animation
        triggerAnimation.Play("GreenOnion_Attack_02");                              // trigger animation
    }

    void FallingAttack()
    {
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");    // play audio clip 
        GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);                // add delay to weapon switch
        GetComponentInParent<Animator>().SetTrigger("MidairAttack");                // play visual attack animation
        triggerAnimation.Play("GreenOnion_FallingAttack");                          // trigger animation
    }

    private void OnEnable()
    {
        player = transform.root.gameObject;
        AudioManager.Instance.playRandom(transform.position, "Sword_Draw_01");      // Sound for when sword is drawn -Brian
        GetComponentInParent<nPlayerMovement>().setOverallAnim("OnionAnim");        // turn off basic animations
        GetComponentInParent<nPlayerMovement>().setIdleAnim("OnionIdle");           // set idle animation
        GetComponentInParent<nPlayerMovement>().setRunAnim("OnionRun");             // set run animation
        GetComponentInParent<nPlayerMovement>().setJumpAnim("OnionJump");           // set jump animation
        GetComponentInParent<nPlayerMovement>().setEvadeRightAnim("EvadeRight");
        GetComponentInParent<nPlayerMovement>().setEvadeLeftAnim("EvadeLeft");
        player.GetComponent<nPlayerManager>().playerEvent += eventHandle;
    }

    private void OnDisable()
    {
        GetComponentInParent<Animator>()?.SetTrigger("Restart");
        GetComponentInParent<nPlayerMovement>()?.setBasicAnim(); // revert to basic animations

        if (player.GetComponent<nPlayerManager>() != null)
        {
            player.GetComponent<nPlayerManager>().playerEvent -= eventHandle;
        }
    }

    // respond to events
    public void eventHandle(string message)
    {
        switch (message)
        {
            case "inAttack":
                inAttack = true;
                //player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", attackSpeed);
                //print("inAttack");
                break;
            case "outAttack":
                inAttack = false;
                //player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", 1.0f);
                attackOneFollowup = false;
                //print("no longer in Attack");
                break;
            case "MovementInterruption":
                inAttack = false;
                break;
            default:
                break;
        }
    }
}
