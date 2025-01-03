using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    Inventory inventory = Inventory.Instance;

    [Header("UI References")]
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private Hotbar hotbar;
    [SerializeField] private Transform inventorySlotContainer;
    [SerializeField] private Transform hotbarSlotContainer;
    [SerializeField] private RectTransform inventorySlotTemplate;
    [SerializeField] private RectTransform hotbarSlotTemplate;
    [SerializeField] private GameObject player;

    private int firstSelectedSlot = -1;

    public void SlotClicked(int slotIndex) {
        if (firstSelectedSlot == -1) {
            firstSelectedSlot = slotIndex;
            Debug.Log("First slot selected: " + slotIndex);
        }
        else if (firstSelectedSlot == slotIndex) {
            firstSelectedSlot = -1;
            Debug.Log("Slot deselected");
        }
        else {
            Debug.Log("Second slot selected: " + slotIndex);
            SwapSlots(firstSelectedSlot, slotIndex);
            firstSelectedSlot = -1;
        }
    }

    private void SwapSlots(int indexA, int indexB) {
        if (indexA == indexB) { return; }
        List<Item> itemList = inventory.GetItemList();
        
        // Swapping Algorithm
        Item temp = itemList[indexA];
        itemList[indexA] = itemList[indexB];
        itemList[indexB] = temp;

        hotbar.RefreshHotbar();
        inventoryUI.RefreshInventory();
    }

    public void DropItem(int targetSlot) {
        if (targetSlot==-1) { return; }
        Debug.Log("Dropping item from slot: " + (targetSlot+1));
        List<Item> itemList = inventory.GetItemList();
        Item item = itemList[targetSlot];
        inventory.RemoveItem(item);
        ItemWorld.DropItem(player.GetComponent<Transform>(), item);

        hotbar.RefreshHotbar();
        inventoryUI.RefreshInventory();
    }
}