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
    }
}
