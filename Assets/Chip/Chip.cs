using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] GameObject ChipAtShip;
    [SerializeField] GameObject ChipInWild;
    [SerializeField] HealthBar healthBar;
    private Inventory inventory = Inventory.Instance;

    private bool ChipIsInShip;


    void Start()
    {
        // Initial States
        ChipInWild.SetActive(true);
        ChipAtShip.SetActive(false);
        ChipIsInShip = false;
        //Debug.Log(ChipIsInShip);
    }

    public void Interact() {
        List<Item> itemList = inventory.GetItemList();
        if (ChipIsInShip==true) { // Case for when Chip is in the ship
            Debug.Log("Interacting with Chip inside ship"); /*Dialogue, Search for rocks, remove them, +HP*/
            //Debug.Log("Checking inv");
            foreach (Item item in itemList) {
                if (item.isRock()) {
                    Debug.Log(item.amount + " Rocks found!");
                    healthBar.bonusHPfromChip((float)item.amount);
                    inventory.RemoveItem(item);
                }
            }
        }
        else { // Case for when Chip is in the wild
            Debug.Log("Interacting with Chip in the wild");
            foreach (Item item in itemList) {
                if (item.isRock() && item.amount>=10) {
                    Debug.Log("10 Rocks found!");
                    if (item.amount == 10) { inventory.RemoveItem(item); }
                    else { item.amount -= 10; }

                    ChipInWild.SetActive(false);
                    ChipAtShip.SetActive(true);
                    ChipIsInShip = true;

                    //ChipInWild.GetComponent<Collider>().enabled = false;
                    //ChipAtShip.GetComponent<Collider>().enabled = true;
                }
            }
        }
    }
}
