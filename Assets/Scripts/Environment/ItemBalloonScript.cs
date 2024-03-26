/*-----------------------------------------
Creation Date: 3/17/2024 8:26:02 PM
Author: theco
Description: Similar to BreakableObjectScript, but for an object that contains an item and can only be destroyed while jumping.
-----------------------------------------*/

using UnityEngine;

public class ItemBalloonScript : MonoBehaviour
{
    [SerializeField] GameObject itemToDrop;
    [SerializeField] GameObject fallingItemPrefab;
    GameObject storedItem;

    void Start()
    {
        storedItem = Instantiate(itemToDrop, GetComponentInChildren<SpriteRenderer>().transform);
        if (storedItem.GetComponent<Pickup>() != null)
        {
            storedItem.GetComponent<Pickup>().StopAllCoroutines();
            storedItem.GetComponent<Pickup>().autoDespawn = false;
        }
        // item in balloon should not be collectable yet
        if (storedItem.GetComponent<Collider2D>() != null)
            storedItem.GetComponent<Collider2D>().enabled = false;
        else
            storedItem.GetComponentInChildren<Collider2D>().enabled = false;
        // find the item's sprite renderer and change its sorting layer
        if (storedItem.GetComponent<SpriteRenderer>() != null)
        {
            storedItem.GetComponent<SpriteRenderer>().sortingLayerName = "AboveEntity";
            storedItem.GetComponent<SpriteRenderer>().sortingOrder = -1; // ensures it will be behind the balloon
        }
        else
        {
            storedItem.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "AboveEntity";
            storedItem.GetComponentInChildren<SpriteRenderer>().sortingOrder = -1; // ensures it will be behind the balloon
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerSword") && other.gameObject.GetComponentInParent<PlayerController>().isJumping)
        {
            GameObject fallingItem = Instantiate(fallingItemPrefab, transform.position, Quaternion.identity);
            storedItem.transform.position = transform.position;
            storedItem.transform.SetParent(fallingItem.transform);
            fallingItem.GetComponent<FallingItemScript>().storedItem = storedItem;
            fallingItem.GetComponent<FallingItemScript>().groundLevelY = transform.root.position.y;
            Destroy(transform.root.gameObject); // gets rid of the item balloon
        }
    }
}
