using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenuScript : MonoBehaviour
{
    [SerializeField] GameObject inventoryMenu; //gets inventory menu panel

    InventoryInput iInput; //reference to input -- letter E for keyboard

    void Awake()
    {
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
