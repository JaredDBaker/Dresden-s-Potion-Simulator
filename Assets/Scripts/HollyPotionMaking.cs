using System.Collections;
using System.Linq;
using static System.Array;
using System.Collections.Generic;
using UnityEngine;

public class HollyPotionMaking : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject introPage;
    public GameObject skull;
    public Transform spawnPlacesParent;
    public Transform tablePlacesParent;
    public Menu menu;
    public static HollyPotionMaking instance;
    private List<PickupItem> itemsInArea;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        itemsInArea = new List<PickupItem>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(int id in Checklist.itemsRequired) {
            Checklist.FoundItem(id, IsItemFound(id));
        }
    }

    // Randomly spawns all of the items 
    void SpawnItems()
    {
        List<int> unusedPlaces = Enumerable.Range(0, spawnPlacesParent.childCount).ToList();
        foreach (GameObject prefab in prefabs) 
        {
            int i = Random.Range(0, unusedPlaces.Count);
            int index = unusedPlaces[i];
            unusedPlaces.RemoveAt(i);
            Instantiate(prefab, spawnPlacesParent.GetChild(index).position, spawnPlacesParent.GetChild(index).rotation);
            Debug.Log("Spawned " + prefab.name + " at " + spawnPlacesParent.GetChild(index).gameObject.name + ": " + index);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null && !itemsInArea.Contains(pickupItem)) 
        {
            itemsInArea.Add(pickupItem);
            if (Checklist.itemsRequired.Contains(pickupItem.id))
            {
                int index = IndexOf(Checklist.itemsRequired, pickupItem.id);
                if (index < tablePlacesParent.childCount) {
                    if (PlayerInventory.instance.carriedItem == pickupItem) {
                        PlayerInventory.Drop();
                    }
                    pickupItem.transform.position = tablePlacesParent.GetChild(index).position;
                    pickupItem.transform.rotation = tablePlacesParent.GetChild(index).rotation;
                }
            }            
        }
    }

    void OnTriggerExit(Collider other) 
    {
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null && itemsInArea.Contains(pickupItem)) 
        {
            itemsInArea.Remove(pickupItem);
        }
    }
    public static void Reset() { 
        if (instance != null) {
            PlayerInventory.Drop();
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            foreach (GameObject item in items) {
                Destroy(item);
            }
            instance.itemsInArea = new List<PickupItem>();
            instance.SpawnItems();
            Instantiate(instance.introPage, new Vector3(0, 1, 0), Quaternion.identity);
            Instantiate(instance.skull, new Vector3(0, 1, -1), Quaternion.Euler(0, -40, 0));
            Cauldron.Reset();
        }
    }

    public static bool IsItemFound(int id) {
        if (instance == null) {
            return false;
        }

        foreach (PickupItem item in instance.itemsInArea)
        {
            if (item.id == id) {
                return true;
            }
        }
        return false;
    }

    public static bool FoundEverything() {
        if (instance == null) {
            return false;
        }

        foreach (int id in Checklist.itemsRequired)
        {
            if (!IsItemFound(id)) {
                return false;
            }
        }
        return true;
    }

    public static void MoveItem(Transform item) { 
        if (instance != null) {
            int index = Random.Range(0, instance.spawnPlacesParent.childCount);
            item.position = instance.spawnPlacesParent.GetChild(index).position;
            item.rotation = instance.spawnPlacesParent.GetChild(index).rotation;
            Debug.Log("Moved " + item.gameObject.name + " to " + instance.spawnPlacesParent.GetChild(index).gameObject.name + ": " + index);
        }
    }
}
