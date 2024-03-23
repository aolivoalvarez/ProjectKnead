/*-----------------------------------------
Creation Date: 3/21/2024 5:48:13 PM
Author: theco
Description: Used for all health pickups.
-----------------------------------------*/

using UnityEngine;

public class HealthPickup : MonoBehaviour, IPickup
{
    [SerializeField] int healthAmount;

    public void PlayerCollect()
    {
        PlayerController.instance.IncreaseHealth(healthAmount);
        Destroy(transform.root.gameObject);
    }
}
