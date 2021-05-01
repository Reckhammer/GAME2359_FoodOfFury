using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: functions as a ketchup gun, spawning in projectiles
//----------------------------------------------------------------------------------------
public class KetchupWeapon : MonoBehaviour
{
    public GameObject projectile;               // shot projectile
    public Transform spawnPoint;                // projectile spawn point
    public float attackDelay        = 0.5f;     // attack delay time
    public int bulletAmount         = 10;       // amount of bullets
    public GameObject reticle;                  // reticle to use
    public float reticleMaxDistance = 10.0f;    // max distance for reticles
    public LayerMask reticleCollidesWith;       // layers for reticles to collide with

    private GameObject player;

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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            player.GetComponent<PlayerMovementTwo>().setAiming(true);
            CameraTarget.instance.offsetTo(new Vector3(2, 2, 0), 0.25f); // offset camera target
            reticleCollision();
            reticle.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            player.GetComponent<PlayerMovementTwo>().setAiming(false);
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

        Debug.DrawLine(spawnPoint.position, spawnPoint.position + (transform.forward * reticleMaxDistance), Color.green);

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint.position, transform.forward, out hit, reticleMaxDistance, reticleCollidesWith, QueryTriggerInteraction.Ignore))
        {
            distance = hit.distance;
        }

        reticle.transform.position = spawnPoint.position + (transform.forward * distance);
        reticle.transform.rotation = transform.rotation;
    }
    
    // does attack
    private void Attack()
    {
        if (bulletAmount != 0)
        {
            player.GetComponent<Animator>().SetTrigger("KetchupAttack_01");   // play visual attack animation
            player.GetComponent<PlayerManager>().addSwitchDelay(attackDelay + 0.1f);
            //GetComponentInParent<PlayerMovementTwo>().stopInput(attackDelay + 0.1f);            // stop player for a bit

            if (bulletAmount == 0)
            {
                player.GetComponent<PlayerManager>().remove(ItemType.Weapon, false);
            }
        }
    }

    private void OnEnable()
    {
        player = transform.parent.gameObject;
        player.GetComponent<PlayerManager>().KetchupGunEvent += eventHandle;
        AudioManager.Instance.playRandom(transform.position, "Ketchup_Reload_01"); //Sound for switch to Ketchup Weapon -Brian
        player.GetComponent<PlayerMovementTwo>().setOverallAnim("KetchupAnim");    // turn off basic animations
        player.GetComponent<PlayerMovementTwo>().setIdleAnim("KetchupIdle");       // set idle animation
        player.GetComponent<PlayerMovementTwo>().setRunAnim("KetchupRun");         // set run animation
        player.GetComponent<PlayerMovementTwo>().setJumpAnim("KetchupJump");       // set jump animation
    }

    private void OnDisable()
    {
        if (player.GetComponent<PlayerManager>() != null)
        {
            player.GetComponent<PlayerManager>().KetchupGunEvent -= eventHandle;
        }
        reticle.SetActive(false);
        CameraTarget.instance.returnDefault(0.25f);                 // return camera target to default
        player.GetComponent<PlayerMovementTwo>()?.setAiming(false); // turn off aiming for player
        player.GetComponent<Animator>()?.SetTrigger("Restart");     // restart animations
        player.GetComponent<PlayerMovementTwo>()?.setBasicAnim();   // revert to basic animations
    }

    // respond to events
    public void eventHandle(string message)
    {
        switch (message)
        {
            case "bulletSpawn": // only one event for now
                AudioManager.Instance.playRandom(transform.position, "Ketchup_Fire_01"); // play audio clip, added sound -Brian 
                Instantiate(projectile, spawnPoint.position, transform.rotation);
                bulletAmount--;
                break;
        }
    }
}
