using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nFryEnemyAttack : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject projectile;
    public float attackDelay = 1.0f;

    private GameObject player;
    private float passedTime = 0.0f;
    private bool inAttack = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // need a better way of getting player
        passedTime = attackDelay; // let first attack start immediately
    }

    private void Update()
    {
        if (!inAttack)
        {
            passedTime += Time.deltaTime;
            if (passedTime > attackDelay)
            {
                attackOne();
            }
        }
    }

    private void attackOne()
    {
        passedTime = 0.0f;
        GetComponent<Animator>().SetTrigger("Attack");
    }

    private void OnEnable()
    {
        GetComponent<nEnemy>().enemyEvent += eventHandle;
    }

    private void OnDisable()
    {
        GetComponent<nEnemy>().enemyEvent -= eventHandle;
    }

    // respond to events
    private void eventHandle(string message)
    {
        switch (message)
        {
            case "spawnBullet":
                Instantiate(projectile, spawnPoint.transform.position, Quaternion.LookRotation((player.transform.position - spawnPoint.transform.position).normalized));
                break;
            case "inAttack":
                inAttack = true;
                break;
            case "outAttack":
                inAttack = false;
                break;
            default:
                break;
        }
    }
}
