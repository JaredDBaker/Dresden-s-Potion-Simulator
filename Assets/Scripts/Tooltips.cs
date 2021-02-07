using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltips : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipObject;
    public GameObject checklistObject;
    public GameObject skullGUIObject;
    public TextMeshProUGUI skullGUIText;
    private static Tooltips instance;
    private bool pickedUpItem;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        pickedUpItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInventory.instance != null)
        {
            PickupItem item = PlayerInventory.instance.carriedItem;
            bool isCarrying = item != null;
            if (!pickedUpItem && isCarrying)
            {
                pickedUpItem = true;
            }

            if (!pickedUpItem)
            {
                ShowTooltip("Press E to pick up items");
            }
            else if (isCarrying && item.id == 11)
            {
                ShowTooltip("Press F to shred items");
            }
            else if (isCarrying && item.id == 15)
            {
                ShowTooltip("Press F to talk");
            }
            else
            {
                HideTooltip();
            }
        }
    }

    public static void ShowTooltip(string text)
    {
        if (instance != null)
        {
            instance.tooltipText.text = text;
            instance.tooltipObject.SetActive(true);
        }
    }
    public static void HideTooltip()
    {
        if (instance != null)
        {
            instance.tooltipObject.SetActive(false);
        }
    }

    public static void ShowHideSkullText(bool show, string text)
    {
        if (instance != null)
        {
            instance.skullGUIObject.SetActive(show);
            instance.skullGUIText.text = text;
        }
    }
    public static void ShowChecklist()
    {
        if (instance != null)
        {
            instance.checklistObject.SetActive(true);
        }
    }
}
