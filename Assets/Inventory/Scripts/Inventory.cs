using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    public static Inventory instance;
    private static int hotbarSize = 9; 
    private static int inventoryUISize = 36;
    public static int maxSize = hotbarSize + inventoryUISize;
    private static List<Item> itemList = new List<Item>(maxSize);
    private int stackLimit = 100;

    public static Inventory Instance {
        get {
            if (instance == null) {
                instance = new Inventory();
                //Debug.Log("New inventory made containing " + (maxSize) + " slots");
            }
            return instance;
        }
    }

    private Inventory() {
        itemList = new List<Item>();
        
        AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Helmet, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Chest, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Legs, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Boots, amount = 1 });
        //Debug.Log(itemList.Count);
    }

    public void AddItem(Item item) {
        //Debug.Log(item.itemType);
        if (item == null)
        {
            Debug.LogError("Item is null and cannot be checked for stackability.");
            return;
        }

        if (item.IsStackable()) { // Stackable case
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemType==item.itemType) {
                    int itemAmountTotal = inventoryItem.amount + item.amount;
                    if (itemAmountTotal > stackLimit) {
                        int residue = stackLimit - inventoryItem.amount;
                        inventoryItem.amount = 100;
                        item.amount = residue;
                        itemList.Add(item);
                    }
                    else {
                        inventoryItem.amount += item.amount;
                        itemAlreadyInInventory = true;
                    }
                }
            }
            if (!itemAlreadyInInventory) {
                itemList.Add(item);
            }
        }
        else if (GetItemCount()+1 > maxSize) { // Full Inventory Ccase
            Debug.Log("Inventory is too full");
            return;
        }
        else {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item) {
        if (item.IsStackable()){
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemType == item.itemType) {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0) {
                itemList.Remove(item);
            }
        }
        else {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList() {
        return itemList;
    }

    public Item GetItem(int index) {
        return itemList[index];
    }

    public int GetItemCount() {
        return itemList.Count;
    }


    public int GetMaxSize() {
        return maxSize;
    }
}
