using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartup : MonoBehaviour
{
    [SerializeField] private Hotbar hotbar;

    // === Instantiate Objects === //
    private Inventory inventory;

    void Awake()
    {
        inventory = new Inventory();
        hotbar.SetInventory(inventory);

        Debug.Log("Spawning!");
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 2), new Item { itemType = Item.ItemType.Sword, amount = 1 });
        Debug.Log("Sword spawned!");
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 1), new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        Debug.Log("Wrench spawned!");
    }
}
