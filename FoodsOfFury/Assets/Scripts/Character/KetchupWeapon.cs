using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KetchupWeapon : MonoBehaviour
{
    public GameObject projectile;           // shot projectile
    public Transform spawnPoint;            // projectile spawn point

    private Coroutine cTimer;               // coroutine reference
    private float attackDelay   = 0.5f;     // attack delay time
    private bool canAttack      = true;     // for attack check

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
        }

    }

    void Attack()
    {
        if (canAttack && GetComponentInParent<PlayerMovementTwo>().onGround())
        {
            //AudioManager.Instance.playRandom(transform.position, "Weapon_Swing_01"); // play audio clip 
            Instantiate(projectile, spawnPoint.position, transform.rotation);
            //GetComponentInParent<PlayerMovementTwo>().stopInput(attackDelay + 0.1f); // stop player input for a bit
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

    // basic timer
    private IEnumerator attackTimer(float duration)
    {
        float passed = 0.0f;
        canAttack = false;
        GetComponentInParent<PlayerMovementTwo>().setAiming(true);

        while (passed < duration)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        canAttack = true;

        while (passed < duration + 0.1f)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        GetComponentInParent<PlayerMovementTwo>().setAiming(false);
    }

    private void OnEnable()
    {
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
        GetComponentInParent<PlayerMovementTwo>()?.stopInput(0.0f, false);
        GetComponentInParent<PlayerMovementTwo>()?.setAiming(false);
        //GetComponentInParent<PlayerMovementTwo>()?.setBasicAnim(true); // revert to basic animations
    }
}
