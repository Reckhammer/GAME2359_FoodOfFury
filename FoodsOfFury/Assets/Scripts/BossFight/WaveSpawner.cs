using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct enemySpawn
{
    public GameObject enemy;
    public Vector3 position;
    public Vector3[] waypoints;
}

public class WaveSpawner : MonoBehaviour
{
    public enemySpawn[] wave1;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
