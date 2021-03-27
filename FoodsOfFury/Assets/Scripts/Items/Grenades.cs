using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenades : MonoBehaviour
{

    public float delay = 3f;
    public float radius = 5f;
    public float force = 100f;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;
    public LayerMask enemyLayers;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

    }

    void Explode()
    {
        Debug.Log("Exploded");
        //Explodion particle effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        AudioManager.Instance.playRandom(transform.position, "Bomb_Explode_01").transform.SetParent(transform);

        //Detect Character
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            //add force
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
            //damage
        }

            //remove grenade
            Destroy(gameObject);
    }

}
