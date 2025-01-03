using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType {
        Sword,
        Wrench,
        Knife
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch (itemType) {
        default:
        case ItemType.Sword: return ItemAssets.Instance.swordSprite;
        case ItemType.Wrench: return ItemAssets.Instance.wrenchSprite;
        case ItemType.Knife: return ItemAssets.Instance.swordSprite;
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

    //public bool IsConsumable() {}
}
