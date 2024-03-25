/*-----------------------------------------
Creation Date: 3/21/2024 3:16:34 PM
Author: theco
Description: For pickups that are collected by walking into them.
-----------------------------------------*/

using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
public class WorldPickupChoice : PickupChoice
{
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerSword"))
        {
            if (GetComponentInChildren<IPickup>() != null)
                GetComponentInChildren<IPickup>().PlayerCollect();
        }
    }
}
