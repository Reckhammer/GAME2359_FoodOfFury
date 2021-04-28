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
    public enemySpawn[] waveEnemies;    // wave enemy spawn info
    public WaveNumber number;           // number of wave
    public float spawnDelayTime = 0.5f; // time delay to spawn enemies
    public GameObject spawnParticle;    // particles when enemy is spawned
    public bool randomizeOrder = false; // randomize the spawn order

    void Start()
    {
        spawnWave();
    }

    // spawns the wave
    public void spawnWave()
    {
        if (waveEnemies.Length != 0)
        {
            StartCoroutine(spawnDelay());
        }
        else
        {
            print("waveEnemies is empty!");
        }
    }

    private IEnumerator spawnDelay(bool firstDelayed = false)
    {
        float passed = 0.0f;
        int current = 0;
        int amount = waveEnemies.Length - 1;

        if (randomizeOrder)
        {
            randomize();
        }

        // spawn first without delay
        if (!firstDelayed)
        {
            spawnEnemy(waveEnemies[current], current);
            passed = spawnDelayTime;    // set time to next interval
            current++;                  // set current to next index
        }

        // while there is still more to spawn
        while (current <= amount)
        {
            //print("delay time: " + (current + 1) * delay);
            // if next one is ready to spawn
            if (passed >= (current + 1) * spawnDelayTime)
            {
                spawnEnemy(waveEnemies[current], current);
                current++;
            }
            passed += Time.deltaTime;
            yield return null;
        }
        //print("done spawning wave " + wave);
    }

    // spawns an enemy on the navmesh (current is for debuging)
    private void spawnEnemy(enemySpawn enemy, int current)
    {
        NavMeshHit closestHit; // grab the closest position on navmesh from spawn position to spawn enemy
        if (NavMesh.SamplePosition(enemy.spawnPosition.position, out closestHit, 2.0f, NavMesh.AllAreas))
        {
            GameObject e = Instantiate(enemy.enemyGameobject, closestHit.position, enemy.spawnPosition.rotation);
            e.AddComponent<WaveEnemyCheck>().number = number;

            if (spawnParticle != null) // create particle effect and destroy after a short time has passed
            {
                Destroy(Instantiate(spawnParticle, closestHit.position, enemy.spawnPosition.rotation), 1.0f);
            }
        }
        else
        {
            print("[wave " + number + ", index: " + current + "], " + enemy.enemyGameobject.name + "'s spawn position is not close enough to the navmesh!");
        }
    }

    private void randomize()
    {
        int length = waveEnemies.Length - 1;

        // shuffle gameobjects
        for (int x = 0; x < length; x++)
        {
            enemySpawn temp = waveEnemies[x];
            int rIndex = Random.Range(x, length + 1);

            waveEnemies[x] = waveEnemies[rIndex];
            waveEnemies[rIndex] = temp;
        }
    }
}
