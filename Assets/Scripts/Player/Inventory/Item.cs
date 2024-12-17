using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType {
        Sword,
        Wrench
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite() {
        switch (itemType) {
        default:
        case ItemType.Sword: return ItemAssets.Instance.swordSprite;
        case ItemType.Wrench: return ItemAssets.Instance.wrenchSprite;
        }
    }

    public GameObject GetPrefab() {
        switch (itemType) {
        default:
        case ItemType.Sword: return ItemAssets.Instance.swordPrefab;
        case ItemType.Wrench: return ItemAssets.Instance.wrenchPrefab;
        }
    }

    public bool IsStackable() {
        switch (itemType) {
        default:
        case ItemType.Sword:
            return true;
        case ItemType.Wrench:
            return false;
        }
    }
}
