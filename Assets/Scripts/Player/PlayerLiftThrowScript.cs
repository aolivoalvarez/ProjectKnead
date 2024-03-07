using System.Collections;
using UnityEngine;

public class PlayerLiftThrowScript : MonoBehaviour
{
    PlayerController playerController;
    LiftHitboxScript liftHitboxScript;

    [Header("Lifting")]
    [SerializeField, Tooltip("How long it takes to lift an object, in seconds.")]
    float liftTime = 1f;
    [SerializeField, Tooltip("How high above the player an object is held.")]
    float liftOffsetY = 2f;
    GameObject liftedObject;

    [Header("Throwing")]
    [SerializeField]
    float throwForce = 10f;
    [SerializeField, Tooltip("Maximum time before a thrown object is destroyed, in seconds.")]
    float thrownObjectLifetime = 5f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        liftHitboxScript = GetComponentInChildren<LiftHitboxScript>();
    }

    void Update()
    {
        playerController.isHoldingObject = liftedObject != null;

        if (playerController.pInput.Player.Interact.triggered)
        {
            // Lift
            if (liftHitboxScript.objectToLift != null && !playerController.isHoldingObject && !playerController.isAttacking && !playerController.isJumping)
                StartCoroutine(LiftRoutine());
            // Throw
            else if (liftedObject != null && !playerController.isLifting)
            {
                liftedObject.transform.SetParent(null);
                liftedObject.GetComponent<Collider2D>().enabled = true; // when an object is thrown, its collider is turned back on
                liftedObject.GetComponent<Collider2D>().isTrigger = true; // collider becomes trigger that only hurts enemies
                liftedObject.GetComponent<ThrowableObjectScript>().isLifted = true;
                liftedObject.AddComponent<Rigidbody2D>();

                // if throwing up or down, turn thrown object's gravity off
                switch (playerController.simpleLookDirection.y)
                {
                    case -1.0f:
                    case 1.0f:
                        liftedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                        break;
                    default:
                        break;
                }

                liftedObject.GetComponent<Rigidbody2D>().freezeRotation = true;
                liftedObject.GetComponent<Rigidbody2D>().AddForce(playerController.simpleLookDirection * throwForce * 50);

                Destroy(liftedObject, thrownObjectLifetime);
                liftedObject = null;
            }
        }
    }

    IEnumerator LiftRoutine()
    {
        playerController.isLifting = true;
        liftedObject = liftHitboxScript.objectToLift;
        yield return new WaitForSeconds(liftTime);
        playerController.isLifting = false;

        liftedObject.transform.position = new Vector2(playerController.transform.position.x, playerController.transform.position.y + liftOffsetY);
        liftedObject.transform.SetParent(playerController.transform);
        liftedObject.GetComponent<Collider2D>().enabled = false; // while an object is held, its collider is turned off
        liftedObject.GetComponent<SpriteRenderer>().sortingLayerName = "AboveCharacter";
    }
}
