/*-----------------------------------------
Creation Date: N/A
Author: alex
Description: 
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HenchmanScript : MonoBehaviour
{
    enum Group //what group the henchman belongs to
    {
        AreaOne,
        AreaTwo,
        AreaThree
    }

    enum Animal //what animal the henchman is
    {
        Beaver,
        Hedgehog,
        Squirrel,
        Snake,
        Toad,
        Duck
    }


    int health;
    [SerializeField] int maxHealth = 4;
    [SerializeField] int attackDamage = 2;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Group group; //what area the henchman belongs to
    [SerializeField] Animal animal; //what animal the henchman is <-- for special cases
    [SerializeField] float targetRange = 5f; //once player is in this range, henchman will pursue
    [SerializeField] float attackRange = 2f; //range to player to be able to attack
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float roamDistMax = 10f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 5f; //holds minimum roaming distance from starting point
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds henchman's starting position
    NavMeshAgent agent; //holds reference to henchman's navmesh agent
    


    void Start()
    {
        startingPosition = transform.position; //gets henchman's starting position

        switch (group) //sets max health + attack damage according to group
        {
            case Group.AreaOne:
                {
                    maxHealth = 4;
                    attackDamage = 2;
                    break;
                }
            case Group.AreaTwo:
                {
                    maxHealth = 8;
                    attackDamage = 4;
                    break;
                }
            case Group.AreaThree:
                {
                    maxHealth = 12;
                    attackDamage = 8;
                    break;
                }
            default:
                {
                    Debug.Log("no area set");
                    break;
                }
        }

        health = maxHealth; //sets health to max

        //references to components
        boxCollider = this.GetComponent<BoxCollider2D>();
        rigidBody = this.GetComponent<Rigidbody2D>();
        //animator = this.GetComponent.<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


       }


    private void FixedUpdate()
    {
        Roam(); //calls function for henchman to roam in it's area

        if (Vector2.Distance(transform.position, player.position) <= targetRange) //checks if player is nearby
        {
            Chase(); //calls function for henchman to chase
        }

        if (Vector2.Distance(transform.position, player.position) <= attackRange) //checks if player is in attack range
        {
            AttackTarget(); //attacks player
        }
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        health -= damage;
        
        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills hencham if health is 0
        }

        return;
    }

    void Roam() //gets random position and sets it as henchman's destination within a certain range of its starting position
    {
        
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

        roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

        agent.SetDestination(roamPos); //sets henchman's destination

        return;
    }

    
    void Chase() //moves henchman towards player
    {

        agent.SetDestination(player.position);
        
        return;
    }

    void Death() //henchman death
    {
        Destroy(this.gameObject); //destroys gameObject

        return;
    }
   
    

    void AttackTarget() //attacks target once target is in attack range
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        pController.DecreaseHealth(attackDamage);
        Debug.Log("attack target triggered");
        Debug.Log(attackDamage);
        
        return;
    }
}
