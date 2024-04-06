/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: The main script for the Player object. Controls player input, movement, most animations, and variables like health and money.
-----------------------------------------*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public PlayerInput pInput { get; private set; }

    //---------------------- Misc ----------------------//
    public int maxHealth = 12;
    public int health { get; private set; }
    public int money { get; private set; }
    [SerializeField, Tooltip("After taking damage, how long the player is invincible, in seconds.")]
    float invincibilityTime;
    [SerializeField] Transform graphic;
    [SerializeField] Transform liftRotationPoint;
    public Rigidbody2D rigidBody { get; private set; }
    public Animator animator;
    public bool isAttacking { get; set; }
    public bool isShielding { get; set; }
    public bool isLifting { get; set; }
    public bool isHoldingObject { get; set; }
    bool isInvincible;

    public bool bossKeyCollected;

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

        inventory = Inventory.instance;
    }

    void Start()
    {
        health = maxHealth;
        money = 0;
        isAttacking = false;
        isShielding = false;
        isLifting = false;
        isHoldingObject = false;
        isInvincible = false;
        moveSpeedMult = 1f;
        inputDirection = Vector2.zero;
        lookDirection = Vector2.down;
        simpleLookDirection = Vector2.down;
        initialGraphicPositionY = graphic.transform.localPosition.y;
        isJumping = false;
        canJump = true;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = graphic.gameObject.GetComponent<Animator>();
        bossKeyCollected = false;

        InitializePlayerInput();
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

        if (inputDirection != Vector2.zero && !isShielding && !(isAttacking && isJumping)) // a jumping attack forces you to look in just one direction
        {
            lookDirection = new Vector2(Mathf.Round(inputDirection.normalized.x), Mathf.Round(inputDirection.normalized.y));
        }
        animator.SetFloat("Speed", inputDirection.magnitude);
        animator.SetFloat("Look X", simpleLookDirection.x);
        animator.SetFloat("Look Y", simpleLookDirection.y);
        animator.SetBool("IsJumping", isJumping);
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
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, -90);
                break;
            case 1.0f:
                simpleLookDirection = Vector2.right;
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, 90);
                break;
            default:
                break;
        }
        switch (lookDirection.y)
        {
            case -1.0f:
                simpleLookDirection = Vector2.down;
                liftRotationPoint.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 1.0f:
                simpleLookDirection = Vector2.up;
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
        GameManager.instance.UpdatePlayerHearts();
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
        GameManager.instance.UpdatePlayerHearts();
    }

    public void IncreaseMoney(int amount)
    {
        money = (money + amount < 999) ? money + amount : 999;
        GameManager.instance.UpdatePlayerMoney();
    }

    public void DecreaseMoney(int amount)
    {
        money = (money - amount > 0) ? money - amount : 0;
        GameManager.instance.UpdatePlayerMoney();
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

    void InitializePlayerInput()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        pInput.Player.Item.started +=
            _ => GetComponent<PlayerUseItemScript>().UseItem_Hold();
        pInput.Player.Item.performed +=
            context =>
            {
                if (context.interaction is SlowTapInteraction)
                    GetComponent<PlayerUseItemScript>().UseItem_Release();
            };
        pInput.Player.Item.canceled +=
            context =>
            {
                if (context.interaction is SlowTapInteraction)
                    GetComponent<PlayerUseItemScript>().UseItem_EarlyRelease();
            };
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

    //Dylan BossKey Code
    public void OnTriggerEnter2D(Collider2D other){
        if((other.tag == "Key")){
            bossKeyCollected = true;
            Destroy(other.gameObject);
            Debug.Log("Key Collected");

        }
    }
}