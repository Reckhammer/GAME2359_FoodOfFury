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
    public Animator weaponComponentAnimator;
    public CapsuleCollider triggerCollider;
    public float attackOneSpeed         = 1.0f;
    public float attackOneFollowUpSpeed = 1.0f;
    public float attackTwoSpeed         = 1.0f;
    public float fallingAttackSpeed     = 1.0f;

    private GameObject player;
    private bool attackOneFollowup = false;
    private bool inAttack = false;
    private string attackName;

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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !inAttack && !GetComponentInParent<nPlayerMovement>().onGround() && !GetComponentInParent<nPlayerMovement>().currentlyGliding())
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
        inAttack = true;
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");                                // play audio clip 
        GetComponentInParent<nPlayerManager>().enableWeaponSwitch(false);                                       // stop weapon switch
        player.GetComponent<Animator>().SetTrigger("GreenOnion_Attack_01");                                     // play visual attack animation
        player.GetComponent<Animator>().SetBool("GreenOnion_Attack_FollowUp", false);                           // set GreenOnion_Attack01_FollowUp false
        attackName = "GreenOnion_Attack_01";                                                                    // set animation to be played
        player.GetComponent<Animator>().SetFloat("GreenOnion_Attack_01_Speed", attackOneSpeed);                 // set animation speed
        weaponComponentAnimator.SetFloat("GreenOnion_Attack_01_Speed", attackOneSpeed);
        player.GetComponent<nPlayerMovement>().stopPlayerVelocity();
        player.GetComponent<nPlayerMovement>().stopInput(true);
        player.GetComponent<nPlayerMovement>().stopRotation(true);

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;     // cam forward without y value
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;         // cam right without y value
        Vector3 direction = Input.GetAxisRaw("Vertical") * camForward + Input.GetAxisRaw("Horizontal") * camRight;    // calculate direction
        if (direction != Vector3.zero)
        {
            player.GetComponent<nPlayerMovement>().applyExtraForce(direction * 20.0f, 0.25f);
            player.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            player.GetComponent<nPlayerMovement>().applyExtraForce(transform.forward * 20.0f, 0.25f);
        }
    }

    void FollowUpAttack()
    {
        inAttack = true;
        attackOneFollowup = false;
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");                                // play audio clip
        GetComponentInParent<nPlayerManager>().enableWeaponSwitch(false);                                       // stop weapon switch
        player.GetComponent<Animator>().SetBool("GreenOnion_Attack_FollowUp", true);                            // play visual attack animation
        attackName = "GreenOnion_Attack_FollowUp";                                                              // set animation to be played
        player.GetComponent<Animator>().SetFloat("GreenOnion_Attack_FollowUp_Speed", attackOneFollowUpSpeed);   // set animation speed
        weaponComponentAnimator.SetFloat("GreenOnion_Attack_FollowUp_Speed", attackOneFollowUpSpeed);
        player.GetComponent<nPlayerMovement>().stopPlayerVelocity();
        player.GetComponent<nPlayerMovement>().stopInput(true);
        player.GetComponent<nPlayerMovement>().stopRotation(true);

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;     // cam forward without y value
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;         // cam right without y value
        Vector3 direction = Input.GetAxisRaw("Vertical") * camForward + Input.GetAxisRaw("Horizontal") * camRight;    // calculate direction
        if (direction != Vector3.zero)
        {
            player.GetComponent<nPlayerMovement>().applyExtraForce(direction * 20.0f, 0.25f);
            player.transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            player.GetComponent<nPlayerMovement>().applyExtraForce(transform.forward * 20.0f, 0.25f);
        }
    }

    void AttackTwo()
    {
        inAttack = true;
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");                                // play audio clip 
        GetComponentInParent<nPlayerManager>().enableWeaponSwitch(false);                                       // stop weapon switch
        player.GetComponent<nPlayerMovement>().stopInput(true);                                                 // stop player input
        player.GetComponent<nPlayerMovement>().stopRotation(true);                                              // stop player rotation
        player.GetComponent<nPlayerMovement>().stopPlayerVelocity();                                            // stop player velocity
        player.GetComponent<Animator>().SetTrigger("GreenOnion_Attack_02");                                     // play visual attack animation
        attackName = "GreenOnion_Attack_02";                                                                    // set animation to be played
        player.GetComponent<Animator>().SetFloat("GreenOnion_Attack_02_Speed", attackTwoSpeed);                 // set animation speed
        weaponComponentAnimator.SetFloat("GreenOnion_Attack_02_Speed", attackTwoSpeed);
    }

    void FallingAttack()
    {
        inAttack = true;
        AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");                                // play audio clip 
        GetComponentInParent<nPlayerManager>().enableWeaponSwitch(false);                                       // stop weapon switch
        player.GetComponent<Animator>().SetTrigger("GreenOnion_FallingAttack");                                 // play visual attack animation
        attackName = "GreenOnion_FallingAttack";                                                                // set animation to be played
        player.GetComponent<Animator>().SetFloat("GreenOnion_FallingAttack_Speed", fallingAttackSpeed);         // set animation speed
        weaponComponentAnimator.SetFloat("GreenOnion_FallingAttack_Speed", fallingAttackSpeed);
    }

    private void OnEnable()
    {
        player = transform.root.gameObject;
        AudioManager.Instance.playRandom(transform.position, "Sword_Draw_01");  // Sound for when sword is drawn -Brian
        player.GetComponent<nPlayerAnimations>().setOverallAnim("OnionAnim");     // turn off basic animations
        player.GetComponent<nPlayerAnimations>().setIdleAnim("OnionIdle");        // set idle animation
        player.GetComponent<nPlayerAnimations>().setRunAnim("OnionRun");          // set run animation
        player.GetComponent<nPlayerAnimations>().setJumpAnim("OnionJump");        // set jump animation
        player.GetComponent<nPlayerAnimations>().setEvadeRightAnim("EvadeRight");
        player.GetComponent<nPlayerAnimations>().setEvadeLeftAnim("EvadeLeft");
        player.GetComponent<nPlayerManager>().playerEvent += eventHandle;
    }

    private void OnDisable()
    {
        player.GetComponent<nPlayerAnimations>()?.setBasicAnim(); // revert to basic animations

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
                weaponComponentAnimator.SetTrigger(attackName); // play component animation
                //player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", attackSpeed);
                //print("inAttack");
                break;
            case "outAttack":
                inAttack = false;
                attackOneFollowup = false;
                //player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", 1.0f);
                //print("no longer in Attack");
                player.GetComponent<nPlayerMovement>().stopInput(false);
                GetComponentInParent<nPlayerManager>().enableWeaponSwitch(true);
                player.GetComponent<nPlayerMovement>().stopRotation(false);
                break;
            case "MovementInterruption":
                inAttack = false;
                attackOneFollowup = false;
                triggerCollider.enabled = false;
                weaponComponentAnimator.SetTrigger("Iterruption");
                GetComponentInParent<nPlayerManager>().enableWeaponSwitch(true); // enable weapon switch
                player.GetComponent<nPlayerMovement>().stopRotation(false);
                player.GetComponent<nPlayerMovement>().stopInput(false);
                break;
            default:
                break;
        }
    }
}