using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV, Jose Villanueva
 * 
 *  Description: This class handles the player's melee weapons.
 *  
 *  */

public class OnionWeapon : MonoBehaviour
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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
          
            if ((attackOnePlayed == false && !followUp) || !attackAnim.isPlaying)
            {
                AttackOne();
       
                if(!attackAnim.isPlaying)
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

        if(followUp && attackAnim.isPlaying)
        {
            FollowUpAttack();
        }

        if (Input.GetKey(KeyCode.Mouse1) && !followUp)
        {
            AttackTwo();
        }
    }

    void AttackOne()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<PlayerMovementTwo>().onGround())
        {
            attackOnePlayed = true;

            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<PlayerManager>().addSwitchDelay(0.7f);
            //GetComponentInParent<PlayerMovementTwo>().stopInput(0.7f);      // stop player for a bit
            GetComponentInParent<Animator>().SetTrigger("OnionAttack_01");  // play visual attack animation
            GetComponentInParent<Animator>().SetBool("Attack01_Followup", false); // Set Attack01_Followup false
            attackAnim.Play("GreenOnion_Attack_01");                        // collider animation
        }
    }

    void FollowUpAttack()
    {

        if (!attackAnim.isPlaying && GetComponentInParent<PlayerMovementTwo>().onGround())
        {
            print("Attack played");

            attackOnePlayed = false;

            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<PlayerManager>().addSwitchDelay(0.7f);
            //GetComponentInParent<Animator>().SetBool("Attack01_Followup", true);              // play visual attack animation
            //GetComponentInParent<PlayerMovementTwo>().stopInput(0.7f);      // stop player for a bit
            attackAnim.Play("GreenOnion_Attack_FollowUp");                        // collider animation
            followUp = false;
        }
    }


    void AttackTwo()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<PlayerMovementTwo>().onGround())
        {
            AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 

            GetComponentInParent<PlayerManager>().addSwitchDelay(1.11f);
            GetComponentInParent<PlayerMovementTwo>().stopInput(1.11f, true, true);     // stop player for a bit
            GetComponentInParent<Animator>().SetTrigger("OnionAttack_02");  // play visual attack animation
            attackAnim.Play("GreenOnion_Attack_02");                        // collider animation
        }
    }

    private void OnEnable()
    {
        GetComponentInParent<PlayerMovementTwo>().setOverallAnim("OnionAnim");  // turn off basic animations
        GetComponentInParent<PlayerMovementTwo>().setIdleAnim("OnionIdle");     // set idle animation
        GetComponentInParent<PlayerMovementTwo>().setRunAnim("OnionRun");       // set run animation
        GetComponentInParent<PlayerMovementTwo>().setJumpAnim("OnionJump");     // set jump animation
    }

    private void OnDisable()
    {
        GetComponentInParent<Animator>()?.SetTrigger("Restart");
        GetComponentInParent<PlayerMovementTwo>()?.setBasicAnim(); // revert to basic animations
    }
}
