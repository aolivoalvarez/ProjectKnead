/*-----------------------------------------
Creation Date: 3/21/2024 3:16:34 PM
Author: theco
Description: Base class for all pickups.
-----------------------------------------*/

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(BoxCollider2D)), RequireComponent (typeof(SpriteRenderer))]
public abstract class Pickup : MonoBehaviour
{
    [SerializeField, Tooltip("How long it takes for this pickup to despawn, in seconds.")]
    protected float timeToDespawn = 5f;
    bool isFlashing;

    void Start()
    {
        isFlashing = false;
        StartCoroutine(DespawnRoutine());
    }

    IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(timeToDespawn * 0.5f);
        isFlashing = true;
        StartCoroutine(FlashingRoutine());
        yield return new WaitForSeconds(timeToDespawn * 0.5f);
        Destroy(gameObject);
    }

    IEnumerator FlashingRoutine()
    {
        while (isFlashing)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    protected abstract void PlayerCollect();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerSword"))
        {
            PlayerCollect();
        }
    }
}
