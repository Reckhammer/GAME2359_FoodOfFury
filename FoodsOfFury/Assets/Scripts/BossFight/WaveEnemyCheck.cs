using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveNumber
{
    WaveOne,
    WaveTwo,
    WaveThree
};

public class WaveEnemyCheck : MonoBehaviour
{
    public WaveNumber number;

    private bool quiting = false;

    private void OnApplicationQuit()
    {
        quiting = true;
    }

    private void OnDestroy()
    {
        if (!quiting)
        {
            BossBurger.Instance?.waveEnemyDeath(number);
        }
    }
}
