using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cauldron : MonoBehaviour
{
    public static Cauldron instance;
    public static int nextItem {
        get {
            return instance.nextItemID;
        }
    }
    public float baseTimeLimit = 120;
    public float perfectTime = 0.7f;
    public int thermometerId;
    public Renderer potionLiquid;
    private List<int> itemsLeft;
    private int nextItemID;
    private float timer;
    private float timeLimit;
    private Gradient gradient;
    private float accuracy;
    private bool timerPaused;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) {
            instance = this;
        }

        timerPaused = true;
        timer = 0;
        timeLimit = baseTimeLimit;

        GradientColorKey[] colorKey = new GradientColorKey[3];
        colorKey[0].color = Color.green;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = perfectTime;
        colorKey[2].color = Color.red;
        colorKey[2].time = 0.85f;

        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 0.9f;
        alphaKey[0].time = 0.0f;

        gradient = new Gradient();
        gradient.SetKeys(colorKey, alphaKey);
    }

    public static void Reset() {
        if (instance != null) {
            instance.itemsLeft = Checklist.itemsRequired.ToList();
            instance.SetNextItem();
            instance.timer = 0;
            instance.timeLimit = instance.baseTimeLimit;
            instance.timerPaused = false;
        }
        
    }

    public static void PauseTimer() {
        if (instance != null) {
            instance.timerPaused = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerPaused) {
            timer += Time.deltaTime;
        }

        if (timer >= timeLimit) {
            Menu.YouLose();
            timerPaused = true;
            timer = 0;
        }

        potionLiquid.material.color = gradient.Evaluate(timer/timeLimit);
    }

    void OnTriggerEnter(Collider other) 
    {
        PickupItem pickupItem = other.GetComponent<PickupItem>();
        if (pickupItem != null && !pickupItem.destroyed) 
        {
            if (pickupItem.id == thermometerId) {
                Checklist.ShowHideCheck(true);
            }
            else {
                if (pickupItem == PlayerInventory.instance.carriedItem) {
                    PlayerInventory.Drop();
                }

                if (nextItem == pickupItem.id)
                {
                    float timeAdded = timer/timeLimit;
                    Checklist.AddItem(pickupItem.id, gradient.Evaluate(timeAdded));
                    itemsLeft.Remove(nextItem);
                    timer = 0;
                    timeLimit = timeLimit - 10;
                    Destroy(pickupItem.gameObject);
                    pickupItem.destroyed = true;

                    int itemsFound = Checklist.itemsRequired.Length - itemsLeft.Count;
                    accuracy = ((accuracy * (itemsFound - 1)) + Mathf.Abs(perfectTime - timeAdded)) / itemsFound;
                    
                }
                else {
                    timer += 30;
                    HollyPotionMaking.MoveItem(pickupItem.transform);
                }

                if (itemsLeft.Count > 0) {
                    SetNextItem();
                }
                else {
                    int quality = (int)(100 - (accuracy * 100));
                    Menu.GameOver(quality);
                    timerPaused = true;
                    timer = 0;
                }
            }
        }
    }

    private void SetNextItem() {
        nextItemID = itemsLeft[Random.Range(0, itemsLeft.Count)];
        Debug.Log("Next item is: " + nextItem);
        Checklist.ShowHideCheck(false);
        Checklist.SetCheck(nextItem);
    }
}
