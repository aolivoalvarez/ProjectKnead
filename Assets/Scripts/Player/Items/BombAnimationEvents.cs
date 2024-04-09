/*-----------------------------------------
Creation Date: 3/28/2024 3:14:05 PM
Author: theco
Description: Attached to Bomb's child GameObject, Graphic. Contains functions to call from animation events.
-----------------------------------------*/

using UnityEngine;

public class BombAnimationEvents : MonoBehaviour
{
    void BombStartExplosion()
    {
        GetComponentInParent<BombScript>().transform.SetParent(null);
        if (PlayerController.instance.GetComponent<PlayerUseItemScript>().heldItem == GetComponentInParent<BombScript>().gameObject)
            PlayerController.instance.GetComponent<PlayerUseItemScript>().heldItem = null;
    }

    void BombExplode()
    {
        Destroy(GetComponentInParent<BombScript>().gameObject);
    }
}
