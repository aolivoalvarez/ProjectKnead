/*-----------------------------------------
Creation Date: 3/21/2024 4:52:57 PM
Author: theco
Description: Used for all money pickups.
-----------------------------------------*/

using UnityEngine;

public class MoneyPickup : MonoBehaviour, IPickup
{
    [SerializeField] int moneyAmount;

    public void PlayerCollect()
    {
        PlayerController.instance.IncreaseMoney(moneyAmount);
        Destroy(transform.root.gameObject);
    }
}
