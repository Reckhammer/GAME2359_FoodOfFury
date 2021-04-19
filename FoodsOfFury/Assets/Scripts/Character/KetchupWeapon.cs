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
    public float bulletSpawnOffset  = 0.0f; // time offset to spawn in a bullet

    private Coroutine cTimer;               // coroutine reference
    private bool canAttack      = true;     // for attack check

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }

    }
    
    // does attack
    void Attack()
    {
        if (canAttack && bulletAmount != 0 && GetComponentInParent<PlayerMovementTwo>().onGround())
        {
            //AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01");  // play audio clip 
            GetComponentInParent<Animator>().SetTrigger("KetchupAttack_01");            // play visual attack animation
            GetComponentInParent<PlayerManager>().addSwitchDelay(attackDelay + 0.1f);
            GetComponentInParent<PlayerMovementTwo>().stopInput(attackDelay + 0.1f);    // stop player for a bit
            //Instantiate(projectile, spawnPoint.position, transform.rotation);

            bulletAmount--;

            if (bulletAmount == 0)
            {
                GetComponentInParent<PlayerManager>().remove(ItemType.Weapon, false);
            }

            //GetComponentInParent<Animator>().SetTrigger("KetchupAttack"); // play visual attack animation

            if (cTimer != null)
            {
                StopCoroutine(cTimer);
                cTimer = StartCoroutine(attackTimer(attackDelay));
            }
            else
            {
                cTimer = StartCoroutine(attackTimer(attackDelay));
            }
        }
    }

    // times attack
    private IEnumerator attackTimer(float duration)
    {
        float passed = 0.0f;
        canAttack = false;

        bool shot = false;
        while (passed < duration)
        {
            if (passed > bulletSpawnOffset && !shot)
            {
                Instantiate(projectile, spawnPoint.position, transform.rotation);
                shot = true;
            }

            passed += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }

    private void OnEnable()
    {
        //GetComponentInParent<PlayerMovementTwo>().setAiming(true);              // set player to aiming
        //CameraTarget.instance.offsetTo(new Vector3(5, 5, 0), 1.0f);             // offset camera target
        GetComponentInParent<PlayerMovementTwo>().setOverallAnim("KetchupAnim");    // turn off basic animations
        GetComponentInParent<PlayerMovementTwo>().setIdleAnim("KetchupIdle");       // set idle animation
        GetComponentInParent<PlayerMovementTwo>().setRunAnim("KetchupRun");         // set run animation
        GetComponentInParent<PlayerMovementTwo>().setJumpAnim("KetchupJump");       // set jump animation
    }

    private void OnDisable()
    {
        canAttack = true;

        if (cTimer != null)
        {
            StopCoroutine(cTimer);
        }
        //CameraTarget.instance.returnDefault(1.0f);                      // return camera target to default
        //GetComponentInParent<PlayerMovementTwo>()?.setAiming(false);    // turn off aiming for player
        GetComponentInParent<Animator>()?.SetTrigger("Restart");
        GetComponentInParent<PlayerMovementTwo>()?.setBasicAnim();  // revert to basic animations
    }
}
