/*-----------------------------------------
Creation Date: 04/04/24
Author: Alex Olivo
Description: Cheddar Boss AI
-----------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CheddarScript : MonoBehaviour
{
    enum State //cheddar's states
    {
        Idle,
        Attack,
        Charge,
        Stun,
        Reset
    }

    int health;
    [SerializeField] int maxHealth = 10;
    [SerializeField] int attackDamage = 4;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float attackRange = 10f; //once player is in this range, cheddar will begin throwing
    [SerializeField] float roamDistMax = 10f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 5f; //holds minimum roaming distance from starting point
    [SerializeField] GameObject bombPrefab; //holds reference to weapon prefab
    [SerializeField] GameObject bombParent; //holds reference to weapon parent game object
    [SerializeField] GameObject keyPrefab; //dungeon key that cheddar drops
    [SerializeField] GameObject cakePrefab; //cake slice that cheddar drops
    [SerializeField] State state; //holds cheddar's state
    int throwAmt; //amount of times cheddar has thrown a bomb
    [SerializeField] int throwRate = 3; //rate that cheddar throws bombs
    int attackBuffer = 2;
    float chargeSpeed = 5f; //speed of cheddar's charge
    int chargeWait = 3; //wait until charge coroutine calls idle coroutine
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
        throwAmt = 0; //sets throw amount to 0

        //references to components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        StartCoroutine(IdleRoutine()); //calls idle coroutine upon start
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && state == State.Charge)
        {
            collision.gameObject.GetComponent<PlayerController>().DecreaseHealth(attackDamage);

            return;
        }
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        if (state == State.Charge) { return; }
        
        health -= damage;

        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills cheddar if health is 0
        }

        GetStunned(); //when cheddar is hit on the head, he is stunned
      
    }

    private void Death() //cheddar death
    {

        Instantiate(keyPrefab, transform.position, Quaternion.identity); //spawns key
        Instantiate(cakePrefab, transform.position, Quaternion.identity); //spawns cake

        Destroy(this.gameObject); //destroys gameObject
    }

    IEnumerator IdleRoutine()
    {
        while (true) //looks scary, but it's fine. As long as it has the yield return new WaitForFixedUpdate(), there will never be an infinite loop error
        {
            agent.speed = moveSpeed; //resets move speed if called after charge

            if (Vector2.Distance(transform.position, player.position) <= attackRange) //checks if player is in range
            {
                break; //breaks from idle loop
            }

            state = State.Idle;

            Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

            roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

            agent.SetDestination(roamPos); //sets cheddar's destination

            yield return new WaitForFixedUpdate();
        }
        throwAmt = 0; //resets throw amount to 0
        StartCoroutine(AttackRoutine()); //calls attack routine
    }

    IEnumerator AttackRoutine() //cheddar attack routine
    {
        state = State.Attack;

        throwAmt++; //increases throw amount
        Vector2 bombMovement = (player.position - transform.position).normalized;
        GameObject thisBomb = Instantiate(bombPrefab, bombParent.transform.position, Quaternion.identity); //spawns bomb
        thisBomb.GetComponent<BombScript>().BombStartMoving(bombMovement); //makes the bomb move towards player
        thisBomb.GetComponentInChildren<Animator>().SetFloat("Direction X", bombMovement.x );
        thisBomb.GetComponentInChildren<Animator>().SetFloat("Direction Y", bombMovement.y );


        yield return new WaitForSeconds(throwRate);

        if (throwAmt >= 3) //if cheddar has thrown three times, the charge starts
        {   
            StartCoroutine(ChargeRoutine());
        }
        else //if he hasn't, he attacks again
        {
            StartCoroutine(AttackRoutine());
        }

    }

    IEnumerator ChargeRoutine()
    {

        state = State.Charge;

        agent.speed = chargeSpeed; //increases cheddar's speed
        agent.SetDestination(player.position); //sets cheddar's destination to player

        yield return new WaitForSeconds(chargeWait); //waits a little bit before calling idle routine

        StartCoroutine(IdleRoutine());

    }

    public void GetStunned() //starts the process of stunning cheddar
    {
        StopAllCoroutines(); //stops all the coroutines
        StartCoroutine(StunRoutine()); //starts cheddar's stun routine
    }

    IEnumerator StunRoutine() //cheddar's stun routine
    {
        state = State.Stun;

        //stun stuff here:

        yield return new WaitForSeconds(stunTimer); //waits for how long cheddar is stunned for

        StartCoroutine(IdleRoutine()); //starts idle routine
    }
}
