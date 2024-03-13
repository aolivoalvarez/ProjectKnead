/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to the Player's child GameObject, Graphic. Contains functions to call from animation events.
-----------------------------------------*/

using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    PlayerAttackScript attackScript;

    void Start()
    {
        attackScript = GetComponentInParent<PlayerAttackScript>();
    }

    void ActivateSwordHitbox()
    {
        attackScript.swordSwingHitbox.SetActive(true);
    }

    void DeactivateSwordHitbox()
    {
        attackScript.swordSwingHitbox.SetActive(false);
    }
}
