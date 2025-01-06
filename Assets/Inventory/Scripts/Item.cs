// Script reference: https://www.youtube.com/watch?v=2WnAOV7nHW0

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType {
        // --- Item Types --- //
        Compass,
        Hammer,
        HammerStatic,
        Multitool,
        //Pebble,
        Rock,

        // --- Weapon Types --- //
     

        // --- Armour Types --- //
        Helmet,
        Chest,
        Legs,
        Boots
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch (itemType) {
        default:
        case ItemType.Hammer: return ItemAssets.Instance.hammerSprite;
        case ItemType.Helmet: return ItemAssets.Instance.helmetSprite;
        case ItemType.Chest: return ItemAssets.Instance.chestSprite;
        case ItemType.Legs: return ItemAssets.Instance.legsSprite;
        case ItemType.Boots: return ItemAssets.Instance.bootsSprite;
        }
    }

    public GameObject GetPrefab() {
        //Debug.Log("ItemType: " + itemType);
        //Debug.Log("ItemAssets.Instance: " + (ItemAssets.Instance != null));
        switch (itemType) {
            default:
                case ItemType.Hammer: return ItemAssets.Instance.hammerPrefab;
        }
    }

    public Sprite GetHelmetSprite() { return ItemAssets.Instance.helmetSprite; }
    public Sprite GetChestSprite() { return ItemAssets.Instance.chestSprite; }
    public Sprite GetLegsSprite() { return ItemAssets.Instance.legsSprite; }
    public Sprite GetBootsSprite() { return ItemAssets.Instance.bootsSprite; }

    // --- Armor --- //
    public bool IsHelmet () { return itemType==ItemType.Helmet; }
    public bool IsChest () { return itemType==ItemType.Chest; }
    public bool IsLegs () { return itemType==ItemType.Legs; }
    public bool IsBoots () { return itemType==ItemType.Boots; }


    // --- Item Behaviours --- //
    // Only true cases needed
    public bool IsStackable() {
        //switch (itemType) {
        //        return true;
        //}
        return false;
    }

    public bool IsWeapon() {
        //switch (itemType) {
        //        return true;
        //}
        return false;
    }

    public bool isCompass() { 
        if (itemType == ItemType.Compass) { return true; }
        else { return false; }
    }

    public bool isRock() {
        if (itemType == ItemType.Rock) { return true; }
        else { return false; }
    }

    public bool isHammer() {
        if (itemType==ItemType.Hammer || itemType==ItemType.HammerStatic) { return true; }
        else { return false; }
    }

    //public bool IsConsumable() {}
}
