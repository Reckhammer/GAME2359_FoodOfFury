using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV
 * 
 *  Description: This class handles the contriols for the projectiles of the player range weapon.
 *  
 *  */

public class Projectiles : MonoBehaviour
{
    public float speed = 20f;
    public float destroyTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }
}
