/*-----------------------------------------
Creation Date: 3/21/2024 6:10:02 PM
Author: theco
Description: For pickups that are spawned from objects and enemies.
-----------------------------------------*/

using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using DG.Tweening;

public class SpawnPickup : ChoosePickup
{
    [SerializeField] bool randomizePickup = false;
    [SerializeField, SerializedDictionary("Pickup Type", "Item Weight")]
    SerializedDictionary<PickupType, int> pickupChance;

    void Start()
    {
        if (randomizePickup)
            thisPickup = GetRandomWeightedIndex(pickupChance);
    }

    public void SpawnThisPickup()
    {
        if (thisPickup != PickupType.None)
        {
            if (pickupPrefabs[thisPickup] != null)
            {
                var spawnedPickup = Instantiate(pickupPrefabs[thisPickup], new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);
                spawnedPickup.GetComponent<Pickup>().StartCoroutine(spawnedPickup.GetComponent<Pickup>().BounceRoutine(
                    spawnedPickup.transform.DOMoveY(transform.position.y, 0.5f).SetEase(AnimationCurvesScript.instance.droppedPickup)));
            }
        }
    }

    PickupType GetRandomWeightedIndex(Dictionary<PickupType, int> weights)
    {
        if (weights == null || weights.Count == 0) return PickupType.None;

        int weightSum = 0;
        foreach (var (key, value) in weights)
        {
            if (value >= 0) weightSum += value;
        }

        float r = UnityEngine.Random.value;
        float s = 0f;

        foreach (var (key, value) in weights)
        {
            if (value <= 0) continue;

            s += (float)value / weightSum;
            if (s >= r) return key;
        }

        return PickupType.None;
    }

    // FUNCTION TAKEN FROM USER LORDOFDUCT ON UNITY FORUMS
    /*int GetRandomWeightedIndex(int[] weights)
    {
        if (weights == null || weights.Length == 0) return -1;

        int weightSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] >= 0) weightSum += weights[i];
        }

        float r = UnityEngine.Random.value;
        float s = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] <= 0f) continue;

            s += (float)weights[i] / weightSum;
            if (s >= r) return i;
        }

        return -1;
    }*/
}
