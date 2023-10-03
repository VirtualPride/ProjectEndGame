using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    private Player1Controller playerController;

    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");
    }

    public void SetInventory(Inventory inventory, Player1Controller playerController)
    {
        this.inventory = inventory;
        this.playerController = playerController; // Inisialisasi referensi ke Player1Controller

        // Menambahkan event handler untuk event OnItemListChanged dari Inventory
        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        // Memuat ulang tampilan inventori
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 60f;
        int selectedItemIndex = playerController.GetSelectedItemIndex(); // Ambil indeks item yang dipilih dari Player1Controller
        int childIndex = 0; // Indeks untuk item slot

        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotTransform.gameObject.SetActive(true);

            itemSlotTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

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
}
