using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartup : MonoBehaviour
{
    [SerializeField] private Hotbar hotbar;

    // === Instantiate Objects === //
    private Inventory inventory;

    void Start()
    {
        inventory = new Inventory();
        hotbar.SetInventory(inventory);

        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 2), new Item { itemType = Item.ItemType.Sword, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 1), new Item { itemType = Item.ItemType.Wrench, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 3), new Item { itemType = Item.ItemType.Sword, amount = 1 });
        ItemWorld.SpawnItemWorld(new Vector3(-7, 1, 4), new Item { itemType = Item.ItemType.Wrench, amount = 1 });
    }

    private void OnTriggerEnter(Collider collider) {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if (itemWorld != null) {
            inventory.AddItem(itemWorld.GetItem());
            Destroy(itemWorld.gameObject);
        }
    }
}
