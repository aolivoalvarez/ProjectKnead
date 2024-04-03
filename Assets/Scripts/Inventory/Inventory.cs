/*-----------------------------------------
Creation Date: N/A
Author: jose
Description: 
-----------------------------------------*/

using UnityEngine;
using AYellowpaper.SerializedCollections;

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

    public SerializedDictionary<Item, bool> collectedItems;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }
}
