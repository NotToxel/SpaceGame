using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; } // Singleton

    private void Awake() {
        //Debug.Log("Item Assets initialising");
        Instance = this;
    }
    

    // === Sprites === //
    // Add more sprites here
    // Make sure to add them in the enum class in Item.cs
    // Uses: Displaying items in the UI
    public Sprite swordSprite;
    public Sprite wrenchSprite;

    // === Prefabs === //
    // Add more prefabs here
    public GameObject swordPrefab;
    public GameObject wrenchPrefab;
}
