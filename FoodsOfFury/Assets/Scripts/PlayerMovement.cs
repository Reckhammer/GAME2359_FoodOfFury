using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 10f;
    public float jumpSpeed = 1.5f;
    private const int maxJump = 2;
    private int currentJump = 0;
    private bool onGround = true;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.Translate(horizontal, 0, vertical);

        if (Input.GetKeyDown("space") && (onGround || maxJump > currentJump))
        {
            rb.AddForce(new Vector3(0, 5, 0) * jumpSpeed, ForceMode.Impulse);
            onGround = false;
            currentJump++;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            onGround = true;
            currentJump = 0;
        }
    }
}
