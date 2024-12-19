using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private CinemachineFreeLook camera;
    private Inventory inventory = Inventory.Instance;
    private static int width = 9; // 9 slots wide
    private static int height = 3; // 3 slots high
    public static int inventorySize = width*height;
    private int hotbarSize = 9;

    void Awake() {
        InventoryPanel.SetActive(false);
    }

    private void RefreshInventory() {
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
        foreach (Item item in inventory.GetItemList().Skip(hotbarSize)) { // Start at index 9
            // Instantiate a new item slot for each item beyond the hotbar
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
            } else {
                text.SetText("");
            }

            // Set the border color (default for non-hotbar items)
            //Image border = itemSlotRectTransform.Find("border").GetComponent<Image>();
            //border.color = defaultSlotColor;

            x++;
            if (x >= width) {
                x = 0;
                y--;
            }
        }
    }

    public void ToggleInventory(bool inventoryIsOpen) {
        if (InventoryPanel != null) {
            bool isOpen = InventoryPanel.activeSelf;
            InventoryPanel.SetActive(!isOpen);

            // Cursor state
            Cursor.lockState = isOpen ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isOpen;

            // Disable or enable Cinemachine camera input
            if (camera != null) {
                camera.m_XAxis.m_InputAxisName = isOpen ? "Mouse X" : "";
                camera.m_YAxis.m_InputAxisName = isOpen ? "Mouse Y" : "";
            }
        }
        RefreshInventory();
    }
}


