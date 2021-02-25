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
    public float delay = 3f;

    float countdown;


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;

        GetComponent<Rigidbody>().velocity = transform.forward * speed;

    }

    // Update is called once per frame
    void Update()
    {
        /*
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            Debug.Log("range");
        }
        */


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }

    }


}
