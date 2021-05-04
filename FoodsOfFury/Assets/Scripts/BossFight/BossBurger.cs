using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBurger : MonoBehaviour
{
    public static BossBurger Instance { get; private set; }

    public string waveOneText;              // objective text when wave one starts
    public string waveTwoText;              // objective text when wave two starts
    public string waveThreeText;            // objective text when wave three starts
    public string bossDefeatedText;         // objective text when boss is defeated
    public WaveSpawner waveOneSpawner;      // wave one enemy spawner
    public WaveSpawner waveTwoSpawner;      // wave two enemy spawner
    public WaveSpawner waveThreeSpawner;    // wave three enemy spawner
    public Spawner acidRainSpawner;         // acid rain spawner
    public ObjectEnabler hazardEnabler;     // hazard enabler

    private int[] waveCounts = { 0, 0, 0 }; // wave enemy amount array
    private int currentWave = 0;            // curent wave index

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
            waveCounts[0] = waveOneSpawner.waveEnemies.Length;
           // print("wave one count: " + waveOneCount);
        }
        else
        {
            print("wave one spawner is not linked!");
        }

        if (waveTwoSpawner != null)
        {
            waveCounts[1] = waveTwoSpawner.waveEnemies.Length;
            //print("wave two count: " + waveTwoCount);
        }
        else
        {
            print("wave two spawner is not linked!");
        }

        if (waveThreeSpawner != null)
        {
            waveCounts[2] = waveThreeSpawner.waveEnemies.Length;
            //print("wave three count: " + waveThreeCount);
        }
        else
        {
            print("wave three spawner is not linked!");
        }

        if (acidRainSpawner == null)
        {
            print("acid rain spawner is not linked!");
        }

        if (hazardEnabler == null)
        {
            print("hazard enabler is not linked!");
        }

        doWaveOne();
    }

    private void doWaveOne()
    {
        UIManager.instance.setObjectiveText(waveOneText + " " + waveCounts[0]);
        waveOneSpawner.spawnWave();
    }

    private void doWaveTwo()
    {
        UIManager.instance.setObjectiveText(waveTwoText + " " + waveCounts[1]);
        waveTwoSpawner.spawnWave();
        acidRainSpawner.spawn();
    }

    private void doWaveThree()
    {
        UIManager.instance.setObjectiveText(waveThreeText + " " + waveCounts[2]);
        waveThreeSpawner.spawnWave();
        hazardEnabler.enableObjects();
    }

    public void waveEnemyDeath(WaveNumber num)
    {
        waveCounts[(int)num]--;

        if (waveCounts[(int)num] == 0)
        {
            currentWave++;

            switch (currentWave) // just in case we ever add more waves
            {
                case 1:
                    print("wave two start");
                    doWaveTwo();
                    break;
                case 2:
                    print("wave three start");
                    doWaveThree();
                    break;
                default:
                    acidRainSpawner.stop();
                    hazardEnabler.destroyObjects();
                    doDeath();
                    break;
            }
        }
        else
        {
            switch (currentWave) // just in case we ever add more waves
            {
                case 0:
                    UIManager.instance.setObjectiveText(waveOneText + " " + waveCounts[0]);
                    break;
                case 1:
                    UIManager.instance.setObjectiveText(waveTwoText + " " + waveCounts[1]);
                    break;
                case 2:
                    UIManager.instance.setObjectiveText(waveThreeText + " " + waveCounts[2]);
                    break;
            }
        }
    }

    private void doDeath()
    {
        UIManager.instance.setObjectiveText(bossDefeatedText);
        print(bossDefeatedText);
    }
}