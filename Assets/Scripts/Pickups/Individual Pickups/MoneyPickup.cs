/*-----------------------------------------
Creation Date: 3/21/2024 4:52:57 PM
Author: theco
Description: Used for all money pickups.
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MoneyPickup : Pickup
{
    [SerializeField] int moneyAmount;

    protected new void Start()
    {
        base.Start();
        GetComponent<Animator>().SetInteger("Value", moneyAmount);
    }

    private void Update()
    {
        GetComponent<Animator>().SetInteger("Value", moneyAmount);
    }

    protected override void PlayerCollect()
    {
        PlayerCollectDontDestroy();
        base.PlayerCollect();
    }

    public override void PlayerCollectDontDestroy()
    {
        PlayerController.instance.IncreaseMoney(moneyAmount);
        base.PlayerCollectDontDestroy();
    }
}
