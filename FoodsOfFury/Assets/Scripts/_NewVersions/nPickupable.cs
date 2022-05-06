using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nPickupable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        if (other.GetComponent<nPlayerInventory>().addItem(GetComponent<nItem>().type))
        {
            // play pickup sound
            switch (GetComponent<nItem>().type)
            {
                case nItemType.Key:
                    AudioManager.Instance.play("Pickup_Key_01", transform.position, true);
                    break;
                case nItemType.StarFruit:
                    AudioManager.Instance.play("Pickup_Starfruit_01", transform.position, true);
                    break;
                case nItemType.HealthPickup:
                    AudioManager.Instance.play("Pickup_Health_02", transform.position, true);
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
