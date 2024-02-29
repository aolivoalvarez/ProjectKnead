using System.Collections;
using System.Collections.Generic;
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
