using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    private Rigidbody2D rb;
    public InventoryPlayer2 inventory; // Referensi ke objek InventoryPlayer2

    [SerializeField]
    private UI_InventoryPlayer2 uI_Inventory; // Referensi ke UI InventoryPlayer2
    private int selectedItemIndex = 0; // Indeks item yang sedang dipilih dalam inventori

    // Tombol-tombol pergerakan yang dapat dikustomisasi oleh pemain
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private void Awake()
    {
        // Membuat instance baru dari InventoryPlayer2 dan menghubungkannya dengan inventory
        inventory = new InventoryPlayer2(UseItem);

        // Mengatur inventory UI dengan inventory yang telah dibuat dan mengirimkan referensi ke Player2Controller
        uI_Inventory.SetInventory(inventory, this);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari GameObject
    }

    void Update()
    {
        // Mendapatkan input dari pemain
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        // Memeriksa input tombol yang telah dikustomisasi
        if (Input.GetKey(moveUpKey))
            moveVertical = 1f;
        if (Input.GetKey(moveDownKey))
            moveVertical = -1f;
        if (Input.GetKey(moveLeftKey))
            moveHorizontal = -1f;
        if (Input.GetKey(moveRightKey))
            moveHorizontal = 1f;

        // Menghitung vektor pergerakan
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        // Mengatur kecepatan karakter
        rb.velocity = movement.normalized * speed;

        // Di dalam Update atau metode lain yang sesuai
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectedItemIndex < inventory.GetItemList().Count - 1)
            {
                selectedItemIndex++;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }

        // Memeriksa input untuk menggunakan item dengan tombol "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Mendapatkan item yang sedang dipilih
            Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventory.UseItem(selectedItemAt); // Menggunakan item yang dipilih
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemWorld itemWorld = other.gameObject.GetComponent<ItemWorld>();

            // Ambil item dari ItemWorld
            Item item = itemWorld.GetItem();

            if (item.IsStackable())
            {
                if (inventory.GetItemList().Count < 4)
                {
                    inventory.AddItem(item);
                    itemWorld.DestroySelf();
                }
                else
                {
                    if (inventory.HasItemInInventory(item))
                    {
                        inventory.AddItem(item);
                        itemWorld.DestroySelf();
                    }
                }
            }
            else
            {
                if (inventory.GetItemList().Count < 4)
                {
                    inventory.AddItem(item);
                    itemWorld.DestroySelf();
                }
            }
        }
    }




    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Kunci:
                Debug.Log("Kunci digunakan");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Kunci, amount = 1 }); // Menghapus kunci dari inventori
                break;
            case Item.ItemType.Buku:
                Debug.Log("Buku digunakan");
                inventory.RemoveItem(item); // Menghapus buku dari inventori
                break;
        }
    }

    public int GetSelectedItemIndex()
    {
        return selectedItemIndex;
    }


}
