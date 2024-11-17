using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; } // Singleton

    private void Awake() {
         if (Instance == null) {
            Instance = this; // Assigns the current instance to the static property
        } else {
            Destroy(gameObject); // Ensures there’s only one instance
        }
    }

    // === Sprites === //
    // Add more sprites here
    // Make sure to add them in the enum class in Item.cs
    public Sprite swordSprite;
    public Sprite wrenchSprite;
}
