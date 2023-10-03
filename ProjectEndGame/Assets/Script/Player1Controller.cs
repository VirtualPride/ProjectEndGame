using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    private Rigidbody2D rb;
    public Inventory inventory;

    [SerializeField]
    private UI_Inventory uI_Inventory;
    private int selectedItemIndex = 0;

    // Tombol-tombol pergerakan yang dapat dikustomisasi oleh pemain
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private void Awake()
    {
        // Membuat instance baru dari Inventory dan menghubungkannya dengan inventory
        inventory = new Inventory(UseItem);

        // Mengatur inventory UI dengan inventory yang telah dibuat dan mengirimkan referensi ke Player1Controller
        uI_Inventory.SetInventory(inventory, this);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndex < inventory.GetItemList().Count - 1)
            {
                selectedItemIndex++;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }


        // Memeriksa input untuk menggunakan item dengan tombol "C"
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Mendapatkan item yang sedang dipilih
            Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventory.UseItem(selectedItemAt);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemWorld itemWorld = other.gameObject.GetComponent<ItemWorld>();
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Kunci:
                Debug.Log("Kunci digunakan");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Kunci, amount = 1 });
                break;
            case Item.ItemType.Buku:
                Debug.Log("Buku digunakan");
                inventory.RemoveItem(item);
                break;
        }
    }
    public int GetSelectedItemIndex()
    {
        return selectedItemIndex;
    }

}
