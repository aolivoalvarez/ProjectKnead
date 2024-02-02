using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanScript : MonoBehaviour
{
    int health;
    [SerializeField] int maxHealth = 4;
    [SerializeField] int attackDamage = 2;
    [SerializeField] float moveSpeed = 5f;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;

    void Start()
    {
        health = maxHealth;
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
}
