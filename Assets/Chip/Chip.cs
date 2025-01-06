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


    // Start is called before the first frame update
    void Start()
    {
        ChipInWild.SetActive(true);
        ChipAtShip.SetActive(true);
        ChipIsInShip = true;
    }

    public void Interact() {
        List<Item> itemList = inventory.GetItemList();
        if (ChipIsInShip) { 
            //Debug.Log("Interacting with Chip inside ship"); /*Dialogue, Search for rocks, remove them, +HP*/
            //Debug.Log("Checking inv");
            foreach (Item item in itemList) {
                if (item.isRock()) {
                    Debug.Log(item.amount + " Rocks found!");
                    healthBar.bonusHPfromChip((float)item.amount);
                    inventory.RemoveItem(item);
                }
            }
        }
        else { /*Dialogue, Search for 10 rocks, if found go to ship*/
            foreach (Item item in itemList) {
                if (item.isRock() && item.amount>=10) {
                    Debug.Log("10 Rocks found!");
                    item.amount -= 10;

                    ChipInWild.SetActive(false);
                    ChipAtShip.SetActive(true);
                    ChipIsInShip = true;
                    }
                if (item.amount <= 0) { inventory.RemoveItem(item); }
            }
        }
    }
}
