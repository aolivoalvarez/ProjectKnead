using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Group group;
    [SerializeField] Animal animal;
    [SerializeField] float targetRange = 5f; //once player is in this range, henchman will pursue
    [SerializeField] float attackRange = 2f; //range to player to be able to attack
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float roamDistMax = 10f;
    [SerializeField] float roamDistMin = 5f;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds henchman's starting position
    Vector2 roamPosition; //holds henchman's roaming position
    


    void Start()
    {
        startingPosition = transform.position; //gets henchman's starting position
        roamPosition = RoamingPosition(); //gets henchman's first roaming position
        

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

 
;    }

    void Update()
    {
        //MoveToPosition(startingPosition, roamPosition);
        //roamPosition = RoamingPosition();

        //FindTarget();

        //Debug.Log(Vector2.Distance(transform.position, player.position));
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            AttackTarget();
            Debug.Log("in attack range");
        }

    }

    private void FixedUpdate()
    {

        do
        {
            MoveToPosition(startingPosition, roamPosition);
            startingPosition = transform.position;
            roamPosition = RoamingPosition();
        } while (Vector2.Distance(transform.position, player.position) > targetRange);
    }


    void TakeDamage(int damage) //takes damage
    {
        health -= damage;
        return;
    }

    Vector2 RoamingPosition() //gets position for henchman to roam to while player is out of range
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

        return startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax);
    }

    void MoveToPosition(Vector2 startPos, Vector2 targetPos)
    {
        //Debug.Log(transform.position);
        targetPos = startPos - targetPos;
        rigidBody.MovePosition(startPos + (targetPos * moveSpeed * Time.deltaTime));
        //Debug.Log("move to positon");
        //Debug.Log(startPos);
        //Debug.Log(targetPos);
        //Debug.Log(transform.position);
        return;
    }
   
    
    void FindTarget()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < targetRange)
        {
            
        }

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
