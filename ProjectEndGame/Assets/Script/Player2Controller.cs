using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    private Rigidbody2D rb;
    public InventoryPlayer2 inventory;

    [SerializeField]
    private UI_InventoryPlayer2 uI_Inventory;

    // Tombol-tombol pergerakan yang dapat dikustomisasi oleh pemain
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private void Awake()
    {
        inventory = new InventoryPlayer2(UseItem);
        uI_Inventory.SetInventory(inventory);


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
}
