using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public int pickupItemId;
    public GameObject prefab;

    void OnTriggerEnter(Collider other) {
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null && pickupItem.id == pickupItemId && !pickupItem.transformed) {
            GameObject instance = Instantiate(prefab, pickupItem.transform.position, pickupItem.transform.rotation);
            PlayerInventory.Pickup(instance.GetComponent<PickupItem>(), true);
            Destroy(pickupItem.gameObject);
            pickupItem.transformed = true;
        }
    }
}
