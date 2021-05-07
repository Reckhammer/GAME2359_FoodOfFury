using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidRain : MonoBehaviour
{
    public GameObject damagingTrigger;  // damging trigger gameobject
    public ParticleSystem rainParticle; // partical system for rain
    public float damageDelay    = 3.0f; // time before rain does damage
    public float rainTime       = 2.0f; // time for rain to be active
    public float stopOffset     = 3.0f; // time for particles to stop showing
    public float destroyDelay   = 5.0f; // time before object gets destroyed

    public AudioSource RainDown;

    void Start()
    {
        StartCoroutine(rainEffect());
    }

    // does acid rain sequences
    private IEnumerator rainEffect()
    {
        float passed = 0.0f;

        // turn on indicator for duration
        while (passed < damageDelay)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        damagingTrigger.SetActive(true);
        audioSource = AudioManager.Instance.playRandom(other.ClosestPointOnBounds(transform.position), "Grease_Rain_01");

        passed = 0.0f; // reset time for next sequence

        while (passed < rainTime)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        rainParticle.Stop();
        passed = 0.0f; // reset time for next sequence

        while (passed < stopOffset)
        {
            passed += Time.deltaTime;
            yield return null;
        }
        damagingTrigger.SetActive(false);

        Destroy(gameObject, destroyDelay); // destroy object when done
    }
}
