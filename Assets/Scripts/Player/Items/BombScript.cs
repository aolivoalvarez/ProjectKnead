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
    float bombSpeed = 3f;

    void Start()
    {
        StartCoroutine(ExplosionRoutine());
    }

    public void BombStartMoving(Vector2 moveDirection)
    {
        var rigidBody = gameObject.AddComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0f;
        rigidBody.angularDrag = 0f;
        rigidBody.freezeRotation = true;
        rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        rigidBody.velocity = moveDirection * bombSpeed;
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(timeUntilExplosion * 0.5f);
        GetComponentInChildren<Animator>().SetBool("IsBlinking", true);
        yield return new WaitForSeconds(timeUntilExplosion * 0.5f);
        if (GetComponent<Rigidbody2D>() == null)
        {
            var rigidBody = gameObject.AddComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0f;
            rigidBody.angularDrag = 0f;
            rigidBody.freezeRotation = true;
            rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        }    
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponentInChildren<Animator>().SetTrigger("Explode");
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
    }    

    void OnTriggerStay2D(Collider2D other)
    {
        if (GetComponent<Rigidbody2D>() != null && !other.isTrigger && !other.CompareTag("Player") && !other.CompareTag("Enemy"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
