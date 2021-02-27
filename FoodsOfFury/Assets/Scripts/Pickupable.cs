using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: Allows the ability to add an item to inventory.
//----------------------------------------------------------------------------------------

public class Pickupable : MonoBehaviour
{
    public ItemType    type;                    // type of item

    private GameObject  player      = null;     // target that can pick item up
    private Coroutine   highlightCr = null;     // Highlight() coroutine
    private Renderer    render      = null;     // object renderer
    private bool        canPickUp   = false;    // to check if pickup is ready
    private Color       orignal;                // orignal renderer color

    void Start()
    {
        render  = GetComponent<Renderer>();
        orignal = render.material.color;
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            if (highlightCr != null)
            {
                StopCoroutine(highlightCr);         // stop coroutine
                render.material.color = orignal;    // return to orignal color (before pickup)
            }

            if (player.GetComponentInParent<Inventory>().add(type, gameObject))
            {
                player.GetComponentInParent<PlayerManager>().equipItem(type); // call player manager to equip item
                Destroy(gameObject); // item succesfully added, delete this object
            }
        }
    }

    // player is inside trigger, check for input
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = true;
            player = other.gameObject;

            if (highlightCr == null) // start highlight coroutine if none exists
            {
                highlightCr = StartCoroutine(Highlight(Color.yellow, 1.0f));
            }
        }
    }

    // player left trigger, stop checking for input
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = false;
            highlightCr = null;
            player = null;
        }
    }

    // coroutine to lerp objects color (does a highlight effect)
    private IEnumerator Highlight(Color color, float speed)
    {
        while (canPickUp) // do color lerp
        {
            render.material.color = Color.Lerp(orignal, color, Mathf.PingPong(Time.time * speed, 1));
            yield return null;
        }
        render.material.color = orignal; // return to original color
    }
}
