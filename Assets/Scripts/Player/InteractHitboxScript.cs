/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to a child GameObject on the Player with a Collider2D trigger component. Determines whether the player can interact with an object.
-----------------------------------------*/

using UnityEngine;

public class InteractHitboxScript : MonoBehaviour
{
    public GameObject objectToLift;
    public GameObject chestToOpen;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("LiftableObject") && other.GetComponent<Rigidbody2D>() == null) // can't lift an object that has already been thrown
        {
            objectToLift = other.gameObject;
        }
        else if (other.gameObject.CompareTag("TreasureChest") && !other.GetComponent<ChooseChestPickup>().chestOpened)
        {
            chestToOpen = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        objectToLift = null;
        chestToOpen = null;
    }
}
