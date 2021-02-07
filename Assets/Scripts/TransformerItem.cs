using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformerItem : UsableItem
{
    public int pickupItemId;
    public GameObject prefab;
    override protected void Use()
    {
        RaycastHit hit;
        int layerMask = 1 << 9;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, PlayerInventory.instance.reach, layerMask, QueryTriggerInteraction.Collide))
        {
            PickupItem pickupItem = hit.collider.GetComponent<PickupItem>();
            if (pickupItem != null && pickupItem.id == pickupItemId && !pickupItem.transformed) {
                GameObject instance = Instantiate(prefab, pickupItem.transform.position, pickupItem.transform.rotation);
                Destroy(pickupItem.gameObject);
                pickupItem.transformed = true;
            }
        }
    }
}
