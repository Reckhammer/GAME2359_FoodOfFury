using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : MonoBehaviour
{

    public float throwForce = 20f;
    public GameObject grenadePrefab;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {

            ThrowGrenade();

        }
    }

    void ThrowGrenade()
    {
        AudioManager.Instance.playRandom(transform.position, "Bomb_Deployed_01").transform.SetParent(transform);

        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }


}
