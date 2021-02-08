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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // pressing the arrow keys or AWSD the character moves
        float vertical = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.Translate(horizontal, 0, vertical);

        // press the space bar to jump and press it again to double jump
        if (Input.GetKeyDown("space") && (onGround || maxJump > currentJump))
        {
            rb.AddForce(new Vector3(0, 5, 0) * jumpSpeed, ForceMode.Impulse);
            onGround = false;
            currentJump++;
        }

        // press x to glide
        if(Input.GetKey("x") && (onGround == false))
        {
            rb.drag = 15;
            currentJump = 2;
        }

        if(Input.GetKeyUp("x"))
        {
            rb.drag = 0;
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
