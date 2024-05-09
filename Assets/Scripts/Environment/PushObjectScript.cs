/*-----------------------------------------
Creation Date: 4/10/2024 3:06:08 PM
Author: theco
Description: For objects that can be pushed by the player.
-----------------------------------------*/

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class PushObjectScript : MonoBehaviour
{
    [SerializeField] float pushSpeed = 1f;
    [SerializeField] float giveUpTime = 1f;
    public bool isPushing { get; private set; }

    void Start()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        isPushing = false;
    }

    public void PushObject(Vector2 direction)
    {
        Vector2 trueDirection;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0) trueDirection = Vector2.left;
            else trueDirection = Vector2.right;
        }
        else
        {
            if (direction.y < 0) trueDirection = Vector2.down;
            else trueDirection = Vector2.up;
        }

        StartCoroutine(PushRoutine(trueDirection));
    }

    IEnumerator PushRoutine(Vector2 direction)
    {
        isPushing = true;
        PlayerController.instance.isPushingObject = true;
        Vector2 initialPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 targetPosition = initialPosition + direction;
        targetPosition = new Vector2(Mathf.Round(targetPosition.x), Mathf.Round(targetPosition.y));
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.bodyType = RigidbodyType2D.Dynamic;

        GameManager.instance.DisablePlayerInput();
        rigidbody.velocity = 50f * pushSpeed * Time.fixedDeltaTime * direction;
        PlayerController.instance.rigidBody.velocity = rigidbody.velocity;
        AudioManager.instance.PlaySound(28);

        float timer = 0f;
        while (((Vector2)transform.position - targetPosition).magnitude > 0.05f)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

            if (timer >= giveUpTime)
            {
                Debug.Log("Block push failed.");
                break;
            }
        }

        rigidbody.velocity = Vector2.zero;
        rigidbody.bodyType = RigidbodyType2D.Static;
        isPushing = false;
        PlayerController.instance.rigidBody.velocity = Vector2.zero;
        PlayerController.instance.isPushingObject = false;
        GameManager.instance.EnablePlayerInput();
        if (timer < giveUpTime)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position = initialPosition;
        }
    }
}
