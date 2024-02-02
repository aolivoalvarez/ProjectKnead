using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int health;
    int money;
    [SerializeField] int maxHealth = 12;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;

    [Header("Movement")]
    Vector2 inputDirection;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpTime = 1f; // how many seconds it takes to land

    [Header("Combat")]
    [SerializeField] float swordSwingTime = 0.75f; // how many seconds the swing takes

    void Start()
    {
        health = maxHealth;
        money = 0;
        inputDirection = Vector2.zero;
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
