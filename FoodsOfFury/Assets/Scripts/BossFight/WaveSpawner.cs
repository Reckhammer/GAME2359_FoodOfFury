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
    public enemySpawn[] wave1;          // wave1 enemy spawn info
    public float spawnDelayTime = 0.5f; // time delay to spawn enemies
    public GameObject spawnParticle; // particles when enemy is spawned

    void Start()
    {
        if (wave1.Length != 0)
        {
            StartCoroutine(spawnDelay(wave1, 1, spawnDelayTime));
        }
        else
        {
            print("wave 1 is empty!");
        }
    }

    void Update()
    {
        
    }

    private IEnumerator spawnDelay(enemySpawn[] enemies, int wave, float delay = 0.0f, bool firstDelayed = false)
    {
        float passed = 0.0f;
        int current = 0;
        int amount = enemies.Length - 1;

        // spawn first without delay
        if (!firstDelayed)
        {
            spawnEnemy(enemies[current], current, wave);
            passed = delay; // set time to next interval
            current++;      // set current to next index
        }

        // while there is still more to spawn
        while (current <= amount)
        {
            //print("delay time: " + (current + 1) * delay);
            // if next one is ready to spawn
            if (passed >= (current + 1) * delay)
            {
                spawnEnemy(enemies[current], current, wave);
                current++;
            }
            passed += Time.deltaTime;
            yield return null;
        }
        //print("done spawning wave " + wave);
    }

    // spawns an enemy on the navmesh (current & wave are for debuging)
    private void spawnEnemy(enemySpawn enemy, int current, int wave)
    {
        NavMeshHit closestHit; // grab the closest position on navmesh from spawn position to spawn enemy
        if (NavMesh.SamplePosition(enemy.spawnPosition.position, out closestHit, 2.0f, NavMesh.AllAreas))
        {
            GameObject e = Instantiate(enemy.enemyGameobject, closestHit.position, enemy.spawnPosition.rotation);

            if (spawnParticle != null) // create particle effect and destroy after a short time has passed
            {
                Destroy(Instantiate(spawnParticle, closestHit.position, enemy.spawnPosition.rotation), 1.0f);
            }
        }
        else
        {
            print("[wave " + wave + ", index: " + current + "], " + enemy.enemyGameobject.name + "'s spawn position is not close enough to the navmesh!");
        }
    }
}
