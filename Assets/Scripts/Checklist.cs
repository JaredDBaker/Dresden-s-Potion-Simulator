using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.Array;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Checklist : MonoBehaviour
{
    public static int[] itemsRequired {
        get {
            return new int[] { 0, 1, 2, 3, 6, 8, 9, 10 };
        }
    }
    public GameObject itemList;
    public TextMeshProUGUI[] listItems;
    public RectTransform check;
    public static Checklist instance;
    private bool[] itemsFound;
    private bool[] itemsAdded;
    private Color[] itemsColor;
    // Start is called before the first frame update
    void Start()
    {
        itemsFound = new bool[itemsRequired.Length];
        itemsAdded = new bool[itemsRequired.Length];
        itemsColor = new Color[itemsRequired.Length];
        if (instance == null) {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < listItems.Length; i++) {
            if (itemsAdded[i]) {
                listItems[i].color = itemsColor[i];
            }
            else {
                listItems[i].color = Color.black;
            }
        }
    }

    public static void AddItem(int id, Color color) {
        int index = IndexOf(itemsRequired, id);
        instance.itemsAdded[index] = true;
        instance.itemsColor[index] = color;
    }
    public static void FoundItem(int id, bool found) {
        int index = IndexOf(itemsRequired, id);
        instance.itemsFound[index] = found;
    }
    public static void ChangeListActive()
    {
        if (instance != null) {
            instance.itemList.SetActive(!instance.itemList.activeSelf); 
        }
               
    }
    public static void ShowHideCheck(bool show) {
        if (instance != null) {
            instance.check.gameObject.SetActive(show); 
        }
    }
    public static void SetCheck(int id) {
        if (instance != null) {
            int index = IndexOf(itemsRequired, id);
            instance.check.anchoredPosition = new Vector2(instance.check.anchoredPosition.x, instance.listItems[index].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }
}
