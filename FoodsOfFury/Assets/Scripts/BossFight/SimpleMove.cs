using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public Transform[] positions;           // target positions to move to
    public float speed              = 1.0f; // speed of movement
    public bool changeRotation      = true; // option to change object rotation towards target position

    private int current             = 0;    // current positions index
    private float minDistance       = 0.2f; // min distance before going to next target position
    private float passed            = 0.0f; // time that passed (using in position lerp)
    private Vector3 oldPos;                 // old positon (used in position lerp)

    private void Start()
    {
        oldPos = transform.position;

        // change rotation to direction of target position
        if (positions.Length != 0 && changeRotation)
        {
            transform.rotation = Quaternion.LookRotation((positions[current].position - transform.position).normalized);
        }
    }

    void FixedUpdate()
    {
        if (positions.Length == 0) // if positions is empty return
        {
            return;
        }

        // lerp from old position to target position
        transform.position = Vector3.Lerp(oldPos, positions[current].position, passed * speed);
        passed += Time.deltaTime;

        // if in min distance from next point, go to next index
        if (Vector3.Distance(transform.position, positions[current].position) <= minDistance)
        {
            passed = 0.0f; // reset timer
            current = ((current + 1) != positions.Length) ? current + 1 : 0; // update current to next index
            oldPos = transform.position; // update old position

            // change rotation to direction of target position
            if (changeRotation)
            {
                transform.rotation = Quaternion.LookRotation((positions[current].position - transform.position).normalized);
            }
        }
    }
}
