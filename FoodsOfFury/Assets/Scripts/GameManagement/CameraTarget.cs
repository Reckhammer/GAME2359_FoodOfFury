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
    private Vector3 targetPos;
    private Coroutine cMoving;  // reference to coroutine
    private LayerMask collideWith;

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
        targetPos = defaultPos;
        collideWith = Camera.main.GetComponent<CameraController>().collideWith;
    }

    private void Update()
    {
        if (targetPos != defaultPos)
        {
            //print("checking collision");
            checkCollision();
        }
        else if (targetPos == defaultPos && transform.localPosition != defaultPos)
        {
            //print("default position revert");
            transform.localPosition = defaultPos;
        }
    }

    // do complicated camera logic to stop camera clipping
    private void checkCollision()
    {
        float collisionDetectDistance = 1.0f;
        Vector3 start = transform.parent.transform.TransformPoint(defaultPos);
        Vector3 end = transform.parent.transform.TransformPoint(targetPos);
        float distance = Vector3.Distance(start, end);

        Vector3 dir = -(start - end).normalized;
        Debug.DrawLine(start, end + (dir * collisionDetectDistance), Color.magenta);

        RaycastHit hit;
        if (Physics.Raycast(start, dir, out hit, distance + collisionDetectDistance, collideWith, QueryTriggerInteraction.Ignore))
        {
            //print("hit: " + hit.transform.gameObject);
            distance = hit.distance - collisionDetectDistance;

            Vector3 endPosition = start + (dir * distance);
            transform.localPosition = transform.parent.InverseTransformPoint(endPosition);
        }
        else
        {
            transform.localPosition = targetPos;
        }

        //print("start: " + start);
        //print("end: " + end);
        //print("defaultPos: " + defaultPos);
        //print("targetPos: " + targetPos);
        //print("distance: " + distance);
        //print("dir: " + dir);
        //print("localPos: " + transform.localPosition);
        //print("------------------------------------------------");
    }

    // offsets camera target to postion
    public void offsetTo(Vector3 position, float duration = 0.0f)
    {
        if (duration == 0.0)
        {
            transform.localPosition = position;
            return;
        }

        targetPos = position;

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
            //transform.localPosition = Vector3.Lerp(start, position, passed / duration);
            targetPos = Vector3.Lerp(start, position, passed / duration);
            passed += Time.deltaTime;
            yield return null;
        }

        //transform.localPosition = position;
        targetPos = position;
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
