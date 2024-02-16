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
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float jumpTime = 1f; // how many seconds it takes to land
    [SerializeField] Vector2 inputDirection;
    Vector2 roughPosition; // keeps track of what the transform.position would be if it wasn't being locked to a unit/pixel grid

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
        boxCollider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        //------------------ Take Input --------------------//
        inputDirection = pInput.Player.Movement.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        roughPosition = new Vector2(roughPosition.x + inputDirection.x * moveSpeed * Time.deltaTime, 
            roughPosition.y + inputDirection.y * moveSpeed * Time.deltaTime);
        transform.position = new Vector2(Mathf.Floor(roughPosition.x), Mathf.Floor(roughPosition.y)); // locks position to a unit/pixel grid
    }
}
