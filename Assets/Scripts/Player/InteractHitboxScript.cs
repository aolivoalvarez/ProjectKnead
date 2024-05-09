/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Attached to a child GameObject on the Player with a Collider2D trigger component. Determines whether the player can interact with an object.
-----------------------------------------*/

using UnityEngine;

public class InteractHitboxScript : MonoBehaviour
{
    public GameObject npcToTalk;
    public GameObject objectToLift;
    public GameObject objectToPush;
    public GameObject chestToOpen;
    public GameObject doorToUnlock;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            npcToTalk = other.gameObject;
        }
        else if (other.gameObject.CompareTag("LiftableObject") && other.GetComponent<Rigidbody2D>() == null) // can't lift an object that has already been thrown
        {
            objectToLift = other.gameObject;
        }
        else if (other.gameObject.CompareTag("TreasureChest") && !other.GetComponent<ChooseChestPickup>().chestOpened)
        {
            chestToOpen = other.gameObject;
        }

        if (other.gameObject.CompareTag("PushObject"))
        {
            objectToPush = other.gameObject;
        }
        
        if (other.gameObject.CompareTag("LockedDoor"))
        {
            doorToUnlock = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        npcToTalk = null;
        objectToLift = null;
        objectToPush = null;
        chestToOpen = null;
        doorToUnlock = null;
    }
}
