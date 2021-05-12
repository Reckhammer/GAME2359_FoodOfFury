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
    public WaveSpawner[] waveOneSpawners;   // wave one enemy spawners
    public WaveSpawner[] waveTwoSpawners;   // wave two enemy spawners
    public WaveSpawner[] waveThreeSpawners; // wave three enemy spawners
    public Spawner acidRainSpawner;         // acid rain spawner
    public ObjectEnabler hazardEnabler;     // hazard enabler
    public GameObject endObject;            // ending trigger object
    public Animator animator;               // animator of boss
    public delegate void EndFight();
    public event EndFight OnEnd;

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
        if (waveOneSpawners != null)
        {
            foreach (WaveSpawner spawner in waveOneSpawners)
            {
                waveCounts[0] += spawner.waveEnemies.Length * spawner.loopAmount;
            }
           // print("wave one count: " + waveOneCount);
        }
        else
        {
            print("wave one spawner is not linked!");
        }

        if (waveTwoSpawners != null)
        {
            foreach (WaveSpawner spawner in waveTwoSpawners)
            {
                waveCounts[1] += spawner.waveEnemies.Length * spawner.loopAmount;
            }
            //print("wave two count: " + waveTwoCount);
        }
        else
        {
            print("wave two spawner is not linked!");
        }

        if (waveThreeSpawners != null)
        {
            foreach (WaveSpawner spawner in waveThreeSpawners)
            {
                waveCounts[2] += spawner.waveEnemies.Length * spawner.loopAmount;
            }
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

        foreach (WaveSpawner spawner in waveOneSpawners)
        {
            spawner.spawnWave();
        }
        animator.SetTrigger("WaveStart");
        AudioManager.Instance.play("Burger_Voice_03", animator.transform.position);
    }

    private void doWaveTwo()
    {
        UIManager.instance.setObjectiveText(waveTwoText + " " + waveCounts[1]);
        foreach (WaveSpawner spawner in waveTwoSpawners)
        {
            spawner.spawnWave();
        }
        acidRainSpawner.spawn();
        animator.SetTrigger("WaveStart");
        AudioManager.Instance.play("Burger_Voice_03", animator.transform.position);
    }

    private void doWaveThree()
    {
        UIManager.instance.setObjectiveText(waveThreeText + " " + waveCounts[2]);
        foreach (WaveSpawner spawner in waveThreeSpawners)
        {
            spawner.spawnWave();
        }
        hazardEnabler.enableObjects();
        animator.SetTrigger("WaveStart");
        AudioManager.Instance.play("Burger_Voice_04", animator.transform.position);
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
                    //print("wave two start");
                    doWaveTwo();
                    break;
                case 2:
                    //print("wave three start");
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
        OnEnd();
        UIManager.instance.setObjectiveText(bossDefeatedText);
        print(bossDefeatedText);
        endObject.SetActive(true);
    }
}