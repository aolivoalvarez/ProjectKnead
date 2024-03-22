/*-----------------------------------------
Creation Date: 3/21/2024 3:16:34 PM
Author: theco
Description: For pickups that are collected by walking into them.
-----------------------------------------*/

using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class WorldPickupChoice : PickupChoice
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerSword"))
        {
            GetComponentInChildren<IPickup>().PlayerCollect();
        }
    }
}
