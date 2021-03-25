using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV, Jose Villanueva
 * 
 *  Description: This class handles the player's melee weapons.
 *  
 *  */

public class MeleeWeapon : MonoBehaviour
{
    public Animation attackAnim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //print("doing attack");
            Attack();
        }

    }

    void Attack()
    {
        if (!attackAnim.isPlaying && GetComponentInParent<PlayerMovement>().onGround())
        {
            GetComponentInParent<PlayerMovement>().stopInput(0.5f);     // stop play for a bit
            GetComponentInParent<Animator>().SetTrigger("OnionAttack"); // play visual attack animation
            attackAnim.Play();                                          // collider animation
        }
    }

    private void OnEnable()
    {
        GetComponentInParent<PlayerMovement>().setBasicAnim(false);         // turn off basic animations
        GetComponentInParent<PlayerMovement>().setIdleAnim("OnionIdle");    // set idle animation
        GetComponentInParent<PlayerMovement>().setRunAnim("OnionRun");      // set run animation
    }

    private void OnDisable()
    {
        GetComponentInParent<PlayerMovement>().setBasicAnim(true); // revert to basic animations
    }
}
