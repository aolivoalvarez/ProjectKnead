using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput pInput { get; private set; }

    [SerializeField] int maxHealth = 12;
    [SerializeField] int health;
    [SerializeField] int money;
    [SerializeField] Transform graphic;
    [SerializeField] Transform attackRotationPoint;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Vector2 inputDirection;
    public Vector2 roughPosition; // keeps track of what the transform.position would be if it wasn't being locked to a unit/pixel grid
    Vector2 lookDirection; // keeps track of the direction Canela last moved (for the animator)

    [Header("Jumping")]
    [SerializeField, Tooltip("How many pixels high one jump is.")]
    float jumpHeight = 15f;
    [SerializeField, Tooltip("How many frames it takes to land.")]
    int jumpTime = 60;
    [SerializeField, Tooltip("Buffer after every jump, in seconds.")]
    float jumpBuffer = 0.5f;
    public bool isJumping { get; private set; }
    bool canJump;
    float roughJumpPosition; // keeps track of what the graphic.position.y would be if it wasn't being locked to a unit/pixel grid
    float initialGraphicPositionY; // stores the initial value of graphic.position.y (for when the player is not jumping)

    void Start()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        health = maxHealth;
        money = 0;
        inputDirection = Vector2.zero;
        lookDirection = Vector2.down;
        roughPosition = new Vector2(transform.position.x, transform.position.y);
        roughJumpPosition = 0;
        initialGraphicPositionY = graphic.transform.position.y;
        isJumping = false;
        canJump = true;
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //------------------ Take Input --------------------//
        inputDirection = pInput.Player.Movement.ReadValue<Vector2>();

        if (pInput.Player.Jump.triggered)
        {
            if (canJump)
                StartCoroutine(JumpRoutine());
        }
        //--------------------------------------------------//

        if (inputDirection != Vector2.zero)
        {
            lookDirection = new Vector2(Mathf.Round(inputDirection.normalized.x), Mathf.Round(inputDirection.normalized.y));
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
    }

    void FixedUpdate()
    {
        roughPosition = new Vector2(roughPosition.x + inputDirection.x * moveSpeed * Time.deltaTime,
            roughPosition.y + inputDirection.y * moveSpeed * Time.deltaTime);

        //---------------- Set final transform -------------//
        transform.position = new Vector2(Mathf.Floor(roughPosition.x), Mathf.Floor(roughPosition.y)); // locks position to a unit/pixel grid
        graphic.localPosition = new Vector2(graphic.localPosition.x, initialGraphicPositionY + Mathf.Floor(roughJumpPosition)); // locks position of the graphic to a unit/pixel grid
        switch (lookDirection.x)
        {
            case -1.0f:
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, -90);
                break;
            case 1.0f:
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 90);
                break;
            default:
                break;
        }
        switch (lookDirection.y)
        {
            case -1.0f:
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case 1.0f:
                attackRotationPoint.localEulerAngles = new Vector3(0, 0, 180);
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
        for (int i = 0; i < Mathf.FloorToInt(jumpTime * jumpArc); i++)
        {
            yield return new WaitForFixedUpdate();
            roughJumpPosition += jumpHeight / Mathf.FloorToInt(jumpTime * jumpArc);
        }
        for (int i = 0; i < Mathf.FloorToInt(jumpTime * fallArc); i++)
        {
            yield return new WaitForFixedUpdate();
            roughJumpPosition -= jumpHeight / Mathf.FloorToInt(jumpTime * fallArc);
        }
        isJumping = false;
        yield return new WaitForSeconds(jumpBuffer);
        canJump = true;
    }

    public void DecreaseHealth(int healthToLose)
    {
        health -= healthToLose;
    }
}