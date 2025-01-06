// Script reference: https://www.youtube.com/watch?v=2WnAOV7nHW0

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

    // - Items - //
    public Sprite hammerSprite;

    // - Armor - //
    public Sprite helmetSprite;
    public Sprite chestSprite;
    public Sprite legsSprite;
    public Sprite bootsSprite;



    // === Prefabs === //
    // Add more prefabs here
    public GameObject hammerPrefab;
    public GameObject hammerStaticPrefab;
}
