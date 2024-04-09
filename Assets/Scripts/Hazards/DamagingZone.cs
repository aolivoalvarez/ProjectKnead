/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a GameObject with a Collider2D trigger. Damages the player for a specified amount.
-----------------------------------------*/

using CustomAttributes;
using UnityEngine;

public class DamagingZone : MonoBehaviour
{
    [SerializeField] int damageAmount = 2;
    [SerializeField] bool appliesKnockback = false;
    [SerializeField, ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(appliesKnockback))]
    float knockbackStrength = 1.0f;
    [SerializeField] bool canJumpOver = false;
    [SerializeField] bool damagesPlayer = true;
    [SerializeField] bool damagesEnemies = false;

    void OnTriggerStay2D(Collider2D other)
    {
        if (damagesPlayer && other.gameObject == PlayerController.instance.gameObject && !(canJumpOver && PlayerController.instance.isJumping))
        {
            if (appliesKnockback)
            {
                Vector2 direction = PlayerController.instance.transform.position - transform.position;
                PlayerController.instance.DecreaseHealth(damageAmount, knockbackStrength, direction);
            }
            else
            {
                PlayerController.instance.DecreaseHealth(damageAmount);
            }
        }
        else if (damagesEnemies && other.gameObject.GetComponent<HenchmanScript>() != null)
        {
            if (appliesKnockback)
            {
                Vector2 direction = other.transform.position - transform.position;
                other.gameObject.GetComponent<HenchmanScript>().TakeDamage(damageAmount, knockbackStrength, direction);
            }
            else
            {
                other.gameObject.GetComponent<HenchmanScript>().TakeDamage(damageAmount);
            }
        }
    }
}
