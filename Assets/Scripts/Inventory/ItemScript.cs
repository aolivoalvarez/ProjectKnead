/*-----------------------------------------
Creation Date: N/A
Author: jose
Description: 
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

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
    // Start is called before the first frame update
    void Start()
    {
        Inventory inventory = Inventory.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
