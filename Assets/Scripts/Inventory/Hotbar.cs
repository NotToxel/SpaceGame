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

    #region Item Selection
    public void HandleScrollInput(float scrollValue)
    {
        //Debug.Log("Scroll Value: " + scrollValue);
        if (scrollValue == 0) { return; }

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
        RefreshInventoryItems();
    }

    public void HandleNumberKeyInput(int keyPressed)
    {
        if (keyPressed == -1) { return; }
        int targetSlot = keyPressed-1;
        Debug.Log("Target Slot: " + targetSlot);

        if (targetSlot == selectedSlot) { 
            selectedSlot = -1;
            Debug.Log("No slot selected");
        }
        else {
            for (int i = 0; i < hotbarSize; i++)
            {
                if (i == targetSlot)
                {
                    Debug.Log("Slot " + i + " selected");
                    selectedSlot = i;
                    break;
                }
            }
        }
        RefreshInventoryItems();
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
                    Debug.Log("Slot" + x + " selected");
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
