/*-----------------------------------------
Creation Date: 4/9/2024 10:11:23 PM
Author: theco
Description: Used for locked dungeon doors and boss doors.
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LockedDoor : MonoBehaviour
{
    public bool isBossDoor = false;

    public void UnlockDoor()
    {
        if (isBossDoor)
        {
            if (DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].hasBossKey)
                Destroy(gameObject);
        }
        else if (DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].CanUseKey())
        {
            DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].keysUsed++;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough keys!");
        }
    }
}
