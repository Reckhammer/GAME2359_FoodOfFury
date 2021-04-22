using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct enemySpawn
{
    public GameObject enemyGameobject;  // enemy to be spawned
    public Transform spawnPosition;     // position for enemy to be spawned
}

public class WaveSpawner : MonoBehaviour
{
    public enemySpawn[] wave1;

    void Start()
    {
        if (wave1.Length != 0)
        {
            for (int x = 0; x <= wave1.Length - 1; x++)
            {
                NavMeshHit closestHit; // grab the closest position on navmesh from spawn position to spawn enemy
                if (NavMesh.SamplePosition(wave1[x].spawnPosition.position, out closestHit, 2.0f, NavMesh.AllAreas))
                {
                    GameObject e = Instantiate(wave1[x].enemyGameobject);
                    e.transform.position = closestHit.position;
                }
                else
                {
                    print("[wave 1, index: " + x + "], " + wave1[x].enemyGameobject.name + "'s spawn position is not close enough to the navmesh!");
                }
            }
        }
        else
        {
            print("wave 1 is empty!");
        }
    }

    void Update()
    {
        
    }
}
