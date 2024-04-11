/*-----------------------------------------
Creation Date: 3/21/2024 2:42:38 PM
Author: theco
Description: Base class for all pickup spawners, such as chests and breakable objects.
-----------------------------------------*/

using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum PickupType
{
    None,
    Money1,
    Money5,
    Money20,
    Heart,
    AttackUp,
    DefenseUp,
    Key,
    BossKey,
    BombBag
}

public abstract class ChoosePickup : MonoBehaviour
{
    [SerializeField] protected PickupType thisPickup;
    //public PickupType ThisPickup { get { return thisPickup; } set { thisPickup = ThisPickup; } }

    [SerializeField, SerializedDictionary("Pickup Type", "Prefab")]
    protected SerializedDictionary<PickupType, GameObject> pickupPrefabs;
}
