/*-----------------------------------------
Creation Date: 3/26/2024 3:39:53 PM
Author: theco
Description: Handles player shielding action and animations.
-----------------------------------------*/

using UnityEngine;

public class PlayerShieldScript : MonoBehaviour
{
    [SerializeField] GameObject shield;
    PlayerController playerController;
    Animator animator;

    void Start()
    {
        playerController = PlayerController.instance;
        animator = shield.GetComponent<Animator>();
    }

    void Update()
    {
        if (playerController.pInput.Player.Shield.inProgress &&
            Inventory.instance.collectedItems[Inventory.Item.PotLid] == true && Inventory.instance.currentShield == Inventory.Shield.PotLid &&
            !playerController.isAttacking && !playerController.isHoldingObject && !playerController.isLifting && !playerController.isJumping && !playerController.isRolling)
        {
            playerController.isShielding = true;
            shield.SetActive(true);
            playerController.moveSpeedMult = 0.5f;
        }
        else
        {
            playerController.isShielding = false;
            shield.SetActive(false);
            playerController.moveSpeedMult = 1f;
        }
        animator.SetFloat("Look X", playerController.simpleLookDirection.x);
        animator.SetFloat("Look Y", playerController.simpleLookDirection.y);
    }
}
