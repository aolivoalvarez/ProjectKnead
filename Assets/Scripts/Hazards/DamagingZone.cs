/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a GameObject with a Collider2D trigger. Damages the player for a specified amount.
-----------------------------------------*/

using UnityEngine;

public class DamagingZone : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == PlayerController.instance.gameObject)
        {
            PlayerController.instance.DecreaseHealth(damageAmount);
        }
    }
}
