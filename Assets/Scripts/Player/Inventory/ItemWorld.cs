using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Item item) {
        GameObject prefab = Instantiate(item.GetPrefab(), position, Quaternion.identity);

        ItemWorld itemWorld = prefab.GetComponent<ItemWorld>();
        //Debug.Log(itemWorld);
        itemWorld.SetItem(item);

        return itemWorld;
    }   
    Item item;
    GameObject prefabRenderer;

    private void Awake() {
        //prefabRenderer = GetComponent<GameObject>();
    }

    public void SetItem(Item item) {
        this.item = item;
        //prefabRenderer = item.GetPrefab();
    }

    public Item GetItem() {
        return item;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
