using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    bool canAttack;
    PlayerController playerController;
    Animator animator;

    [Header("Sword")]
    public GameObject swordSwingHitbox;
    [SerializeField, Tooltip("Max time an attack lasts, in seconds.")]
    float swordSwingTime = 0.5f;
    [SerializeField, Tooltip("Buffer after every attack, in seconds.")]
    float swordSwingBuffer = 0.1f;

    void Start()
    {
        canAttack = true;
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        swordSwingHitbox.SetActive(false);
    }

    void Update()
    {
        if (playerController.pInput.Player.Attack.triggered)
        {
            if (canAttack && !playerController.isHoldingObject)
                StartCoroutine(SwordAttackRoutine());
        }
    }

    IEnumerator SwordAttackRoutine()
    {
        animator.SetTrigger("Attack");
        playerController.isAttacking = true; // player stops moving when attacking
        canAttack = false;
        //swordSwingHitbox.SetActive(true);
        yield return new WaitForSeconds(swordSwingTime);
        //swordSwingHitbox.SetActive(false);
        yield return new WaitForSeconds(swordSwingBuffer);
        canAttack = true;
        playerController.isAttacking = false; // player can move again after attack ends
    }
}
