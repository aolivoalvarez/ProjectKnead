/*-----------------------------------------
Creation Date: 3/28/2024 2:03:41 PM
Author: theco
Description: Attached to the Bomb prefab, controls the behavior of bombs spawned by the player.
-----------------------------------------*/

using System.Collections;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField, Tooltip("How long it takes for the bomb to explode, in seconds.")]
    float timeUntilExplosion = 4f;
    [SerializeField, Tooltip("Speed of the bomb in units per second.")]
    float bombSpeed = 1f;
    public bool movingBomb { get; set; } = false;
    Animator animator;
    Rigidbody2D rigidBody;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        GetComponent<Collider2D>().enabled = movingBomb; // if it's a stationary bomb, its collider becomes a trigger

        switch (PlayerController.instance.simpleLookDirection.x)
        {
            case -1.0f:
                animator.SetTrigger("Left");
                break;
            case 1.0f:
                animator.SetTrigger("Right");
                break;
            default:
                break;
        }
        switch (PlayerController.instance.simpleLookDirection.y)
        {
            case -1.0f:
                animator.SetTrigger("Down");
                break;
            case 1.0f:
                animator.SetTrigger("Up");
                break;
            default:
                break;
        }

        if (movingBomb)
            rigidBody.velocity = PlayerController.instance.simpleLookDirection * bombSpeed;

        StartCoroutine(ExplosionRoutine());
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(timeUntilExplosion * 0.5f);
        animator.SetTrigger("StartBlinking");
        yield return new WaitForSeconds(timeUntilExplosion * 0.5f);
        animator.SetTrigger("Explode");
    }    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.otherCollider.isTrigger)
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
