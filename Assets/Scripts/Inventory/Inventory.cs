/*-----------------------------------------
Creation Date: N/A
Author: jose
Description: 
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
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
    //[SerializeField] public Item currentItem;

    public SerializedDictionary<Item, bool> collectedItems = new SerializedDictionary<Item, bool>();

    public void Start()
    {
        foreach(var(key,value) in collectedItems)
        {
            collectedItems[key] = false;
        }
    }
}
