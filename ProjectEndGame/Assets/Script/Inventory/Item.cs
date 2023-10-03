using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public bool isSelected = false;
    public enum ItemType
    {
        Kunci,
        Buku
    }

    public ItemType itemType;
    public int amount;
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Kunci: return ItemAssets.Instance.kunciSprite;
            case ItemType.Buku: return ItemAssets.Instance.bukuSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Kunci: return true;
            case ItemType.Buku: return false;
        }
    }
}
