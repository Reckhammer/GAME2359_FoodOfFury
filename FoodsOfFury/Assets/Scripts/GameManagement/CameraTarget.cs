using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Controlls the camera target allowing for transitions
//----------------------------------------------------------------------------------------
public class CameraTarget : MonoBehaviour
{
    public static CameraTarget instance { get; private set; } // CameraTarget instance

    private Vector3 defaultPos; // default camera target positon
    private Coroutine cMoving;  // reference to coroutine

    // do singleton stuff
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        defaultPos = transform.localPosition;
    }

    // offsets camera target to postion
    public void offsetTo(Vector3 position, float duration = 0.0f)
    {
        if (duration == 0.0)
        {
            transform.localPosition = position;
            return;
        }

        if (cMoving != null)
        {
            StopCoroutine(cMoving);
        }

        cMoving = StartCoroutine(Moving(position, duration));
    }

    // handles timed movement
    private IEnumerator Moving(Vector3 position, float duration)
    {
        float passed = 0.0f;
        Vector3 start = transform.localPosition;

        while (passed < duration)
        {
            transform.localPosition = Vector3.Lerp(start, position, passed / duration);
            passed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = position;
    }

    // returns camera target to default position
    public void returnDefault(float duration = 0.0f)
    {
        if (duration == 0.0f)
        {
            transform.localPosition = defaultPos;
        }
        else
        {
            offsetTo(defaultPos, duration);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
