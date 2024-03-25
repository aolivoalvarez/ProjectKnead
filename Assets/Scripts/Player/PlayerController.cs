/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: The main script for the Player object. Controls player input, movement, most animations, and variables like health and money.
-----------------------------------------*/

using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerInput pInput { get; private set; }

    //---------------------- Misc ----------------------//
    public int maxHealth = 12;
    public int health { get; private set; }
    [SerializeField] int money;
    [SerializeField, Tooltip("After taking damage, how long the player is invincible, in seconds.")]
    float invincibilityTime;
    [SerializeField] Transform graphic;
    [SerializeField] Transform attackRotationPoint;
    [SerializeField] Transform liftRotationPoint;
    public Rigidbody2D rigidBody { get; private set; }
    Animator animator;
    public bool isAttacking { get; set; }
    public bool isLifting { get; set; }
    public bool isHoldingObject { get; set; }
    bool isInvincible;

    Inventory inventory;
    //--------------------------------------------------//

    [Header("Movement")]
    [SerializeField, Tooltip("Speed in units per second.")]
    float moveSpeed = 5f;
    public float moveSpeedMult { get; set; } = 1f; // will normally be 1, but various hazards can modify it to lower the player's speed
    [SerializeField] Vector2 inputDirection;
    public Vector2 lookDirection { get; private set; } // keeps track of the direction the player last moved (for the animator)
    public Vector2 simpleLookDirection { get; private set; } // reduces lookDirection to just the 4 cardinal directions

    [Header("Jumping")]
    [SerializeField, Tooltip("How many units high one jump is. Purely visual.")]
    float jumpHeight = 0.5f;
    [SerializeField, Tooltip("How long it takes to land, in seconds.")]
    float jumpTime = 1f;
    [SerializeField, Tooltip("Buffer after every jump, in seconds.")]
    float jumpBuffer = 0.05f;
    public bool isJumping { get; private set; }
    public bool canJump { get; set; }
    float initialGraphicPositionY; // stores the initial value of graphic.position.y (for when the player is not jumping)

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//

        inventory = new Inventory();
    }

    void Start()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        health = maxHealth;
        money = 0;
        isAttacking = false;
        isLifting = false;
        isHoldingObject = false;
        isInvincible = false;
        moveSpeedMult = 1f;
        inputDirection = Vector2.zero;
        lookDirection = Vector2.down;
        simpleLookDirection = Vector2.down;
        initialGraphicPositionY = graphic.transform.position.y;
        isJumping = false;
        canJump = true;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //------------------ Take Input --------------------//
        inputDirection = ((isAttacking && !isJumping) || isLifting) ? Vector2.zero : pInput.Player.Movement.ReadValue<Vector2>(); // takes input unless lifting an object or attacking while grounded

        if (pInput.Player.Jump.triggered)
        {
            if (canJump && !isJumping && !isAttacking && !isHoldingObject)
                StartCoroutine(JumpRoutine());
        }
        //--------------------------------------------------//

        if (inputDirection != Vector2.zero && !(isAttacking && isJumping)) // a jumping attack forces you to look in just one direction
        {
            lookDirection = new Vector2(Mathf.Round(inputDirection.normalized.x), Mathf.Round(inputDirection.normalized.y));
        }
        animator.SetFloat("Speed", inputDirection.magnitude);
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetBool("isJumping", isJumping);
    }

    void FixedUpdate()
    {
        //---------------- Set final values ----------------//
        rigidBody.velocity = new Vector2(inputDirection.x * moveSpeed * moveSpeedMult * Time.fixedDeltaTime * 50,
            inputDirection.y * moveSpeed * moveSpeedMult * Time.fixedDeltaTime * 50);
        
        switch (lookDirection.x)
        {
            case -1.0f:
                simpleLookDirection = Vector2.left;
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, -90);
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, -90);
                break;
            case 1.0f:
                simpleLookDirection = Vector2.right;
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 90);
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, 90);
                break;
            default:
                break;
        }
        switch (lookDirection.y)
        {
            case -1.0f:
                simpleLookDirection = Vector2.down;
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 0);
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 1.0f:
                simpleLookDirection = Vector2.up;
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 180);
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, 180);
                break;
            default:
                break;
        }
    }

    IEnumerator JumpRoutine()
    {
        isJumping = true;
        canJump = false;
        float jumpArc = .55f; // the percentage of jumpTime spent ascending
        float fallArc = .45f; // the percentage of jumpTime spent decending
        for (float i = 0; i < jumpTime * jumpArc; i += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            graphic.localPosition = new Vector3(graphic.localPosition.x, Mathf.Lerp(initialGraphicPositionY, initialGraphicPositionY + jumpHeight, i / (jumpTime * jumpArc)));
        }
        for (float i = 0; i < jumpTime * fallArc; i += Time.fixedDeltaTime)
        {
            yield return new WaitForFixedUpdate();
            graphic.localPosition = new Vector3(graphic.localPosition.x, Mathf.Lerp(initialGraphicPositionY + jumpHeight, initialGraphicPositionY, i / (jumpTime * fallArc)));
        }
        graphic.localPosition = new Vector3(graphic.localPosition.x, initialGraphicPositionY);
        isJumping = false;
        yield return new WaitForSeconds(jumpBuffer);
        canJump = true;
    }

    public void IncreaseHealth(int healthToGain)
    {
        health = (health + healthToGain < maxHealth) ? health + healthToGain : maxHealth;
    }

    public void DecreaseHealth(int healthToLose)
    {
        if (!isInvincible)
        {
            health -= healthToLose;
            GameManager.instance.UpdatePlayerHearts();
            StartCoroutine(InvincibleRoutine());
        }
        if (health <= 0)
        {
            GameManager.instance.GameOverSequence();
        }
    }

    public void HealthToMax()
    {
        health = maxHealth;
    }

    public void IncreaseMoney(int amount)
    {
        money = (money + amount < 999) ? money + amount : 999;
    }

    public void DecreaseMoney(int amount)
    {
        money = (money - amount > 0) ? money - amount : 0;
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        StartCoroutine(FlashingRoutine());
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    IEnumerator FlashingRoutine()
    {
        while (isInvincible)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            graphic.GetComponent<SpriteRenderer>().color = Color.clear;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            graphic.GetComponent<SpriteRenderer>().color = Color.white;
        }
        graphic.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // For when the player dies. Stops all coroutines and sets various values to default.
    public void EndPlayerCoroutines()
    {
        StopAllCoroutines();
        isAttacking = false;
        isLifting = false;
        isHoldingObject = false;
        isInvincible = false;
        moveSpeedMult = 1f;
        graphic.localPosition = new Vector3(graphic.localPosition.x, initialGraphicPositionY);
        isJumping = false;
        canJump = true;
        graphic.GetComponent<SpriteRenderer>().color = Color.white;
    }
}