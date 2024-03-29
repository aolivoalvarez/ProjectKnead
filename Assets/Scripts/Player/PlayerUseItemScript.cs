/*-----------------------------------------
Creation Date: 3/28/2024 2:28:36 PM
Author: theco
Description: Handles all of the Player's equipped item actions.
-----------------------------------------*/

using UnityEngine;

public class PlayerUseItemScript : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    [SerializeField] LayerMask layerMask;
    PlayerController playerController;
    GameObject heldItem;

    void Start()
    {
        playerController = PlayerController.instance;
    }

    void Update()
    {
        /*Debug.DrawLine(transform.position, transform.position + (Vector3)playerController.simpleLookDirection, Color.green);
        if (Physics2D.Raycast(transform.position, playerController.simpleLookDirection, 1f, layerMask))
            Debug.DrawLine(transform.position, transform.position + (Vector3)playerController.simpleLookDirection, Color.red);*/
        if (heldItem != null)
        {
            var animator = heldItem.GetComponentInChildren<Animator>();
            animator.SetFloat("Direction X", playerController.simpleLookDirection.x);
            animator.SetFloat("Direction Y", playerController.simpleLookDirection.y);
        }
    }

    public void HoldBomb()
    {
        heldItem = Instantiate(bombPrefab, transform);
        heldItem.GetComponent<Collider2D>().enabled = false;
        heldItem.transform.position = new Vector2(playerController.transform.position.x, playerController.transform.position.y + 2f);
        heldItem.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "AboveEntity";
    }

    public void ReleaseBomb(bool movingBomb = false)
    {
        if (heldItem == null)
            return;

        heldItem.transform.SetParent(null);
        heldItem.GetComponent<Collider2D>().enabled = true;
        heldItem.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "Entity";
        
        if (Physics2D.Raycast(transform.position, playerController.simpleLookDirection, 1f, layerMask))
        {
            Vector2 hitPoint = Physics2D.Raycast(transform.position, playerController.simpleLookDirection, 1f, layerMask).point;
            heldItem.transform.position = hitPoint;
        }
        else
        {
            heldItem.transform.position = new Vector2(transform.position.x + playerController.simpleLookDirection.x,
                transform.position.y + playerController.simpleLookDirection.y);
        }

        if (movingBomb) heldItem.GetComponent<BombScript>().BombStartMoving();
        heldItem = null;
    }
}
