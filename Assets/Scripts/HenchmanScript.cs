using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanScript : MonoBehaviour
{
    int health;
    [SerializeField] int maxHealth = 4;
    [SerializeField] int attackDamage = 2;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] string group;
    [SerializeField] string animal;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;

    void Start()
    {

        switch (group)
        {
            case "HenchGroup1":
                {
                    maxHealth = 4;
                    attackDamage = 2;
                    break;
                }
            case "HenchGroup2":
                {
                    maxHealth = 8;
                    attackDamage = 4;
                    break;
                }
            case "HenchanGroup3":
                {
                    maxHealth = 12;
                    attackDamage = 8;
                    break;
                }
            default:
                {
                    maxHealth = 4;
                    attackDamage = 2;
                    break;
                }
        }

        health = maxHealth;

        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            player.DecreaseHealth(attackDamage);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
