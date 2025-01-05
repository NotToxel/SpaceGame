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
        Wrench,
        Pebble,
        Rock,

        // --- Weapon Types --- //
        Sword,
        Knife,

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
        case ItemType.Sword: return ItemAssets.Instance.swordSprite;
        case ItemType.Wrench: return ItemAssets.Instance.wrenchSprite;
        case ItemType.Knife: return ItemAssets.Instance.swordSprite;
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
                case ItemType.Sword: return ItemAssets.Instance.swordPrefab;
                case ItemType.Wrench: return ItemAssets.Instance.wrenchPrefab;
                case ItemType.Knife: return ItemAssets.Instance.knifePrefab;
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
        switch (itemType) {
            case ItemType.Sword:
            case ItemType.Knife:
                return true;
        }
        return false;
    }

    public bool IsWeapon() {
        switch (itemType) {
            case ItemType.Sword:
            case ItemType.Knife:
                return true;
        }
        return false;
    }

    public bool isCompass() { 
        if (itemType == ItemType.Compass) { return true; }
        else { return false; }
    }

    //public bool IsConsumable() {}
}
