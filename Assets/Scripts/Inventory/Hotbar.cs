using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hotbar : MonoBehaviour
{
    private Inventory inventory = Inventory.Instance;
    private UIManager uiManager;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    [SerializeField] private InventoryUI inventoryUI;
    public static int hotbarSize = 9;
    private int selectedSlot = 0;
    public bool inventoryIsOpen;

    void Awake() {
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update() {
        inventoryIsOpen = inventoryUI.IsOpen();
        //Debug.Log(inventoryIsOpen);
    }

    #region Item Selection
    public void HandleScrollInput(float scrollValue)
    {  
        if (inventoryIsOpen) { return; } // Disable when inventory is open

        //Debug.Log("Scroll Value: " + scrollValue);
        if (scrollValue == 0) { return; } // Return if no change in scrollValue

        if (selectedSlot==-1) { 
            selectedSlot = 0;
            return;
        }

        if (scrollValue > 0.0f) // Scroll up
        {
            selectedSlot = (selectedSlot + 1) % hotbarSize;
        }
        else if (scrollValue < 0.0f) // Scroll down
        {
            selectedSlot = (selectedSlot - 1 + hotbarSize) % hotbarSize;
        }
        RefreshHotbar();
    }

    public void HandleNumberKeyInput(int keyPressed)
    {
        if (inventoryIsOpen) { return; } // Disable when inventory is open

        if (keyPressed == -1) { return; } // return if no key is pressed
        int targetSlot = keyPressed-1;
        //Debug.Log("Key Pressed: " + keyPressed);
        //Debug.Log("Target Slot: " + targetSlot);

        if (targetSlot == selectedSlot) { 
            selectedSlot = -1;
            //Debug.Log("No slot selected");
        }
        else {
            for (int i = 0; i < hotbarSize; i++)
            {
                if (i == targetSlot)
                {
                    //Debug.Log("Slot " + i + " selected");
                    selectedSlot = i;
                    break;
                }
            }
        }
        RefreshHotbar();
    }

    public void PickupItem(Collider objCollider) {
        ItemWorld itemWorld = objCollider.GetComponent<ItemWorld>();
        inventory.AddItem(itemWorld.GetItem());
    }

    public void DropItem() {
        if (selectedSlot==-1) { return; }
        //Debug.Log("Dropping item from slot: " + (selectedSlot+1));
        List<Item> itemList = inventory.GetItemList();
        Item item = itemList[selectedSlot];
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.GetComponent<Transform>(), item);

        // Remove from hotbar
        /*Transform child = itemSlotContainer.GetChild(selectedSlot);

        if (child == itemSlotTemplate) { return; }

        Image image = child.Find("image").GetComponent<Image>();
        image.enabled = false;*/
        RefreshHotbar();
    }

    public GameObject GetSelectedItemPrefab() {
        if (selectedSlot==-1) { return null; }
        List<Item> itemList = inventory.GetItemList();
        return itemList[selectedSlot].GetPrefab();
    }
    #endregion

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshHotbar();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        RefreshHotbar();
    }

    private void UpdateSelectedSlot() {
        Debug.Log(selectedSlot);
        for (int i=1; i<hotbarSize; i++) {
            Transform child = itemSlotContainer.GetChild(i);

            if (child == itemSlotTemplate) { continue; }

            Image border = child.Find("border").GetComponent<Image>();
            if (i == selectedSlot+1) {  border.color = uiManager.selectedSlotColor; }
            else { border.color = uiManager.defaultSlotColor; }
        }
    }

    public void RefreshHotbar() {
        // Clear existing slots
        foreach (Transform child in itemSlotContainer) {
            if (child != itemSlotTemplate) { 
                Destroy(child.gameObject);
            }
        }

        List<Item> itemList = inventory.GetItemList();
        int x = 0;
        int y = 0;
        hotbarSize = 0;
        float itemSlotCellSize = 100f;
        foreach (Item item in inventory.GetItemList()) { // Set item indexes 0-8 as hotbar slots
            int currentSlotIndex = x;
            if (x < 9) { // The hotbar has max. 9 slots
                // Make a new instance of the item slot template
                RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

                // Find and set the image to the item's sprite
                Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
                Sprite itemSprite = item.GetSprite();
                image.sprite = itemSprite;

                // Set the amount text
                TextMeshProUGUI text = itemSlotRectTransform.Find("amount").GetComponent<TextMeshProUGUI>();
                if (item.amount > 1) {
                    text.SetText(item.amount.ToString());
                }
                else {
                    text.SetText("");
                }

                // Set the border color
                Image border = itemSlotRectTransform.Find("border").GetComponent<Image>();
                //Debug.Log(inventoryUI.IsOpen());
                if (inventoryUI.IsOpen()) {
                    border.color = uiManager.SetBorderColor(border, x);
                }
                else {
                    if (x == selectedSlot) {
                        //Debug.Log("Slot" + x + " selected");
                        border.color = uiManager.selectedSlotColor;
                    }
                    else {
                        border.color = uiManager.defaultSlotColor;
                    }
                }

                // Add button listeners
                Button button = itemSlotRectTransform.Find("border").GetComponent<Button>();
                if (button != null)
                {
                    // Listen for left click
                    button.onClick.AddListener(() => uiManager.SlotClicked(currentSlotIndex));
                    
                    // Listen for right click
                    EventTrigger eventTrigger = button.gameObject.AddComponent<EventTrigger>();
                    EventTrigger.Entry rightClickEntry = new EventTrigger.Entry
                    {
                        eventID = EventTriggerType.PointerClick
                    };

                    // Check if the right mouse button was clicked
                    rightClickEntry.callback.AddListener((data) =>
                    {
                        PointerEventData pointerData = (PointerEventData)data;
                        if (pointerData.button == PointerEventData.InputButton.Right)
                        {
                            uiManager.DropItem(currentSlotIndex);
                        }
                    });

                    eventTrigger.triggers.Add(rightClickEntry);
                }
                else
                {
                    Debug.LogError("Button component not found in itemSlotTemplate.");
                }

                hotbarSize++;
                x++;
            } 
        }
    }


    public bool isHoldingWeapon() {
        return inventory.GetItem(selectedSlot).IsWeapon();
    }

    public int getHotbarSize() { return hotbarSize; }
}
