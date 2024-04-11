/*-----------------------------------------
Creation Date: 4/7/2024 3:46:44 PM
Author: theco
Description: Stores specific info about a dungeon.
-----------------------------------------*/

using UnityEngine;

[CreateAssetMenu]
public class DungeonInfo : ScriptableObject
{
    public GameObject wholeDungeonPrefab;
    public GameObject[] roomPrefabs;
    public int keysCollected = 0;
    public int keysUsed = 0;
    public bool hasBossKey = false;

    public void ResetValues()
    {
        keysCollected = 0;
        keysUsed = 0;
        hasBossKey = false;
    }

    public bool CanUseKey() { return keysCollected > keysUsed; }
}
