using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    Inventory inventory = Inventory.Instance;

    [Header("UI References")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private ArmorPanel armorPanel;
    [SerializeField] private GameObject compass;

    [Header("Slot References")]
    [SerializeField] private Transform inventorySlotContainer;
    [SerializeField] private Transform hotbarSlotContainer;
    [SerializeField] private RectTransform inventorySlotTemplate;
    [SerializeField] private RectTransform hotbarSlotTemplate;

    [Header("Player References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;

    private int firstSelectedSlot = -1;
    public Color selectedSlotColor = Color.blue;
    public Color defaultSlotColor = Color.white;

    void Start() {
        RefreshUI();
    }

    public void SlotClicked(int slotIndex) {
        if (firstSelectedSlot == -1) {
            firstSelectedSlot = slotIndex;
            //Debug.Log("First slot selected: " + slotIndex);
        }
        else if (firstSelectedSlot == slotIndex) {
            firstSelectedSlot = -1;
            //Debug.Log("Slot deselected");
        }
        else {
            //Debug.Log("Second slot selected: " + slotIndex);
            SwapSlots(firstSelectedSlot, slotIndex);
            firstSelectedSlot = -1;
        }
        RefreshUI();
    }

    private void SwapSlots(int indexA, int indexB) {
        if (indexA == indexB) { return; } // Can't swap same slot
        if (indexA>=inventory.GetMaxSize() && indexB>=inventory.GetMaxSize()) { return; } // Can't swap armor in different slots (they will not be same type)

        List<Item> itemList = inventory.GetItemList();

        // Try to swap equipped armor
        if (indexA>=inventory.GetMaxSize() ^ indexB>=inventory.GetMaxSize()) { 
            //Debug.Log("Trying armor swap");
            SwapArmor(indexA, indexB);
            return;
        }
        
        // Swap Normally
        Item temp = itemList[indexA];
        itemList[indexA] = itemList[indexB];
        itemList[indexB] = temp;

        RefreshUI();
    }

    public void DropItem(int targetSlot) {
        if (targetSlot==-1) { return; }
        //Debug.Log("Dropping item from slot: " + (targetSlot+1));
        List<Item> itemList = inventory.GetItemList();
        Item item = itemList[targetSlot];
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.GetComponent<Transform>(), camera.GetComponent<Transform>(), item);

        RefreshUI();
    }

    public int SelectedSlot() {
        return firstSelectedSlot;
    }

    public Color SetBorderColor(Image border, int slotIndex) {
        if (slotIndex == firstSelectedSlot) {
            return selectedSlotColor;
        }
        else {
            return defaultSlotColor;
        }
    }

    public void RefreshUI() {
        hotbar.RefreshHotbar();
        inventoryUI.RefreshInventory();
        armorPanel.RefreshArmorPanel();
        refreshCompass();
        Debug.Log("Refreshing UI");
    }

    public void SwapArmor(int indexA, int indexB) {
        List<Item> itemList = inventory.GetItemList();
        Item equippedArmor;
        Item storedArmor;
        int armorSlot;
        int inventorySlot;
        bool AisArmorSlot = false;
        if (indexA >= inventory.GetMaxSize()) { 
            equippedArmor = armorPanel.GetEquippedArmor(indexA);
            storedArmor = itemList[indexB];
            armorSlot = indexA;
            inventorySlot = indexB;
            AisArmorSlot = true;
        }
        else { 
            equippedArmor = armorPanel.GetEquippedArmor(indexB);
            storedArmor = itemList[indexA];
            armorSlot = indexB;
            inventorySlot = indexA;
        }

        // New Equip case
        if (equippedArmor == null) { 
            //Debug.Log(armorSlot);
            bool success = armorPanel.Equip(storedArmor, armorSlot);
            if (success) { inventory.RemoveItem(itemList[inventorySlot]); }
            //Debug.Log("Removing slot " + inventorySlot);
            return;
        }

        // Not same armor piece case
        if (equippedArmor.itemType != storedArmor.itemType) { /*Debug.Log("Not the same armor piece");*/ return; }

        // Swap Case
        itemList[inventorySlot] = armorPanel.SwapArmor(storedArmor, armorSlot);
    }

    public void refreshCompass() {
        if (inventory.ContainsCompass()) { compass.SetActive(true); /*Debug.Log("No Compass");*/ }
        else { compass.SetActive(false); /*Debug.Log("Compass");*/ }
    }
}