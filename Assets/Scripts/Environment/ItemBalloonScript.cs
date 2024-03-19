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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerSword") && other.gameObject.GetComponentInParent<PlayerController>().isJumping)
        {
            GameObject fallingItem = Instantiate(fallingItemPrefab, transform.position, Quaternion.identity);
            fallingItem.GetComponent<FallingItemScript>().storedItem = Instantiate(itemToDrop, fallingItem.transform);
            fallingItem.GetComponent<FallingItemScript>().groundLevelY = transform.root.position.y;
            Destroy(transform.root.gameObject); // gets rid of the item balloon
        }
    }
}
