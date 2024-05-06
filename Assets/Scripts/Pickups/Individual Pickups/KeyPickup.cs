/*-----------------------------------------
Creation Date: 4/9/2024 6:37:30 PM
Author: theco
Description: Used for all key pickups.
-----------------------------------------*/

using UnityEngine;

public class KeyPickup : Pickup
{
    [SerializeField] bool isBossKey = false;

    protected override void PlayerCollect()
    {
        PlayerCollectDontDestroy();
        base.PlayerCollect();
    }

    public override void PlayerCollectDontDestroy()
    {
        if (isBossKey)
        {
            DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].hasBossKey = true;
        }
        else
        {
            DungeonManager.instance.dungeonsInfo[DungeonManager.instance.currentDungeon].keysCollected++;
        }
        base.PlayerCollectDontDestroy();
    }
}
