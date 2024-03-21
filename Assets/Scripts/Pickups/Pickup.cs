/*-----------------------------------------
Creation Date: 3/21/2024 2:42:38 PM
Author: theco
Description: Base class for all pickups.
-----------------------------------------*/

using UnityEngine;
using CustomAttributes;
using AYellowpaper.SerializedCollections;

public enum PickupType
{
    Money1,
    Money5,
    Money20,
    Heart,
    AttackUp,
    DefenseUp
}

public class Pickup : MonoBehaviour
{
    [SerializeField]
    [OnChangedCall(nameof(UpdatePickupObject))]
    protected PickupType thisPickup;

    [SerializedDictionary("Pickup Type", "Prefab")]
    SerializedDictionary<PickupType, GameObject> pickupPrefabs;

    public void UpdatePickupObject()
    {
        Destroy(GetComponentInChildren<GameObject>());
        Instantiate(pickupPrefabs[thisPickup], transform);
    }
}
