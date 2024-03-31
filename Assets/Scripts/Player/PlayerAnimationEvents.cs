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

    void EndFallingIntoPit()
    {
        GameManager.instance.EnablePlayerInput();
        PlayerController.instance.animator.SetBool("IsFallingIntoPit", false);
        GameManager.instance.RespawnAtCheckpoint(2);
    }

    void ToGameOverScene()
    {
        PlayerController.instance.animator.SetBool("IsDying", false);
        SceneManagerScript.SwapScene(GameManager.instance.gameOverScene);
    }
}
