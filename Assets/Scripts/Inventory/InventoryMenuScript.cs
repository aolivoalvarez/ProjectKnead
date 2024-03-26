/*-----------------------------------------
Creation Date: N/A
Author: alex
Description: Handles the UI panel for the player's inventory.
-----------------------------------------*/

using UnityEngine;

public class InventoryMenuScript : MonoBehaviour
{
    public static InventoryMenuScript instance;
    public InventoryInput iInput { get; private set; } //reference to input -- letter E for keyboard

    [SerializeField] SceneField titleScene;
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    [SerializeField] GameObject inventoryMenu; //gets inventory menu panel

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
            //RefreshInventoryItems();
        }
    }

    public void ToggleInventoryMenu() //toggles inventory menu on or off
    {
        inventoryMenu.SetActive(!inventoryMenu.activeSelf); //turns menu on or off based on previous value
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

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
    }

    /*public void RefreshInventoryItems()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 30f;
        foreach (Item item in inventory.currentItem())
        { 
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate,itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            x++;
            if (x > 4)
            {
                x = 0;
                y++;
            }
        }
    } */
}
