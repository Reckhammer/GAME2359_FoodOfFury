using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnabler : MonoBehaviour
{
    public GameObject[] objects;            // objects to be enabled
    public float enableDelayTime = 0.5f;    // time delay between enabling objects
    public GameObject enableParticle;       // particles when object is enabled
    public bool randomizeOrder = false;     // randomize the enable order

    private Coroutine routine;

    // spawns the objects
    public void enableObjects()
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
            enableObject(objects[current]);
            passed = enableDelayTime;    // set time to next interval
            current++;                  // set current to next index
        }

        // while there is still more to spawn
        while (current <= amount)
        {
            //print("delay time: " + (current + 1) * delay);
            // if next one is ready to spawn
            if (passed >= (current + 1) * enableDelayTime)
            {
                enableObject(objects[current]);
                current++;
            }
            passed += Time.deltaTime;
            yield return null;
        }
    }

    // enables an object
    private void enableObject(GameObject obj)
    {
        obj.SetActive(true);

        if (enableParticle != null) // create particle effect and destroy after a short time has passed
        {
            Destroy(Instantiate(enableParticle, obj.transform.position, obj.transform.rotation), 1.0f);
        }
    }

    private void randomize()
    {
        int length = objects.Length - 1;

        // shuffle gameobjects
        for (int x = 0; x < length; x++)
        {
            GameObject temp = objects[x];
            int rIndex = Random.Range(x, length + 1);

            objects[x] = objects[rIndex];
            objects[rIndex] = temp;
        }
    }

    public void destroyObjects()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }
}