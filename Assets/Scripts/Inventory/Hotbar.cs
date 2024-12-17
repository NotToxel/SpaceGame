using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    public int hotbarSize;
    private int selectedSlot = 0;
    public Color selectedSlotColor = Color.blue;
    public Color defaultSlotColor = Color.white;

    
    void Update() {
        HandleScrollInput();
        HandleNumberKeyInput();
        DropItemKeyInput();
        //ItemPickup();
    }

    #region Item Selection
    void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) // Scroll up
        {
            selectedSlot = (selectedSlot + 1) % hotbarSize;
        }
        else if (scroll < 0f) // Scroll down
        {
            selectedSlot = (selectedSlot - 1 + hotbarSize) % hotbarSize;
        }
    }

    void HandleNumberKeyInput()
    {
        for (int i = 0; i < hotbarSize; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedSlot = i;
                break;
            }
        }
    }

    void DropItemKeyInput() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            List<Item> itemList = inventory.GetItemList();
            Item item = itemList[selectedSlot];
            inventory.RemoveItem(item);
            ItemWorld.DropItem(player.GetComponent<Transform>(), item);
        }
    }

    //void ItemPickup() {
    //}
    #endregion

    public void SetInventory(Inventory inventory) {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems() {
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
                if (x == selectedSlot) {
                    border.color = selectedSlotColor;
                }
                else {
                    border.color = defaultSlotColor;
                }

                hotbarSize++;
                x++;
            } 
            else { Debug.Log("Hotbar is full!"); break; }
        }

        /*for(int i = 0; i < 9; i++) {
            Item item = itemList[i];
            //Debug.Log(item.itemType);

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
            if (x == selectedSlot) {
                border.color = selectedSlotColor;
            }
            else {
                border.color = defaultSlotColor;
            }

            hotbarSize++;
            x++;
        }*/
    }

    public bool isHoldingWeapon() {
        return inventory.GetItem(selectedSlot).IsWeapon();
    }
}
