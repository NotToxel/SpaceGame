using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject InventoryPanel;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private GameObject camera;
    private Inventory inventory = Inventory.Instance;
    private UIManager uiManager;
    private static int width = 9; // 9 slots wide
    private static int height = 3; // 3 slots high
    public static int inventorySize = width*height;
    private int hotbarSize = 9;
    private bool invIsOpen;

    void Awake() {
        InventoryPanel.SetActive(false);
        invIsOpen = false;
        uiManager = FindObjectOfType<UIManager>();
    }

    public void RefreshInventory() {
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
        foreach (Item item in itemList.Skip(hotbarSize)) { // Start at index 9
            int currentSlotIndex = hotbarSize + x + (Math.Abs(y) * width);

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
            if (item.amount > 1) { text.SetText(item.amount.ToString()); }
            else { text.SetText(""); }

            // Set border colors
            Image border = itemSlotRectTransform.Find("border").GetComponent<Image>();
            border.color = uiManager.SetBorderColor(border, currentSlotIndex);

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
                        Debug.Log("Dropping item at slot " + currentSlotIndex);
                        uiManager.DropItem(currentSlotIndex);
                    }
                    });

                eventTrigger.triggers.Add(rightClickEntry);
            }
            else
            {
                Debug.LogError("Button component not found in itemSlotTemplate.");
            }

            // New row when needed i.e. when x exceeds the width
            x++;
            if (x >= width) {
                x = 0;
                y--;
            }
        }
    }

    public void openInventory() {
        invIsOpen = true;

        if (InventoryPanel != null) {
            InventoryPanel.SetActive(true);

            // Cursor state
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable or enable Cinemachine camera input
            if (camera != null) {
                FirstPersonCamera cameraScript = FindObjectOfType<FirstPersonCamera>();
                if (cameraScript != null) { 
                    cameraScript.DisableCam();
                }
            }
            else { Debug.Log("camera is null"); }
        }
        uiManager.RefreshUI();
    }

    public void closeInventory() {
        invIsOpen = false;

        if (InventoryPanel != null) {
            InventoryPanel.SetActive(false);

            // Cursor state
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Disable or enable Cinemachine camera input
            if (camera != null) {
                FirstPersonCamera cameraScript = FindObjectOfType<FirstPersonCamera>();
                if (cameraScript != null) { 
                    cameraScript.EnableCam();
                }
            }
            else { Debug.Log("camera is null"); }
        }
        uiManager.RefreshUI();
    }
    public bool IsOpen() { return invIsOpen; }
}


