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

    private void OnDestroy()
    {
        BossBurger.Instance?.waveEnemyDeath(number);
    }
}
