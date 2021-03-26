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
    public ItemType     type;                   // type of item
    public Renderer     render      = null;     // object renderer

    private GameObject  player      = null;     // target that can pick item up
    private Coroutine   highlightCr = null;     // Highlight() coroutine
    private bool        canPickUp   = false;    // to check if pickup is ready
    private Color       orignal;                // orignal renderer color

    void Start()
    {
        orignal = render.material.GetColor("Color_23038745b9c94e5f899a6bcb26c1f301");
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            if (highlightCr != null)
            {
                StopCoroutine(highlightCr);         // stop coroutine
                render.material.SetColor("Color_23038745b9c94e5f899a6bcb26c1f301", orignal);    // return to orignal color (before pickup)
            }

            if (player.GetComponentInParent<Inventory>().add(gameObject, type))
            {
                if (type == ItemType.Consumable)
                {
                    AudioManager.Instance.playRandom(transform.position, "Pickup_Health_1", "Pickup_Health_2", "Pickup_Health_3");
                }
                player.GetComponentInParent<PlayerManager>().equipNextItem(type); // call player manager to equip new item
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
        if (render == null)
        {
            yield return null;
        }

        while (canPickUp) // do color lerp
        {
            render.material.SetColor("Color_23038745b9c94e5f899a6bcb26c1f301", Color.Lerp(orignal, color, Mathf.PingPong(Time.time * speed, 1)));
            yield return null;
        }
        render.material.SetColor("Color_23038745b9c94e5f899a6bcb26c1f301", orignal); // return to orignal color
    }
}
