using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer2
{
    public event EventHandler OnItemListChanged; // Event yang dipanggil ketika isi inventori berubah.
    private List<Item> itemList; // Daftar item dalam inventori.
    private Action<Item> useItemAction; // Aksi yang akan dilakukan saat menggunakan item.

    // Konstruktor untuk membuat instance InventoryPlayer2.
    public InventoryPlayer2(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction; // Menginisialisasi aksi penggunaan item.
        itemList = new List<Item>(); // Inisialisasi daftar item dalam inventori.

        // Contoh: Menambahkan beberapa item awal ke inventori.
        AddItem(new Item { itemType = Item.ItemType.Kunci, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.Buku, amount = 1 });

        Debug.Log(itemList.Count); // Menampilkan jumlah item dalam inventori (debugging).
    }

    // Metode untuk menambahkan item ke inventori.
    public void AddItem(Item item)
    {
        if (item.IsStackable()) // Cek apakah item dapat di-stack (sama jenis item).
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount; // Menambah jumlah item yang sudah ada.
                    itemAlreadyInInventory = true;
                    break; // Keluar dari loop setelah menemukan item yang sudah ada.
                }
            }

            if (!itemAlreadyInInventory)
            {
                itemList.Add(item); // Tambahkan item baru jika belum ada dalam inventori.
            }
        }
        else
        {
            itemList.Add(item); // Tambahkan item non-stackable ke inventori.
        }

        // Panggil event untuk memberi tahu bahwa inventori telah berubah.
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    // Metode untuk menggunakan item dari inventori.
    public void UseItem(Item item)
    {
        useItemAction(item); // Panggil aksi penggunaan item yang telah diatur.
    }

    // Metode untuk menghapus item dari inventori.
    public void RemoveItem(Item item)
    {
        if (item.IsStackable()) // Cek apakah item dapat di-stack (sama jenis item).
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount; // Kurangi jumlah item yang sudah ada.
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

    // Metode untuk mendapatkan item berdasarkan indeks dalam inventori.
    public Item GetItemAtIndex(int index)
    {
        if (index >= 0 && index < itemList.Count)
        {
            return itemList[index];
        }
        else
        {
            return null; // Mengembalikan null jika indeks tidak valid.
        }
    }

    // Metode untuk mendapatkan seluruh daftar item dalam inventori.
    public List<Item> GetItemList()
    {
        return itemList;
    }
}
