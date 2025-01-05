using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    [SerializeField] GameObject ChipAtShip;
    [SerializeField] GameObject ChipInWild;
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
            foreach (Item item in itemList) {
                if (item.isRock() && item.amount>=10) { item.amount -= 10; }
                if (item.amount <= 0) { inventory.RemoveItem(item); }
                ChipInWild.SetActive(false);
                ChipAtShip.SetActive(true);
                ChipIsInShip = true;
            }
        }
        else { /*Dialogue, Search for 10 rocks, if found go to ship*/ }
    }
}
