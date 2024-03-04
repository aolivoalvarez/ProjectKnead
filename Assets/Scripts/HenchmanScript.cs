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
    [SerializeField] float targetRange = 10f; //once player is in this range, henchman will pursue
    [SerializeField] float attackRange = 5f; //range to player to be able to attack
    [SerializeField] GameObject player; //holds reference to player
    [SerializeField] float roamDistMax = 30f;
    [SerializeField] float roamDistMin = 10f;
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
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        MoveToPosition(startingPosition, roamPosition);
;    }

    void Update()
    {
        //MoveToPosition(startingPosition, roamPosition);
        //roamPosition = RoamingPosition();
        
        //FindTarget();
        
        if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            AttackTarget();
            Debug.Log("in attack range");
        }

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
        Debug.Log(transform.position);
        transform.position = Vector2.Lerp(startPos, targetPos, Time.deltaTime);
        Debug.Log("move to positon");
        Debug.Log(startPos);
        Debug.Log(targetPos);
        Debug.Log(transform.position);
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
        return;
    }
}
