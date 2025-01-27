// Script reference: https://www.youtube.com/watch?v=2WnAOV7nHW0

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{
    public Item item;

    private void Start() {
        Debug.Log("Spawning Item");
        ItemWorld.SpawnItemWorld(transform.position, transform.rotation, item);
        Destroy(gameObject);
    }
}
