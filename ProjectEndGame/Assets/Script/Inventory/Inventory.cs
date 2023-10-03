using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        AddItem(new Item { itemType = Item.ItemType.Kunci, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });

        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                    break; // Keluar dari loop setelah menemukan item yang sudah ada.
                }
            }

            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                    break;
                }
            }

            // Hapus item dari inventori jika jumlahnya kurang dari atau sama dengan 0.
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            // Hapus item non-stackable langsung dari inventori.
            itemList.Remove(item);
        }

        // Panggil event untuk memberi tahu bahwa inventori telah berubah.
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public Item GetItemAtIndex(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            return itemList[index];
        }
        else
        {
            return null;
        }
    }




    public List<Item> GetItemList()
    {
        return itemList;
    }
}
