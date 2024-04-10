/*-----------------------------------------
Creation Date: 3/21/2024 5:48:13 PM
Author: theco
Description: Used for all health pickups.
-----------------------------------------*/

using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] int healthAmount;

    protected override void PlayerCollect()
    {
        PlayerController.instance.IncreaseHealth(healthAmount);
        base.PlayerCollect();
    }

    public override void PlayerCollectDontDestroy()
    {
        PlayerController.instance.IncreaseHealth(healthAmount);
    }
}
