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
    public float attackDelay   = 0.5f;      // attack delay time

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
        if (canAttack)
        {
            //AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 
            Instantiate(projectile, spawnPoint.position, transform.rotation);
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

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        canAttack = true;
    }

    private void OnEnable()
    {
        GetComponentInParent<PlayerMovementTwo>().setAiming(true);              // set player to aiming
        CameraTarget.instance.offsetTo(new Vector3(5, 5, 0), 1.0f);             // offset camera target
        //GetComponentInParent<PlayerMovementTwo>().setBasicAnim(false);         // turn off basic animations
        //GetComponentInParent<PlayerMovementTwo>().setIdleAnim("KetchupIdle");    // set idle animation
        //GetComponentInParent<PlayerMovementTwo>().setRunAnim("KetchupRun");      // set run animation
    }

    private void OnDisable()
    {
        canAttack = true;

        if (cTimer != null)
        {
            StopCoroutine(cTimer);
        }
        CameraTarget.instance.returnDefault(1.0f);                      // return camera target to default
        GetComponentInParent<PlayerMovementTwo>()?.setAiming(false);    // turn off aiming for player
        //GetComponentInParent<PlayerMovementTwo>()?.setBasicAnim(true); // revert to basic animations
    }
}
