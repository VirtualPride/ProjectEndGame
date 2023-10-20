using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    private Rigidbody2D rb;
    private bool menuOpened = false; // Status menu inventory
    private bool playerMovementEnabled = true; // Status pergerakan karakter
    private Vector3 lastPlayerPosition;
    public InventoryPlayer2 inventory; // Referensi ke objek InventoryPlayer2
    private Item.ItemType keyDoorType;
    private bool nearDoor;
    private bool canOpenDoor;
    private KeyDoor currentKeyDoor; // Menyimpan referensi ke pintu saat ini yang dapat dibuka

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
        uI_Inventory.gameObject.SetActive(false);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari GameObject
    }

    void Update()
    {
        // Mendapatkan input dari pemain

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!menuOpened)
            {
                lastPlayerPosition = transform.position;
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        if (playerMovementEnabled && !menuOpened)
        {
            HandleMovementInput();
        }
        else
        {
            HandleInventoryInput();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ItemWorld itemWorld = other.gameObject.GetComponent<ItemWorld>();
            Item item = itemWorld.GetItem();

            if (inventory.GetItemList().Count < 4)
            {
                inventory.AddItem(item);
                itemWorld.DestroySelf();
            }
            else if (item.IsStackable() && inventory.HasItemInInventory(item))
            {
                inventory.AddItem(item);
                itemWorld.DestroySelf();
            }
        }

        KeyDoor keyDoor = other.gameObject.GetComponent<KeyDoor>();
        if (keyDoor != null)
        {
            keyDoorType = keyDoor.GetItemType();
            nearDoor = true;
            currentKeyDoor = keyDoor;
        }
        else
        {
            nearDoor = false;
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Kunci:
                if (nearDoor && keyDoorType == item.itemType)
                {
                    canOpenDoor = true;
                    inventory.RemoveItem(item);
                    if (canOpenDoor && currentKeyDoor != null)
                    {
                        currentKeyDoor.OpenDoor();
                        canOpenDoor = false;
                        currentKeyDoor = null;
                    }

                }
                else
                {
                    canOpenDoor = false;
                    Debug.Log("Pergi kedekat pintu");
                }
                break;
            case Item.ItemType.Buku:
                Debug.Log("Buku digunakan");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Buku, amount = 1 }); // Menghapus buku dari inventori
                break;
            case Item.ItemType.Kunci2:
                if (nearDoor && keyDoorType == item.itemType)
                {
                    canOpenDoor = true;
                    inventory.RemoveItem(item);
                    if (canOpenDoor && currentKeyDoor != null)
                    {
                        currentKeyDoor.OpenDoor();
                        canOpenDoor = false;
                        currentKeyDoor = null;
                    }

                }
                else
                {
                    canOpenDoor = false;
                    Debug.Log("Pergi kedekat pintu");
                }
                break;
        }
    }

    public int GetSelectedItemIndex()
    {
        return selectedItemIndex;
    }

    private void OpenMenu()
    {
        rb.velocity = Vector2.zero; // Hentikan pergerakan
        transform.position = lastPlayerPosition;
        menuOpened = true;
        uI_Inventory.gameObject.SetActive(true); // Mengaktifkan UI InventoryPlayer2
        playerMovementEnabled = false;

    }

    private void CloseMenu()
    {
        menuOpened = false;
        uI_Inventory.gameObject.SetActive(false);
        playerMovementEnabled = true;
    }
    void HandleMovementInput()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

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

        if (playerMovementEnabled)
        {
            // Mengatur kecepatan karakter
            rb.velocity = movement.normalized * speed;
        }
    }



    void HandleInventoryInput()
    {
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

}
