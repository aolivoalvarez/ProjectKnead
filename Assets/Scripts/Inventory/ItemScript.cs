/*-----------------------------------------
Creation Date: N/A
Author: jose
Description: 
-----------------------------------------*/

using UnityEngine;

public class ItemScript : Pickup
{
    [SerializeField] protected Inventory.Item thisItem;

    protected override void PlayerCollect()
    {
        Inventory.instance.collectedItems[thisItem] = true;
        Destroy(gameObject);
    }

    public override void PlayerCollectDontDestroy()
    {
        Inventory.instance.collectedItems[thisItem] = true;
    }
}
