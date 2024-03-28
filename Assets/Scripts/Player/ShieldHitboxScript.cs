/*-----------------------------------------
Creation Date: 3/26/2024 5:37:41 PM
Author: theco
Description: Checks if an object should be destoyed by the shield.
-----------------------------------------*/

using UnityEngine;

public class ShieldHitboxScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BlockedByShield"))
        {
            Destroy(other.gameObject);
        }
    }
}
