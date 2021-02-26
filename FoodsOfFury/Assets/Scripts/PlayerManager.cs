using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private void OnEnable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate += HealthUpdated;
    }

    private void OnDisable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate -= HealthUpdated;
    }

    // Does health reactions
    private void HealthUpdated(float amount)
    {
        if (amount == 0)
        {
            doDie();
        }
        else
        {
            print("Player was damaged!");
            // hurt animations?
            // update UI (from GameController)
        }
    }

    // hangles death operations
    private void doDie()
    {
        print("Player died");

        // do death animation?
        // send message to GameController
        //Destroy(gameObject);
    }
}
