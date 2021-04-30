using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct objectSpawn
{
    public GameObject spawnObject;  // object to be spawned
    public Transform spawnPosition; // position for enemy to be spawned
}

public class Spawner : MonoBehaviour
{
    public objectSpawn[] objects;                   // object spawn info
    public float spawnDelayTime         = 0.5f;     // time delay between spawining objects
    public GameObject spawnParticle;                // particles when object is spawned
    public bool randomizeOrder          = false;    // randomize the spawn order
    public bool looping                 = false;    // option to loop spawning
    public float loopDelay              = 1.0f;     // delay for next spawn loop

    private Coroutine routine;

    // spawns the objects
    public void spawn()
    {
        if (objects.Length != 0)
        {
            routine = StartCoroutine(spawnDelay());
        }
        else
        {
            print("objects is empty!");
        }
    }

    public void stop()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }

    private IEnumerator spawnDelay(bool firstDelayed = false)
    {
        float passed = 0.0f;
        int current = 0;
        int amount = objects.Length - 1;

        if (randomizeOrder)
        {
            randomize();
        }

        // spawn first without delay
        if (!firstDelayed)
        {
            spawnObject(objects[current]);
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
                spawnObject(objects[current]);
                current++;
            }
            passed += Time.deltaTime;
            yield return null;
        }

        if (looping)
        {
            passed = 0.0f;
            while (passed <= loopDelay)
            {
                passed += Time.deltaTime;
                yield return null;
            }

            routine = StartCoroutine(spawnDelay(firstDelayed));
        }
        //print("done spawning objects " + wave);
    }

    // spawns an enemy on the navmesh
    private void spawnObject(objectSpawn obj)
    {
        GameObject e = Instantiate(obj.spawnObject, obj.spawnPosition.position, obj.spawnPosition.rotation);

        if (spawnParticle != null) // create particle effect and destroy after a short time has passed
        {
            Destroy(Instantiate(spawnParticle, obj.spawnPosition.position, obj.spawnPosition.rotation), 1.0f);
        }
    }

    private void randomize()
    {
        int length = objects.Length - 1;

        // shuffle gameobjects
        for (int x = 0; x < length; x++)
        {
            objectSpawn temp = objects[x];
            int rIndex = Random.Range(x, length + 1);

            objects[x] = objects[rIndex];
            objects[rIndex] = temp;
        }
    }

    private void OnDisable()
    {
        stop();
    }
}
