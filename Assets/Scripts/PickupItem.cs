using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PickupItem : MonoBehaviour
{
    public int id;
    public bool transformed = false;
    public bool destroyed = false;
    public bool disableCrosshair = false;
    public bool usePreferredPosition = false;
    public Vector3 preferredPosition = Vector3.zero;
    public bool usePreferredRotation = false;
    public Vector3 preferredRotation = Vector3.zero;
}
