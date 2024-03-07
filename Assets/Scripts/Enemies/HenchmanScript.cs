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
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] Group group; //what area the henchman belongs to
    [SerializeField] Animal animal; //what animal the henchman is <-- for special cases
    [SerializeField] float targetRange = 5f; //once player is in this range, henchman will pursue
    [SerializeField] float attackRange = 2f; //range to player to be able to attack
    Transform player; //holds reference to player's transform
   // [SerializeField] float roamDistMax = 10f;
   // [SerializeField] float roamDistMin = 5f;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds henchman's starting position
    //Vector2 roamPosition; //holds henchman's roaming position
    


    void Start()
    {
        startingPosition = transform.position; //gets henchman's starting position
        //roamPosition = RoamingPosition(); //gets henchman's first roaming position
        player = PlayerController.instance.gameObject.transform;
        player = null; // DELETE THIS AFTER MVP
        

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


    private void FixedUpdate()
    {

        FindPlayer(); //finds where player is + checks if player is in chasing range
        
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
            Destroy(gameObject); //destroys gameObject
        }
        return;
    }

    /*Vector2 RoamingPosition() //gets position for henchman to roam to while player is out of range
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;

        return startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax);
    } */

    void FindPlayer() //checks if player is in chasing range and follows them if they are
    {
        if (Vector2.Distance(transform.position, player.position) <= targetRange)
        {
            MoveToPlayer();
        }

        return;
    }
    
    void MoveToPlayer() //moves henchman towards player
    {
        Vector2 movement = player.position - transform.position;
        movement.Normalize();

        rigidBody.MovePosition((Vector2)transform.position + (movement * moveSpeed * Time.deltaTime));
        
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
