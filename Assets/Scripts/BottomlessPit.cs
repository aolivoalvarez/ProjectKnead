using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BottomlessPit : MonoBehaviour
{
    [SerializeField, Tooltip("How quickly the pit pulls entities in.")]
    float pullSpeed = 1f;
    [SerializeField, Tooltip("How close an entity must be to fall into the pit.")]
    float fallRadius = 1f;
    [SerializeField, Tooltip("How much damage the player takes if they fall in.")]
    int damage = 2;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.isJumping)
            {
                Vector3 distance = transform.position - other.gameObject.transform.position;
                Vector3 pullDirection = distance.normalized;
                player.roughPosition += new Vector2(pullDirection.x * pullSpeed, pullDirection.y * pullSpeed);

                if (distance.magnitude < fallRadius)
                {
                    gameManager.RespawnAtCheckpoint(damage);
                }
            }
        }
    }
}
