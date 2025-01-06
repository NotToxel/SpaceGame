// Script reference: https://www.youtube.com/watch?v=2WnAOV7nHW0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(Vector3 position, Quaternion rotation, Item item) {
        GameObject prefab = Instantiate(item.GetPrefab(), position, rotation);

        ItemWorld itemWorld = prefab.GetComponent<ItemWorld>();
        //Debug.Log(itemWorld);
        itemWorld.SetItem(item);

        return itemWorld;
    }   

    public static ItemWorld DropItem(Transform playerTransform, Transform cameraDirection, Item item) {
        // Implement offset via camera direction here, probably by passing it thorugh as a param
        Vector3 playerPosition = playerTransform.position;
        Vector3 offset = cameraDirection.forward * 1.0f;
        Vector3 dropPosition = playerPosition + offset;
        
        // Can Implement random rotation
        Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        ItemWorld itemWorld = SpawnItemWorld(dropPosition, randomRotation, item);
        Rigidbody rb = itemWorld.GetComponent<Rigidbody>();

        Vector3 force = new Vector3(0f, -1f, 0f);
        rb.AddForce(force, ForceMode.Impulse);

        rb.isKinematic = false;

        return itemWorld;
    }

    Item item;
    GameObject prefabRenderer;

    private void Awake() {
        //prefabRenderer = GetComponent<GameObject>();
    }

    public void SetItem(Item item) {
        this.item = item;
        prefabRenderer = item.GetPrefab();
    }

    public Item GetItem() {
        return item;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
