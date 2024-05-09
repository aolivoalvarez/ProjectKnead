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
            AudioManager.instance.PlaySound(29);
            if (DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].hasBossKey)
                Destroy(gameObject);
        }
        else if (DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].CanUseKey())
        {
            AudioManager.instance.PlaySound(29);
            DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].keysUsed++;
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not enough keys!");
        }
    }
}
