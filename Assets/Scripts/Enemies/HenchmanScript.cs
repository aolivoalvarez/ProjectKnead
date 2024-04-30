/*-----------------------------------------
Creation Date: N/A
Author: alex
Description: 
-----------------------------------------*/

using System.Collections;
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
    [SerializeField] float attackRange; //range to player to be able to attack
    [SerializeField] float mAttackRange = 2f; //range for melee attack
    [SerializeField] float rAttackRange = 5f; //range for ranged attack
    [SerializeField] Transform player; //holds reference to player's transform
    [SerializeField] float roamDistMax = 50f; //holds maximum roaming distance from starting point
    [SerializeField] float roamDistMin = 20f; //holds minimum roaming distance from starting point
    [SerializeField] GameObject weapon; //holds reference to weapon prefab
    [SerializeField] GameObject weaponParent; //holds reference to weapon parent game object
    [SerializeField] float fireRate = 1f; //fire rate for ranged weapon
    [SerializeField, Range(0f, 1f), Tooltip("Percentage of incoming knockback this henchman takes. At 0, no knockback is taken.")]
    float knockbackMultiplier = 1f;
    [SerializeField] GameObject pSysDespawnPrefab;
    public bool isShielded { get; set; } = false;
    bool isInvincible;
    float nextFire; //for fire rate calculations
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;
    Vector2 startingPosition; //holds henchman's starting position
    public NavMeshAgent agent { get; private set; } //holds reference to henchman's navmesh agent
    
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
        isInvincible = false;

        //references to components
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //sets target and attack ranges based on attack type
        if (attackType == AttackType.Melee)
        {
            targetRange = mTargetRange;
            attackRange = mAttackRange;
        } else if (attackType == AttackType.Ranged)
        {
            targetRange = rTargetRange;
            attackRange = rAttackRange;
        }
    }

    void FixedUpdate()
    {
        animator.SetFloat("lookX", agent.velocity.x);
        animator.SetFloat("lookY", agent.velocity.y);
        
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(startingPosition);
            Roam(); //calls coroutine for henchman to roam in it's area

            if (Vector2.Distance(transform.position, player.position) <= targetRange && Vector2.Distance(transform.position, player.position) > attackRange) //checks if player is nearby
            {
                Chase(); //calls function for henchman to chase
            }

            if (Vector2.Distance(transform.position, player.position) <= attackRange) //checks if player is in attack range
            {
                AttackTarget(); //attacks player
            }
        }
    }

    public void TakeDamage(int damage) //takes damage + destroys gameObject when health <= 0
    {
        if (isInvincible)
            return;
        if (!isShielded)
        {
            health -= damage;
            StartCoroutine(InvincibleRoutine());
        }
        
        if (health <= 0) //checks if health is 0 or less
        {
            Death(); //kills henchman if health is 0
        }
    }
    public void TakeDamage(int damage, float knockbackStrength, Vector2 knockbackDirection) //override that applies knockback to this enemy
    {
        TakeDamage(damage);

        if (!isShielded)
            StartCoroutine(KnockbackRoutine(knockbackStrength, knockbackDirection));
    }

    IEnumerator KnockbackRoutine(float knockbackStrength, Vector2 knockbackDirection)
    {
        agent.enabled = false; //navmesh agent must be disabled for AddForce to work properly
        rigidBody.AddForce(100f * knockbackMultiplier * knockbackStrength * knockbackDirection.normalized);
        yield return new WaitForSeconds(0.5f);
        rigidBody.velocity = Vector2.zero;
        agent.enabled = true;
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        StartCoroutine(FlashingRoutine());
        yield return new WaitForSeconds(0.25f);
        isInvincible = false;
    }

    IEnumerator FlashingRoutine()
    {
        while (isInvincible)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Roam() //gets random position and sets it as henchman's destination within a certain range of its starting position
    {
        Vector2 roamPos = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized; //random vector

        roamPos = startingPosition + roamPos * Random.Range(roamDistMin, roamDistMax); //multiplies vector by random distance

        agent.SetDestination(roamPos); //sets henchman's destination
    }

    
    private void Chase() //moves henchman towards player
    {
        agent.SetDestination(player.position);
    }

    private void Death() //henchman death
    {
        Instantiate(pSysDespawnPrefab, new Vector3(transform.position.x, transform.position.y + GetComponent<SpriteRenderer>().bounds.extents.y * 0.5f,
            transform.position.z), Quaternion.identity);
        Destroy(gameObject); //destroys gameObject
    }

    private void AttackTarget() //attacks target once target is in attack range
    {
        if (animal == Animal.Beaver)
        {
            MeleeAttack();
        } else if (animal == Animal.Squirrel)
        {
            RangedAttack();
        }
        
        return;
    }


    private void MeleeAttack() //fuction for melee attack
    {
        PlayerController pController = player.GetComponent<PlayerController>();
        pController.DecreaseHealth(attackDamage, 1f, pController.transform.position - transform.position);
        return;
    }

    private void RangedAttack() //function for ranged attack
    {
        if (nextFire < Time.time)
        {
            Instantiate(weapon, weaponParent.transform.position, Quaternion.identity); //spawns bullet
            nextFire = Time.time + fireRate; //updates nextFire according to fireRate
        }

        return;
    }
}
