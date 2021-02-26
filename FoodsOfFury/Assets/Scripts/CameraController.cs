using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Controlls the camera actings as a free look camera (proxy for cinemachine)
//
// TODO: write comments explaining class
//----------------------------------------------------------------------------------------

public class CameraController : MonoBehaviour
{
    public Transform target;
    //public Vector3 offset     = new Vector3(0, 0, 0);
    public float scrollSpeed  = 2.0f;
    //public float cameraSmooth = 0.0f;
    public float topLimit     = -20.0f;
    public float bottomLimit  = 80.0f;
    public float minDistance  = 3.0f;
    public float maxDistance  = 10.0f;
    public float minCollidingDistance = 0.1f;
    public float collisionDetectionDistance = 1.0f;
    public float cameraDistance = 4.0f;
    public LayerMask collideWith;

    private Camera cam;
    private float mouseX;
    private float mouseY;
    private float wantedDistance = 0.0f;

    Vector3 velocity = Vector3.one;

    private void Start()
    {
        cam = GetComponent<Camera>();
        wantedDistance = cameraDistance;
    }

    void Update()
    {
        mouseY += Input.GetAxis("Mouse Y");
        mouseX += Input.GetAxis("Mouse X");
        mouseY = Mathf.Clamp(mouseY, -bottomLimit, -topLimit);

        wantedDistance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        wantedDistance = Mathf.Clamp(wantedDistance, minDistance, maxDistance);

        if (!checkViewCollision())
            cameraDistance = wantedDistance;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(-mouseY, mouseX, 0);
        transform.position = target.position + rotation * new Vector3(0, 0, -cameraDistance);
        //smoothFollow();
        transform.LookAt(target.position);
    }

    // smoothly follows the target (this breaks collision checks if used)
    //private void smoothFollow()
    //{
    //    Quaternion rotation = Quaternion.Euler(-mouseY, mouseX, 0);

    //    Vector3 desiredPosition = target.position + rotation * new Vector3(0, 0, -cameraDistance);
    //    Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, cameraSmooth);

    //    transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, smoothedPosition.z) + offset;
    //}

    // checks for camera collisions between viewports and target
    private bool checkViewCollision()
    {
        Vector3[] starts = new Vector3[4];
        Vector3 vpOffset = (transform.forward * (cameraDistance - Camera.main.nearClipPlane)); // distance from viewport and target
        starts[0] = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)) + vpOffset;
        starts[1] = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)) + vpOffset;
        starts[2] = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)) + vpOffset;
        starts[3] = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)) + vpOffset;

        Vector3[] ends = new Vector3[4];

        // set ends
        for (int x = 0; x <= 3; x++)
        {
            ends[x] = starts[x] + (-transform.forward * (wantedDistance + collisionDetectionDistance));
            Debug.DrawLine(starts[x], ends[x], Color.black);
        }

        RaycastHit hit;

        float distance = 0.0f;

        //for (int x = 0; x <= 3; x++)
        //{
        //    if (Physics.Raycast(starts[x], -transform.forward, out hit, Vector3.Distance(starts[x], ends[x])))
        //    {
        //        if (hit.collider.tag != "Player")
        //        {
        //            if (distance == 0.0f || distance > (hit.distance - collisionDetectionDistance))
        //                distance = hit.distance - collisionDetectionDistance;
        //        }
        //    }
        //}

        for (int x = 0; x <= 3; x++)
        {
            if (Physics.Raycast(starts[x], -transform.forward, out hit, Vector3.Distance(starts[x], ends[x]), collideWith, QueryTriggerInteraction.Ignore))
            {
                if (distance == 0.0f || distance > (hit.distance - collisionDetectionDistance))
                    distance = hit.distance - collisionDetectionDistance;
            }
        }

        if (distance != 0)
        {
            cameraDistance = distance;

            if (cameraDistance < minCollidingDistance)
            {
                cameraDistance = minCollidingDistance;
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
