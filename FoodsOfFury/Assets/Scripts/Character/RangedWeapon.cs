using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------------------------------
 * Author: Abdon J. Puente IV
 * 
 *  Description: This class handles the range weapon
 *  
 *  */

[System.Serializable]
public class RangedWeapon : MonoBehaviour
{

    public GameObject shot;
    public Transform BulletSpawner;
    public float fireRate = 1.5f;

    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z") && Time.time > nextFire)
        {

            nextFire = Time.time + fireRate;
            Instantiate(shot, BulletSpawner.position, BulletSpawner.rotation);

        }
    }

}
