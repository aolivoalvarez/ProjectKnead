/*-----------------------------------------
Creation Date: N/A
Author: alex
Description: Handles the UI panel for the player's inventory.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class InventoryMenuScript : MonoBehaviour
{
    public static InventoryMenuScript instance;
    public InventoryInput iInput { get; private set; } //reference to input -- letter E for keyboard

    [SerializeField] Button initialButton;
    [SerializeField] SceneField titleScene;
    Inventory inventory;

    [SerializeField] GameObject inventoryMenu; //gets inventory menu panel
    public SerializedDictionary<Inventory.Item, GameObject> itemSlots;
    public SerializedDictionary<Inventory.Item, Sprite> itemImages;

    public bool isPaused;

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
        inventory = Inventory.instance;
    }

    void Update()
    {
        //keyboard input for opening/closing inventory menu
        if (iInput.Inventory.Open.triggered)
        {
            ToggleInventoryMenu();
        }

        if (inventoryMenu.activeSelf)
        {
            UpdateInventoryMenu();
        }
    }

    public void ToggleInventoryMenu() //toggles inventory menu on or off
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        if (isPaused) PlayerController.instance.pInput.Disable();
        else PlayerController.instance.pInput.Enable();
        initialButton.Select();
        inventoryMenu.SetActive(isPaused);
    }

    public void ReturnToTitle()
    {
        ToggleInventoryMenu();
        SceneManagerScript.SwapScene(titleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateInventoryMenu()
    {
        foreach(var (key,value) in inventory.collectedItems)
        {
            if(itemSlots.ContainsKey(key))
            {
                if (value)
                {
                    itemSlots[key].SetActive(true);
                    itemSlots[key].GetComponentInChildren<Image>().color = Color.white;
                    itemSlots[key].GetComponentInChildren<Image>().sprite = itemImages[key];
                }
                else
                {
                    itemSlots[key].GetComponentInChildren<Image>().color = Color.clear;
                }
            }
        }
    }
}
