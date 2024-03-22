/*-----------------------------------------
Creation Date: 3/21/2024 2:42:38 PM
Author: theco
Description: Base class for choosing what pickup to give.
-----------------------------------------*/

using UnityEngine;
using CustomAttributes;
using AYellowpaper.SerializedCollections;
using System.Diagnostics;

public enum PickupType
{
    None,
    Money1,
    Money5,
    Money20,
    Heart,
    AttackUp,
    DefenseUp
}

public class PickupChoice : MonoBehaviour
{
    [SerializeField]
    #if UNITY_EDITOR
    [OnChangedCall(nameof(EditorUpdatePickupObject))]
    #endif
    protected PickupType thisPickup;

    [SerializeField]
    [SerializedDictionary("Pickup Type", "Prefab")]
    protected SerializedDictionary<PickupType, GameObject> pickupPrefabs;

    [Conditional("UNITY_EDITOR")]
    public virtual void EditorUpdatePickupObject()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        Instantiate(pickupPrefabs[thisPickup], transform);
    }

    public virtual void UpdatePickupObject()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        Instantiate(pickupPrefabs[thisPickup], transform);
    }

    public void SetPickup(PickupType pickup)
    {
        thisPickup = pickup;
        UpdatePickupObject();
    }
    public PickupType GetPickup() { return thisPickup; }

    public GameObject GetCurrentPrefab()
    {
        return pickupPrefabs[thisPickup];
    }
}
