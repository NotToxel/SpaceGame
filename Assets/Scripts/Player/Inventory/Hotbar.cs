using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;

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
        float itemSlotCellSize = 100f;
        foreach (Item item in itemList) {
            if (x < 9) { // The hotbar has 9 slots
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

                x++;
            } 
            else { Debug.Log("Hotbar is full!"); break; }
        }
    }
}
