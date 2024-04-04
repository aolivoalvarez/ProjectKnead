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
    enum State //cheddar's activity state
    {
        Idle,
        Throw,
        Charge,
        Stun
    }

    int health;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int attackDamage = 4;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] State state; //what state cheddar is in
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float attackRange; //once player is in this range, cheddar will begin throwing
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
        state = State.Idle;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        health -= damage;

        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills cheddar if health is 0
        }
    }

    private void Death() //henchman death
    {

        Instantiate(key, transform.position, Quaternion.identity);
        Instantiate(cake, transform.position, Quaternion.identity);

        Destroy(this.gameObject); //destroys gameObject
    }
}
