using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInput pInput;

    [SerializeField] int maxHealth = 12;
    [SerializeField] int health;
    [SerializeField] int money;
    BoxCollider2D boxCollider;
    Rigidbody2D rigidBody;
    Animator animator;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 15f;
    [SerializeField] int jumpTime = 60; // how many frames it takes to land
    [SerializeField] Vector2 inputDirection;
    [SerializeField] Transform graphic;
    Vector2 roughPosition; // keeps track of what the transform.position would be if it wasn't being locked to a unit/pixel grid
    float roughJumpPosition; // keeps track of what the graphic.position.y would be if it wasn't being locked to a unit/pixel grid
    bool isJumping;

    [Header("Combat")]
    [SerializeField] float swordSwingTime = 0.75f; // how many seconds the swing takes

    void Start()
    {
        pInput = new PlayerInput();
        pInput.Enable();
        health = maxHealth;
        money = 0;
        inputDirection = Vector2.zero;
        roughPosition = new Vector2(transform.position.x, transform.position.y);
        roughJumpPosition = 0;
        isJumping = false;
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
            if (!isJumping)
                StartCoroutine(JumpRoutine());
        }
        //--------------------------------------------------//
    }

    void FixedUpdate()
    {
        roughPosition = new Vector2(roughPosition.x + inputDirection.x * moveSpeed * Time.deltaTime,
            roughPosition.y + inputDirection.y * moveSpeed * Time.deltaTime);
        transform.position = new Vector2(Mathf.Floor(roughPosition.x), Mathf.Floor(roughPosition.y)); // locks position to a unit/pixel grid
        graphic.localPosition = new Vector2(0, Mathf.Floor(roughJumpPosition)); // locks position of the graphic to a unit/pixel grid
    }

    IEnumerator JumpRoutine()
    {
        isJumping = true;
        for (int i = 0; i < jumpTime; i++)
        {
            yield return new WaitForFixedUpdate();
            roughJumpPosition += jumpHeight / (jumpTime * .5f);
        }
        for (int i = 0; i < jumpTime; i++)
        {
            yield return new WaitForFixedUpdate();
            roughJumpPosition -= jumpHeight / (jumpTime * .5f);
        }
        isJumping = false;
    }
}