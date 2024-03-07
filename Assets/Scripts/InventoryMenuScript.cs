using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuScript : MonoBehaviour
{
    public static InventoryMenuScript instance;

    [SerializeField] GameObject inventoryMenu; //gets inventory menu panel

    InventoryInput iInput; //reference to input -- letter E for keyboard

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//

        iInput = new InventoryInput(); //reference to input
    }
    void Start()
    {
        inventoryMenu.SetActive(false); //disables inventory menu on start
        iInput.Enable();
    }

    void Update()
    {
        //keyboard input for opening/closing inventory menu
        if (iInput.Inventory.Open.triggered)
        {
            ToggleInventoryMenu();
        }
    }

    public void ToggleInventoryMenu() //toggles inventory menu on or off
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf); //turns menu on or off based on previous value
    }
}
