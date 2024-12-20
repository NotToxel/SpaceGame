using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    public static Inventory instance;
    private static int hotbarSize = 9; 
    private static int inventoryUISize = 27;
    public static int maxSize = hotbarSize + inventoryUISize;
    private static List<Item> itemList = new List<Item>(maxSize);

    public static Inventory Instance {
        get {
            if (instance == null) {
                instance = new Inventory();
                Debug.Log("New inventory made containing " + (maxSize) + " slots");
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
        //Debug.Log(itemList.Count);
    }

    public void AddItem(Item item) {
        //Debug.Log(item.itemType);
        if (GetItemCount()+1 > maxSize) {
            Debug.Log("Inventory is too full");
            return;
        }
        if (item.IsStackable()){
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemType == item.itemType) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory) {
                itemList.Add(item);
            }
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
