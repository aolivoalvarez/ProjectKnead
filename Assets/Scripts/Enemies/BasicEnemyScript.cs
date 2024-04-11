/*-----------------------------------------
Creation Date: 4/10/2024 8:21:36 PM
Author: Alex Olivo
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyScript : MonoBehaviour
{
    int health;
    [SerializeField] int maxHealth = 2;
    [SerializeField] int attackDamage = 1;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float attackRange = 5f; //once player is in this range, cheddar will begin throwing
    [SerializeField] float roamDistMax = 10f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 5f; //holds minimum roaming distance from starting point
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField, Range(0f, 1f), Tooltip("Percentage of incoming knockback this basic enemy takes. At 0, no knockback is taken.")]
    float knockbackMultiplier = 1f;

    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds basic enemy's starting position
    public NavMeshAgent agent { get; private set; } //holds reference to basic enemy's navmesh agent
    void Start()
    {
        player = PlayerController.instance.transform;
        startingPosition = transform.position; //gets basic enemy's starting position
        health = maxHealth; //sets health to max
        agent.speed = moveSpeed; //sets speed to moveSpeed

        //references to components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        //After knockback is over, return movement control to the navmesh agent
        if (agent != null && !agent.enabled && rigidBody.velocity.magnitude <= 0.1f)
        {
            agent.enabled = true;
        }
    }

    void FixedUpdate()
    {
        if (agent != null && agent.enabled)
        {
            Roam(); //calls function for basic enemy to roam in it's area

            if (Vector2.Distance(transform.position, player.position) <= attackRange) //checks if player is in attack range
            {
                AttackTarget(); //attacks player
            }
        }
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        health -= damage;

        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills basic enemy if health is 0
        }
    }

    public void TakeDamage(int damage, float knockbackStrength, Vector2 knockbackDirection) //override that applies knockback to this basic enemy
    {
        TakeDamage(damage);

        agent.enabled = false; //navmesh agent must be disabled for AddForce to work properly
        rigidBody.AddForce(100f * knockbackMultiplier * knockbackStrength * knockbackDirection.normalized);
    }

    void Roam() //gets random position and sets it as basic enemy's destination within a certain range of its starting position
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

        roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

        agent.SetDestination(roamPos); //sets basic enemy's destination
    }

    private void AttackTarget() //moves basic enemy towards player to attack
    {
        agent.SetDestination(player.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().DecreaseHealth(attackDamage);
        }
    }

    private void Death() //basic enemy death
    {
        Destroy(gameObject); //destroys gameObject
    }
}
