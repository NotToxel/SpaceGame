using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] GameObject ChipAtShip;
    [SerializeField] GameObject ChipInWild;

    private bool ChipIsInShip;


    // Start is called before the first frame update
    void Start()
    {
        ChipInWild.SetActive(true);
        ChipAtShip.SetActive(true);
        ChipIsInShip = true;
    }

    public void Interact() {
        if (ChipIsInShip) { Debug.Log("Interacting with Chip inside ship");/*Dialogue, Search for rocks, remove them, +HP*/ }
        else { /*Dialogue, Search for 10 rocks, if found go to ship*/ }
    }
}
