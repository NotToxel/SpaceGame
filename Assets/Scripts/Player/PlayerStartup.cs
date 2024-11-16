using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartup : MonoBehaviour
{
    // === Instantiate Objects === //
    private Inventory inventory;

    void Awake()
    {
        inventory = new Inventory();
    }
}
