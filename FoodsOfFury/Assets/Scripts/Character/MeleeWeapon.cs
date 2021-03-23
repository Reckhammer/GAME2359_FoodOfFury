using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV
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
        if (!attackAnim.isPlaying)
        {
            GetComponentInParent<Animator>().SetTrigger("OnionAttack");
            attackAnim.Play();
        }
    }

}
