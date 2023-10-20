using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Kunci,
        Buku,
        Kunci2
    }

    public ItemType itemType;
    public int amount;
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Kunci: return ItemAssets.Instance.kunciSprite;
            case ItemType.Kunci2: return ItemAssets.Instance.kunci2Sprite;
            case ItemType.Buku: return ItemAssets.Instance.bukuSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Kunci: return false;
            case ItemType.Kunci2: return false;
            case ItemType.Buku: return true;
        }
    }


}
