using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public PickupItem carriedItem;
    public float reach = 4.0f;
    public float carryDistance = 1.75f;
    public GameObject crosshair;
    private Quaternion carriedRot;
    public static PlayerInventory instance;
    // Start is called before the first frame update
    void Start()
    {
        carriedItem = null;
        carriedRot = Quaternion.identity;
        if (instance == null) {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Interact"))
        {
            if (carriedItem == null)
            {
                RaycastHit hit;
                int layerMask = 1 << 9;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, reach, layerMask, QueryTriggerInteraction.Collide))
                {
                    PickupItem pickupItem = hit.collider.GetComponent<PickupItem>();
                    if (pickupItem != null) {
                        Pickup(pickupItem);
                    }
                }
            }
            else {
                Drop();
            }
        }

        if (carriedItem != null) {
            Rigidbody rb = carriedItem.GetComponent<Rigidbody>();

            float distance = carryDistance;
            RaycastHit hit;
            int layerMask = ~((1 << 9) | (1 << 8) | (1 << 10));
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, carryDistance, layerMask))
            {
                distance = hit.distance < carryDistance ? hit.distance : carryDistance;
            }
            
            Vector3 position = carriedItem.usePreferredPosition ? Camera.main.transform.rotation * carriedItem.preferredPosition : Camera.main.transform.forward * distance;
            Quaternion rotation = carriedItem.usePreferredRotation ? Quaternion.Euler(carriedItem.preferredRotation) : carriedRot;

            carriedItem.transform.position = (Camera.main.transform.position + position);
            rb.MoveRotation(Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up) * rotation);
        }
    }

    public static void Pickup(PickupItem pickupItem, bool preserveRotation = false) {
        if (instance != null) {
            Debug.Log("picked up " + pickupItem.gameObject.name);
            instance.carriedItem = pickupItem;
            if (!preserveRotation) {
                instance.carriedRot = Quaternion.Inverse(Quaternion.LookRotation(-Camera.main.transform.forward, Camera.main.transform.up)) * instance.carriedItem.transform.rotation;
            }
            if (pickupItem.disableCrosshair) {
                instance.crosshair.SetActive(false); 
            }
            Rigidbody rb = instance.carriedItem.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = 0.05f;
        }
    }

    public static void Drop() {
        if (instance != null) {
            Debug.Log("dropping item...");
            if (instance.carriedItem != null) {
                Rigidbody rb = instance.carriedItem.GetComponent<Rigidbody>();
                rb.useGravity = true;
                if (instance.carriedItem.disableCrosshair) {
                    instance.crosshair.SetActive(true); 
                }
            }
            instance.carriedItem = null;
            instance.carriedRot = Quaternion.identity;
        }
    }
}
