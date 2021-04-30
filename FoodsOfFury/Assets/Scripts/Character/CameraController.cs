using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Controlls the camera actings as a free look camera (proxy for cinemachine)
//----------------------------------------------------------------------------------------

public class CameraController : MonoBehaviour
{
    public Transform target;                            // transform of target
    public GameObject targetVisuals;                    // visuals to turn off if colliding with camera
    //public Vector3 offset     = new Vector3(0, 0, 0); // camera offset (not used)
    public float scrollSpeed  = 2.0f;                   // scroll speed for camera distance
    //public float cameraSmooth = 0.0f;                 // camera smooth modifier (not used)
    public float topLimit     = -20.0f;                 // top limit of camera
    public float bottomLimit  = 80.0f;                  // bottom limit of camera
    //public float minDistance  = 3.0f;                   // min distance of camera
    //public float maxDistance  = 10.0f;                  // max distance of camera
    public float minCollidingDistance = 0.1f;           // min distance of camera when colliding
    public float collisionDetectionDistance = 1.0f;     // distance to detect collision behind camera
    public float cameraDistance = 4.0f;                 // current camera distance
    public LayerMask collideWith;                       // layers that the camera will collide with

    private Camera cam;                                 // reference to camera
    private float mouseX;                               // mouse x pos
    private float mouseY;                               // mouse y pos
    private float wantedDistance = 0.0f;                // wanted camera distance

    //Vector3 velocity = Vector3.one;                   // reference for smooth damp (not used)

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = GetComponent<Camera>();
        wantedDistance = cameraDistance;

        if (!GetComponent<Collider>())
        {
            gameObject.AddComponent<CapsuleCollider>().isTrigger = true;
        }

        // get eular loacl angles from transform (reverse operation of rotation to get starting rotation)
        mouseX = transform.localEulerAngles.y;
        mouseY = (transform.localEulerAngles.x < 180) ? -transform.localEulerAngles.x : -(transform.localEulerAngles.x - 360); // get negative eular angles too
        //print("transform.localEulerAngles.x: " + transform.localEulerAngles.x);
        //print("eular fixed mouseY: " + mouseY);
    }

    void Update()
    {
        if (PauseMenu.gameIsPaused)
        {
            return;
        }

        // get and clamp mouse pos
        mouseY += Input.GetAxis("Mouse Y");
        mouseX += Input.GetAxis("Mouse X");
        print("Mouse X: " + mouseX);
        print("Mouse Y: " + mouseY);
        mouseY = Mathf.Clamp(mouseY, -bottomLimit, -topLimit);

        // get and clamp wanted camera distance
        //wantedDistance += -Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        //wantedDistance = Mathf.Clamp(wantedDistance, minDistance, maxDistance);

        // if not colliding set camera distance to wanted distance
        if (!checkViewCollision())
            cameraDistance = wantedDistance;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(-mouseY, mouseX, 0);                             // create rotation based on angle of mouse input pos
        transform.position = target.position + rotation * new Vector3(0, 0, -cameraDistance);   // set position
        //smoothFollow();
        transform.LookAt(target.position);                                                      // set rotation towards target
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && targetVisuals != null)
        {
            targetVisuals.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && targetVisuals != null)
        {
            targetVisuals.SetActive(true);
        }
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
        Vector3[] starts = new Vector3[4];                                                      // starting positions for raycast
        Vector3 vpOffset = (transform.forward * (cameraDistance - Camera.main.nearClipPlane));  // distance from viewport and target
        starts[0] = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)) + vpOffset;  // bottom left viewport
        starts[1] = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)) + vpOffset;  // bottom right viewport
        starts[2] = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)) + vpOffset;  // top left viewport
        starts[3] = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)) + vpOffset;  // top right viewport

        Vector3[] ends = new Vector3[4]; // end positions for ray cast

        // set ends
        for (int x = 0; x <= 3; x++)
        {
            ends[x] = starts[x] + (-transform.forward * (wantedDistance + collisionDetectionDistance)); // to behind camera + collisionDetectionDistance
            Debug.DrawLine(starts[x], ends[x], Color.black);
        }

        RaycastHit hit;
        float distance = 0.0f;

        // grab longest detected collision distance
        for (int x = 0; x <= 3; x++)
        {
            if (Physics.Raycast(starts[x], -transform.forward, out hit, Vector3.Distance(starts[x], ends[x]), collideWith, QueryTriggerInteraction.Ignore))
            {
                if (distance == 0.0f || distance > (hit.distance - collisionDetectionDistance)) // set distance only if it is the longest
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

            return true; // collision detected set cameraDistance (clamped to minCollidingDistance)
        }
        else
        {
            return false; // no collision detected
        }
    }
}
