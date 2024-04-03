/*-----------------------------------------
Creation Date: 3/21/2024 4:52:57 PM
Author: theco
Description: Used for all money pickups.
-----------------------------------------*/

using UnityEngine;
using DG.Tweening;

public class MoneyPickup : Pickup
{
    [SerializeField] int moneyAmount;
    Tween turn1, turn2;

    void Update()
    {
        if (transform.localScale.x == 1f)
            turn1 = transform.DOScaleX(-1f, 0.5f);
        if (!turn1.IsActive() && transform.localScale.x == -1f)
            turn2 = transform.DOScaleX(1f, 0.5f);
    }

    protected override void PlayerCollect()
    {
        PlayerController.instance.IncreaseMoney(moneyAmount);
        Destroy(transform.root.gameObject);
    }

    public override void PlayerCollectDontDestroy()
    {
        PlayerController.instance.IncreaseMoney(moneyAmount);
    }
}
