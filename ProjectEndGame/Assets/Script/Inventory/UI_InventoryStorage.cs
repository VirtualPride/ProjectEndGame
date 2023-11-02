using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_InventoryStorage : MonoBehaviour
{
    private InventoryStorage inventoryStorage;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private InventoryStorageManager inventoryStorageManager;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainerStorage");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplateStorage");
    }

    public void SetInventory(InventoryStorage inventoryStorage, InventoryStorageManager inventoryStorageManager)
    {
        this.inventoryStorage = inventoryStorage;
        this.inventoryStorageManager = inventoryStorageManager; // Inisialisasi referensi ke Player2Controller

        // Menambahkan event handler untuk event OnItemListChanged dari InventoryPlayer2
        inventoryStorage.OnItemListChanged += Inventory_OnItemListChanged;

        // Memuat ulang tampilan inventori
        RefreshInventoryItems();
    }


    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        // Ketika isi inventori berubah, memuat ulang tampilan inventori
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        // Menghapus semua item slot yang ada dalam itemSlotContainer
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0; // Koordinat X untuk item slot
        int y = 0; // Koordinat Y untuk item slot
        float itemSlotCellSize = 60f; // Ukuran setiap item slot
        int selectedItemIndex = inventoryStorageManager.GetSelectedItemIndex(); // Ambil indeks item yang dipilih dari Player2Controller
        int childIndex = 0; // Indeks untuk item slot

        // Iterasi melalui semua item dalam inventori dan membuat item slot baru untuk setiap item
        foreach (Item item in inventoryStorage.GetItemList())
        {
            // Membuat objek RectTransform sebagai item slot baru dari itemSlotTemplate
            RectTransform itemSlotTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotTransform.gameObject.SetActive(true);

            // Menentukan posisi anchored (berdasarkan koordinat x dan y) untuk item slot
            itemSlotTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

            // Mengambil komponen Image untuk menampilkan gambar item
            Image image = itemSlotTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            // Mengambil komponen TextMeshProUGUI untuk menampilkan jumlah item (jika lebih dari 1)
            TextMeshProUGUI uiText = itemSlotTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                uiText.SetText(item.amount.ToString());
            }
            else
            {
                uiText.SetText("");
            }

            Image selectImage = itemSlotTransform.Find("select").GetComponent<Image>();
            selectImage.enabled = childIndex == selectedItemIndex; // Aktifkan gambar select hanya pada item yang dipilih
            childIndex++;



            // Mengatur posisi X untuk item slot berikutnya
            x++;
            if (x > 3)
            {
                x = 0;
                y--;
            }
        }
    }

    public void SetSelectedItemHighlight(int selectedIndex)
    {
        int childIndex = 0;
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;

            Image selectImage = child.Find("select").GetComponent<Image>();
            selectImage.enabled = childIndex == selectedIndex; // Aktifkan gambar select hanya pada item yang dipilih
            childIndex++;
        }
    }

    internal void SetInventory(InventoryStorage inventoryStorage, Player1Controller player1Controller)
    {
        throw new NotImplementedException();
    }
}