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
    public GameObject projectile;           // shot projectile
    public Transform spawnPoint;            // projectile spawn point
    public float attackDelay        = 0.5f; // attack delay time
    public int bulletAmount         = 10;   // amount of bullets

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }
    
    // does attack
    void Attack()
    {
        if (bulletAmount != 0)
        {
            AudioManager.Instance.playRandom(transform.position, "Ketchup_Fire_01");            // play audio clip, added sound -Brian 
            transform.parent.GetComponentInParent<Animator>().SetTrigger("KetchupAttack_01");   // play visual attack animation
            GetComponentInParent<PlayerManager>().addSwitchDelay(attackDelay + 0.1f);
            //GetComponentInParent<PlayerMovementTwo>().stopInput(attackDelay + 0.1f);            // stop player for a bit

            if (bulletAmount == 0)
            {
                GetComponentInParent<PlayerManager>().remove(ItemType.Weapon, false);
            }
        }
    }

    private void OnEnable()
    {
        GetComponentInParent<PlayerManager>().KetchupGunEvent += eventHandle;
        //GetComponentInParent<PlayerMovementTwo>().setAiming(true);              // set player to aiming
        //CameraTarget.instance.offsetTo(new Vector3(5, 5, 0), 1.0f);             // offset camera target
        AudioManager.Instance.playRandom(transform.position, "Ketchup_Reload_01"); //Sound for switch to Ketchup Weapon -Brian
        GetComponentInParent<PlayerMovementTwo>().setOverallAnim("KetchupAnim");    // turn off basic animations
        GetComponentInParent<PlayerMovementTwo>().setIdleAnim("KetchupIdle");       // set idle animation
        GetComponentInParent<PlayerMovementTwo>().setRunAnim("KetchupRun");         // set run animation
        GetComponentInParent<PlayerMovementTwo>().setJumpAnim("KetchupJump");       // set jump animation
    }

    private void OnDisable()
    {
        GetComponentInParent<PlayerManager>().KetchupGunEvent -= eventHandle;
        //CameraTarget.instance.returnDefault(1.0f);                      // return camera target to default
        //GetComponentInParent<PlayerMovementTwo>()?.setAiming(false);    // turn off aiming for player
        transform.parent.GetComponentInParent<Animator>()?.SetTrigger("Restart");
        GetComponentInParent<PlayerMovementTwo>()?.setBasicAnim();  // revert to basic animations
    }

    // respond to events
    public void eventHandle(string message)
    {
        switch (message)
        {
            case "bulletSpawn": // only one event for now
                Instantiate(projectile, spawnPoint.position, transform.rotation);
                bulletAmount--;
                break;
        }
    }
}
