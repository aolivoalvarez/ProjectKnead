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
    Inventory inventory = new Inventory();
    [SerializeField] protected Inventory.Item thisItem;

    protected override void PlayerCollect()
    {
        
        inventory.collectedItems[thisItem] = true;
    }

    public override void PlayerCollectDontDestroy()
    {
        throw new System.NotImplementedException();
    }
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
