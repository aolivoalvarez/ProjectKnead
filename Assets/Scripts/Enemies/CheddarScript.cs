/*-----------------------------------------
Creation Date: 04/04/24
Author: alex
Description: 
-----------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CheddarScript : MonoBehaviour
{

    int health;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int attackDamage = 4;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float attackRange = 10f; //once player is in this range, cheddar will begin throwing
    [SerializeField] float roamDistMax = 10f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 5f; //holds minimum roaming distance from starting point
    [SerializeField] GameObject bomb; //holds reference to weapon prefab
    [SerializeField] GameObject bombParent; //holds reference to weapon parent game object
    [SerializeField] GameObject key; //dungeon key that cheddar drops
    [SerializeField] GameObject cake; //cake slice that cheddar drops
    int throwAmt; //amount of times cheddar has thrown a bomb
    [SerializeField] int throwRate = 1; //rate that cheddar throws bombs
    int stunTimer = 5; //how long cheddar is stunned for

    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds cheddar's starting position
    NavMeshAgent agent; //holds reference to cheddar's navmesh agent


    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance.transform;
        startingPosition = transform.position; //gets cheddar's starting position
        health = maxHealth; //sets health to max

        //references to components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Idle(); //default idling

        if (Vector2.Distance(transform.position, player.position) <= attackRange && throwAmt < 3) //if player is close and he hasn't thrown 3 bombs, cheddar attacks
        {
            Attack();
        } else if (throwAmt == 3) //once cheddar throws three bombs, he charges
        {
            Charge();
        }
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        health -= damage;

        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills cheddar if health is 0
        }

        Stun(); //when cheddar is hit on the head, he is stunned
    }

    private void Death() //cheddar death
    {

        Instantiate(key, transform.position, Quaternion.identity); //spawns key
        Instantiate(cake, transform.position, Quaternion.identity); //spawns cake

        Destroy(this.gameObject); //destroys gameObject
    }

    private void Idle() //gets random position and sets it as cheddar's destination within a certain range of its starting position
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

        roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

        agent.SetDestination(roamPos); //sets cheddar's destination
    }

    private void Attack()
    {

    }

    private void Charge()
    {

    }

    private void Stun()
    {

    }
}
