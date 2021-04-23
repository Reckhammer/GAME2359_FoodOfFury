using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : MonoBehaviour
{
    public GameObject indicator;            // indicator object (warns the player)
    public GameObject rain;                 // rain object (does the damage)
    public float indicatorTime      = 3.0f; // time for indicator to be active
    public float rainTime           = 2.0f; // time for rain to be active

    void Start()
    {
        StartCoroutine(rainEffect());    
    }

    // does acid rain sequences
    private IEnumerator rainEffect()
    {
        float passed = 0.0f;

        // turn on indicator for duration
        indicator.SetActive(true);
        while (passed < indicatorTime)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        indicator.SetActive(false);
        passed = 0.0f; // reset time for next sequence

        // turn on rain for duration
        rain.SetActive(true);

        while (passed < rainTime)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        rain.SetActive(false);

        Destroy(gameObject); // destroy object when done
    }
}
