/*-----------------------------------------
Creation Date: N/A
Author: jose
Description: 
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public enum Weapon
    {
        Spoon,
        RollingPin,
        Chancla,
    }
    public enum Shield
    {
        PotLid,
        TinFoil,
    }
    public enum Subweapon
    {
        Bomb,
    }
    public enum Item
    {
        Spoon,
        RollingPin,
        Chancla,
        PotLid,
        TinFoil,
        Bomb,
    }
    
    public Weapon currentWeapon;
    public Shield currentShield;
    public Subweapon currentSubweapon;
    [SerializeField] public Item currentItem;

    public Dictionary<Item, bool> collectedItems = new Dictionary<Item, bool>();
}
