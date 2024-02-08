using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InventoryMenuScript : MonoBehaviour
{
    [SerializeField] GameObject inventoryMenu; //gets inventory menu panel

    InventoryInput iInput; //reference to input -- letter E for keyboard

    private void Awake()
    {
        iInput = new InventoryInput(); //reference to input
    }

    private void OnEnable()
    {
        iInput.Enable(); //enables input
    }

    private void OnDisable()
    {
        iInput.Disable(); //disables input
    }
    void Start()
    {
        inventoryMenu.SetActive(false); //disables inventory menu on start
    }

    private void Update()
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

    public void pressStart() //function for start button -- goes back to title screen
    {
        SceneManager.LoadSceneAsync("TitleScreen"); //opens TitleSreen scene
    }
    
    public void pressContinue() //function for continue button -- toggles inventory menu to take it off
    {
        ToggleInventoryMenu();
    }

    public void pressExit() //function for exit button -- goes to game over screen
    {
        SceneManager.LoadSceneAsync("GameOverScreen"); //opens GameOverScreen scene
    }
}
