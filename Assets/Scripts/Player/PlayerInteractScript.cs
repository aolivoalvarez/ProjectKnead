/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Handles the lifting and throwing of objects by the Player.
-----------------------------------------*/

using System.Collections;
using UnityEngine;

public class PlayerInteractScript : MonoBehaviour
{
    PlayerController playerController;
    InteractHitboxScript interactHitboxScript;

    [Header("Chest Opening")]
    [SerializeField, Tooltip("How long it takes to open a chest, in seconds.")]
    float openingWaitTime = 0.25f;
    [SerializeField, Tooltip("How long the chest item is held above Canela's head, in seconds.")]
    float holdItemWaitTime = 2f;
    [SerializeField, Tooltip("How high above the player the chest item is held.")]
    float chestItemOffsetY = 2f;

    [Header("Lifting")]
    [SerializeField, Tooltip("How long it takes to lift an object, in seconds.")]
    float liftTime = 1f;
    [SerializeField, Tooltip("How high above the player an object is held.")]
    float liftOffsetY = 2f;
    GameObject liftedObject;

    [Header("Throwing")]
    [SerializeField, Tooltip("The shadow prefab that's added as a child object to better show where the thrown object is.")]
    GameObject shadow;
    [SerializeField]
    float throwForce = 10f;
    [SerializeField, Tooltip("Maximum time before a thrown object is destroyed, in seconds.")]
    float thrownObjectLifetime = 5f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        interactHitboxScript = GetComponentInChildren<InteractHitboxScript>();
    }

    void Update()
    {
        playerController.isHoldingObject = liftedObject != null;

        if (playerController.pInput.Player.Interact.triggered && !playerController.isAttacking &&
            !playerController.isJumping && !playerController.isShielding)
        {
            // Open chest
            if (interactHitboxScript.chestToOpen != null && !playerController.isHoldingObject)
                StartCoroutine(OpenChestRoutine(interactHitboxScript.chestToOpen.GetComponent<ChooseChestPickup>().OpenChest()));
            // Lift object
            else if (interactHitboxScript.objectToLift != null)
                StartCoroutine(LiftRoutine());
            // Throw object
            else if (liftedObject != null && !playerController.isLifting)
            {
                liftedObject.transform.SetParent(null);
                liftedObject.GetComponent<ThrowableObjectScript>().isThrown = true;
                liftedObject.GetComponentInChildren<Animator>().SetTrigger("ObjectFall"); // trigger the animation to make the object appear like it's falling
                Instantiate(shadow, liftedObject.transform).transform.SetParent(liftedObject.transform); // give the object a shadow for better readability

                liftedObject.AddComponent<Rigidbody2D>();
                liftedObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                liftedObject.GetComponent<Rigidbody2D>().freezeRotation = true;
                liftedObject.GetComponent<Rigidbody2D>().AddForce(playerController.simpleLookDirection * throwForce * 50);

                //Destroy(liftedObject, thrownObjectLifetime);
                liftedObject = null;
            }
        }
    }

    IEnumerator LiftRoutine()
    {
        playerController.isLifting = true;
        liftedObject = interactHitboxScript.objectToLift;
        yield return new WaitForSeconds(liftTime);
        playerController.isLifting = false;

        liftedObject.transform.position = playerController.transform.position; // actual position of object and its collider will be the same as the player's position
        liftedObject.transform.SetParent(playerController.transform);
        liftedObject.layer = LayerMask.NameToLayer("ThrownObject"); // layer that does not collide with the player
        liftedObject.GetComponent<Collider2D>().isTrigger = true; // object no longer needs solid collision now that the player's holding it

        // shrink the collider size to be smaller than the player's
        liftedObject.GetComponent<BoxCollider2D>().size = new Vector2(playerController.GetComponent<BoxCollider2D>().size.x - 0.02f,
                                                                      playerController.GetComponent<BoxCollider2D>().size.y - 0.02f); 
        liftedObject.GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0.25f);

        GameObject graphic = liftedObject.GetComponentInChildren<SpriteRenderer>().gameObject;
        graphic.transform.position = new Vector2(playerController.transform.position.x, playerController.transform.position.y + liftOffsetY); // object's sprite will appear above the player
        graphic.GetComponent<SpriteRenderer>().sortingLayerName = "AboveEntity";
        graphic.GetComponent<Animator>().enabled = true;
    }

    IEnumerator OpenChestRoutine(GameObject itemFromChest)
    {
        GameManager.instance.DisablePlayerInput();
        yield return new WaitForSeconds(openingWaitTime);
        var heldItem = Instantiate(itemFromChest, new Vector3(transform.position.x, transform.position.y + chestItemOffsetY, transform.position.z), Quaternion.identity);
        heldItem.GetComponent<Collider2D>().enabled = false; // items from chests should not be tangible to the player
        heldItem.GetComponent<SpriteRenderer>().sortingLayerName = "AboveEntity";
        heldItem.GetComponent<Pickup>().autoDespawn = false;
        heldItem.GetComponent<Pickup>().StopAllCoroutines();
        heldItem.GetComponent<Pickup>().PlayerCollectDontDestroy();
        yield return new WaitForSeconds(holdItemWaitTime);
        Destroy(heldItem);
        GameManager.instance.EnablePlayerInput();
    }
}
