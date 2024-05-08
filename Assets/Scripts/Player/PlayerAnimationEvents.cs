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
        PlayerController.instance.isAttacking = true;
    }

    void DeactivateSwordHitbox()
    {
        attackScript.swordSwingHitbox.SetActive(false);
        PlayerController.instance.isAttacking = false;
    }

    void EndFallingIntoPit()
    {
        GameManager.instance.EnablePlayerInput();
        PlayerController.instance.animator.SetBool("IsFallingIntoPit", false);
        GameManager.instance.RespawnAtCheckpoint(2);
    }

    void StartInvincibility()
    {
        PlayerController.instance.isInvincible = true;
        //PlayerController.instance.GetComponentInChildren<SpriteRenderer>().color = Color.green;
    }

    void EndInvincibility()
    {
        PlayerController.instance.isInvincible = false;
        //PlayerController.instance.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    void ToGameOverScene()
    {
        PlayerController.instance.animator.SetBool("IsDying", false);
        SceneManagerScript.SwapScene(GameManager.instance.gameOverScene);
    }
}
