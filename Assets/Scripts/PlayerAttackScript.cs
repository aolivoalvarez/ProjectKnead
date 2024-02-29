using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackScript : MonoBehaviour
{
    PlayerController playerController;
    bool canAttack;

    [Header("Sword")]
    [SerializeField] GameObject swordSwingHitbox;
    [SerializeField, Tooltip("Max time an attack lasts, in seconds.")]
    float swordSwingTime = 0.5f;
    [SerializeField, Tooltip("Buffer after every attack, in seconds.")]
    float swordSwingBuffer = 0.1f;

    void Start()
    {
        playerController = GetComponentInParent<PlayerController>();
        canAttack = true;
        swordSwingHitbox.SetActive(false);
    }

    void Update()
    {
        if (playerController.pInput.Player.Attack.triggered)
        {
            if (canAttack)
                StartCoroutine(SwordAttackRoutine());
        }
    }

    IEnumerator SwordAttackRoutine()
    {
        canAttack = false;
        swordSwingHitbox.SetActive(true);
        yield return new WaitForSeconds(swordSwingTime);
        swordSwingHitbox.SetActive(false);
        yield return new WaitForSeconds(swordSwingBuffer);
        canAttack = true;
    }
}
