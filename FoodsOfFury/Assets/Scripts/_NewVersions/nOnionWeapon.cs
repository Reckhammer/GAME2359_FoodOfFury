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
    public Animation attackAnim;
    private bool attackOnePlayed = false;
    private bool followUp = false;

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && GetComponentInParent<nPlayerMovement>().onGround())
        {

            if ((attackOnePlayed == false && !followUp) || !attackAnim.isPlaying)
            {
                AttackOne();

                if (!attackAnim.isPlaying)
                {
                    followUp = false;
                    GetComponentInParent<Animator>().SetBool("Attack01_Followup", false);
                }
            }
            else
            {
                followUp = true;
                GetComponentInParent<Animator>().SetBool("Attack01_Followup", true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && GetComponentInParent<nPlayerMovement>().onGround() == false && GetComponentInParent<nPlayerMovement>().isGliding == false)
        {
            FallingAttack();
        }

        if (followUp && attackAnim.isPlaying)
        {
            FollowUpAttack();
        }

        if (Input.GetKey(KeyCode.Mouse1))// && !followUp
        {
            AttackTwo();
        }
    }

    void AttackOne()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<nPlayerMovement>().onGround())
        {
            attackOnePlayed = true;

            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);
            //GetComponentInParent<nPlayerMovement>().stopInput(0.7f);      // stop player for a bit
            GetComponentInParent<Animator>().SetTrigger("OnionAttack_01");  // play visual attack animation
            GetComponentInParent<Animator>().SetBool("Attack01_Followup", false); // Set Attack01_Followup false
            attackAnim.Play("GreenOnion_Attack_01");                        // collider animation
        }
    }

    void FollowUpAttack()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<nPlayerMovement>().onGround())
        {
            print("Attack played");

            attackOnePlayed = false;

            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);
            //GetComponentInParent<Animator>().SetBool("Attack01_Followup", true);              // play visual attack animation
            //GetComponentInParent<nPlayerMovement>().stopInput(0.7f);      // stop player for a bit
            attackAnim.Play("GreenOnion_Attack_FollowUp");                        // collider animation
            followUp = false;
        }
    }

    void FallingAttack()
    {
        if (!attackAnim.isPlaying)// && GetComponentInParent<nPlayerMovement>().onGround()
        {

            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<nPlayerManager>().addSwitchDelay(0.7f);
            //GetComponentInParent<nPlayerMovement>().stopInput(0.7f);        // stop player for a bit
            GetComponentInParent<Animator>().SetTrigger("MidairAttack");        // play visual attack animation
            attackAnim.Play("GreenOnion_FallingAttack");                        // collider animation
        }
    }

    void AttackTwo()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<nPlayerMovement>().onGround())
        {
            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<nPlayerManager>().addSwitchDelay(1.11f);
            GetComponentInParent<nPlayerMovement>().stopInput(1.11f, true, true);     // stop player for a bit
            GetComponentInParent<Animator>().SetTrigger("OnionAttack_02");  // play visual attack animation
            attackAnim.Play("GreenOnion_Attack_02");                        // collider animation
        }
    }

    private void OnEnable()
    {
        AudioManager.Instance.playRandom(transform.position, "Sword_Draw_01"); //Sound for when sword is drawn -Brian
        GetComponentInParent<nPlayerMovement>().setOverallAnim("OnionAnim");  // turn off basic animations
        GetComponentInParent<nPlayerMovement>().setIdleAnim("OnionIdle");     // set idle animation
        GetComponentInParent<nPlayerMovement>().setRunAnim("OnionRun");       // set run animation
        GetComponentInParent<nPlayerMovement>().setJumpAnim("OnionJump");     // set jump animation
        GetComponentInParent<nPlayerMovement>().setEvadeRightAnim("EvadeRight");
        GetComponentInParent<nPlayerMovement>().setEvadeLeftAnim("EvadeLeft");
    }

    private void OnDisable()
    {
        GetComponentInParent<Animator>()?.SetTrigger("Restart");
        GetComponentInParent<nPlayerMovement>()?.setBasicAnim(); // revert to basic animations
    }
}
