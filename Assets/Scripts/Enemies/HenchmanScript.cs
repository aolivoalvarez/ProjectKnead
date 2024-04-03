/*-----------------------------------------
Creation Date: N/A
Author: alex
Description: 
-----------------------------------------*/

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

    enum AttackType //if enemy is melee or ranged
    {
        Melee,
        Ranged
    }

    int health;
    [SerializeField] int maxHealth = 4;
    [SerializeField] int attackDamage = 2;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Group group; //what area the henchman belongs to
    [SerializeField] Animal animal; //what animal the henchman is <-- for special cases
    [SerializeField] AttackType attackType; //holds henchman's attack type
    [SerializeField] float targetRange; //once player is in this range, henchman will pursue
    [SerializeField] float mTargetRange = 5f; //short target range for melee attacks
    [SerializeField] float rTargetRange = 10f; //long target range for ranged attacks
    [SerializeField] float attackRange = 2f; //range to player to be able to attack
    [SerializeField] float roamDistMax = 10f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 5f; //holds minimum roaming distance from starting point
    Transform player; //holds reference to player's transform
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds henchman's starting position
    NavMeshAgent agent; //holds reference to henchman's navmesh agent
    
    void Start()
    {
        player = PlayerController.instance.transform;
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
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        //animator = this.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        if (attackType == AttackType.Melee)
        {
            targetRange = mTargetRange;
        }
        else if (attackType == AttackType.Ranged)
        {
            targetRange = rTargetRange;
        }
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
            Death(); //kills henchman if health is 0
        }
    }

    void Roam() //gets random position and sets it as henchman's destination within a certain range of its starting position
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

        roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

        agent.SetDestination(roamPos); //sets henchman's destination
    }

    
    void Chase() //moves henchman towards player
    {
        agent.SetDestination(player.position);
    }

    void Death() //henchman death
    {
        Destroy(gameObject); //destroys gameObject
    }

    void AttackTarget() //attacks target once target is in attack range
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        pController.DecreaseHealth(attackDamage);
        Debug.Log("attack target triggered");
        Debug.Log(attackDamage);
    }
}
