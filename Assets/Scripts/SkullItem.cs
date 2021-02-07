using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkullItem : UsableItem
{
    public string[] tips;
    private int tipIndex = 0;
    private bool used = false;
    override protected void Use()
    {
        if (used || tipIndex != 0) {
            tipIndex = (tipIndex % (tips.Length - 1)) + 1;
        }
        used = true;
        
        if (tipIndex == 0) {
            Tooltips.ShowChecklist();
        }
    }

    override protected void Update()
    {
        base.Update();
        if (PlayerInventory.instance != null)
        {
            PickupItem item = PlayerInventory.instance.carriedItem;
            bool isCarrying = item != null;
            if (!isCarrying) {
                used = false;
            }
            Tooltips.ShowHideSkullText(isCarrying && item.id == 15 && used, tips[tipIndex]);
        }
    }
}
