using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: functions as a ketchup gun, spawning in projectiles
//----------------------------------------------------------------------------------------

public class nKetchupWeapon : MonoBehaviour
{
    public GameObject projectile;                       // shot projectile
    public Transform spawnPoint;                        // projectile spawn point
    public float attackDelay = 0.5f;                    // attack delay time
    public int maxBullets = 10;                         // max amount of bullets
    public int bulletAmount = 0;                        // amount of bullets
    public GameObject reticle;                          // reticle to use
    public float reticleMaxDistance = 10.0f;            // max distance for reticles
    public LayerMask reticleCollidesWith;               // layers for reticles to collide with
    public float attackSpeed = 1.0f;                    

    private GameObject player;
    private bool inAttack = false;

    private void Start()
    {
        if (reticle == null)
        {
            reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Destroy(reticle.GetComponent<Collider>());
        }
        else
        {
            reticle = Instantiate(reticle);
        }

        reticle.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0) && !inAttack)
        {
            Attack();
        }

        if (Input.GetKey(KeyCode.Mouse1) && (!player.GetComponent<nPlayerMovement>().currentlyGliding() && !reticle.activeSelf))
        {
            player.GetComponent<nPlayerMovement>().setAiming(true);
            CameraTarget.instance.offsetTo(new Vector3(2, 2, 0), 0.25f); // offset camera target
            reticleCollision();
            reticle.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) || (player.GetComponent<nPlayerMovement>().currentlyGliding() && reticle.activeSelf))
        {
            player.GetComponent<nPlayerMovement>().setAiming(false);
            CameraTarget.instance.returnDefault(0.25f); // return camera target to default
            reticle.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            reticleCollision();
        }
    }

    // checks for reticle collisions
    private void reticleCollision()
    {
        float distance = reticleMaxDistance;

        Debug.DrawLine(spawnPoint.position, spawnPoint.position + (Camera.main.transform.forward * reticleMaxDistance), Color.green);

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, Camera.main.transform.forward, out hit, reticleMaxDistance, reticleCollidesWith, QueryTriggerInteraction.Ignore))
        {
            distance = hit.distance;
        }

        reticle.transform.position = spawnPoint.position + (Camera.main.transform.forward * distance);
        reticle.transform.rotation = Camera.main.transform.rotation;
    }

    // does attack
    private void Attack()
    {
        if (bulletAmount != 0)
        {
            player.GetComponent<Animator>().SetTrigger("KetchupAttack_01"); // play visual attack animation
            GetComponentInParent<nPlayerManager>().enableWeaponSwitch(false);  // stop weapon switch
            //GetComponentInParent<nPlayerMovement>().stopInput(attackDelay + 0.1f);            // stop player for a bit
        }
    }

    private void OnEnable()
    {
        player = transform.root.gameObject;
        player.GetComponent<nPlayerManager>().playerEvent += eventHandle;
        AudioManager.Instance.playRandom(transform.position, "Ketchup_Reload_01"); //Sound for switch to Ketchup Weapon -Brian
        player.GetComponent<nPlayerAnimations>().setOverallAnim("KetchupAnim");    // turn off basic animations
        player.GetComponent<nPlayerAnimations>().setIdleAnim("KetchupIdle");       // set idle animation
        player.GetComponent<nPlayerAnimations>().setRunAnim("KetchupRun");         // set run animation
        player.GetComponent<nPlayerAnimations>().setJumpAnim("KetchupJump");       // set jump animation
        nUIManager.instance.setWeaponUseUI(1, bulletAmount, true);
    }

    private void OnDisable()
    {
        if (player.GetComponent<nPlayerManager>() != null)
        {
            player.GetComponent<nPlayerManager>().playerEvent -= eventHandle;
        }
        reticle.SetActive(false);
        CameraTarget.instance.returnDefault(0.25f);                 // return camera target to default
        player.GetComponent<nPlayerMovement>()?.setAiming(false); // turn off aiming for player
        player.GetComponent<nPlayerAnimations>()?.setBasicAnim();   // revert to basic animations
        nUIManager.instance.setWeaponUseUI(1, 0, false);
        inAttack = false;
    }

    // respond to events
    public void eventHandle(string message)
    {
        switch (message)
        {
            case "spawnBullet": // only one event for now
                AudioManager.Instance.playRandom(transform.position, "Ketchup_Fire_01"); // play audio clip, added sound -Brian 
                if (reticle.activeSelf)
                {
                    Instantiate(projectile, spawnPoint.position, Camera.main.transform.rotation);
                }
                else
                {
                    Instantiate(projectile, spawnPoint.position, transform.rotation);
                }
                bulletAmount--;
                nUIManager.instance.setWeaponUseUI(1, bulletAmount, true);
                break;
            case "inAttack":
                inAttack = true;
                player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", attackSpeed);
                //print("inAttack");
                break;
            case "outAttack":
                inAttack = false;
                player.GetComponent<Animator>().SetFloat("KetchupAttackSpeed", 1.0f);
                //print("no longer in Attack");
                GetComponentInParent<nPlayerManager>().enableWeaponSwitch(true); // enable weapon switch
                if (bulletAmount == 0)
                {
                    player.GetComponent<nPlayerInventory>().disableCurrentWeapon();
                }
                break;
            case "MovementInterruption":
                inAttack = false;
                GetComponentInParent<nPlayerManager>().enableWeaponSwitch(true); // enable weapon switch
                if (bulletAmount == 0)
                {
                    player.GetComponent<nPlayerInventory>().disableCurrentWeapon();
                }
                break;
            default:
                break;
        }
    }

    // returns true if gun can add more bullets
    public bool reload()
    {
        if (maxBullets != bulletAmount)
        {
            bulletAmount = maxBullets;
            nUIManager.instance.setWeaponUseUI(1, bulletAmount, gameObject.activeSelf);
            return true;
        }

        return false;
    }
}