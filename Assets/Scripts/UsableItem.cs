using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PickupItem))]
public abstract class UsableItem : MonoBehaviour
{
    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetButtonUp("Use") && PlayerInventory.instance.carriedItem == GetComponent<PickupItem>())
        {
            Use();
        }
    }

    protected abstract void Use();
}
