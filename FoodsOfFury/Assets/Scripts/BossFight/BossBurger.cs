using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBurger : MonoBehaviour
{
    public static BossBurger Instance { get; private set; }

    public WaveSpawner waveOneSpawner;      // reference to wave one spawner
    public WaveSpawner waveTwoSpawner;      // reference to wave two spawner
    public WaveSpawner waveThreeSpawner;    // reference to wave three spawner

    private int waveOneCount    = 0;    // amount of enemies in wave one spawner
    private int waveTwoCount    = 0;    // amount of enemies in wave two spawner
    private int waveThreeCount  = 0;    // amount of enemies in wave three spawner

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        if (waveOneSpawner != null)
        {
            waveOneCount = waveOneSpawner.waveEnemies.Length;
           // print("wave one count: " + waveOneCount);
        }
        else
        {
            print("wave one spawner is not linked!");
        }

        if (waveTwoSpawner != null)
        {
            waveTwoCount = waveTwoSpawner.waveEnemies.Length;
            //print("wave two count: " + waveTwoCount);
        }
        else
        {
            print("wave two spawner is not linked!");
        }

        if (waveThreeSpawner != null)
        {
            waveThreeCount = waveThreeSpawner.waveEnemies.Length;
            //print("wave three count: " + waveThreeCount);
        }
        else
        {
            print("wave three spawner is not linked!");
        }
    }

    public void waveEnemyDeath(WaveNumber num)
    {
        print("wave " + num + " enemy died");
    }
}