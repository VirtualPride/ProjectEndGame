using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    [HideInInspector]
    public Rigidbody2D rb;
    private bool menuOpened = false;
    // Status menu inventory
    public Inventory inventory; // Referensi ke objek InventoryPlayer2
    public InventoryStorage inventoryStorage;
    public StorageInteract storageInteract;
    private Item.ItemType keyDoorType;
    private bool nearDoor;
    private bool canOpenDoor;
    private bool tradeOpen;
    private bool isItemSelected = false;
    private KeyDoor currentKeyDoor; // Menyimpan referensi ke pintu saat ini yang dapat dibuka
    private PlayerInteract playerInteract;
    [HideInInspector]
    public bool playerMovementEnabled = true; // Status pergerakan karakter
    [HideInInspector]
    public Vector3 lastPlayerPosition;
    [SerializeField]
    private UI_Inventory uI_Inventory; // Referensi ke UI InventoryPlayer2
    [SerializeField]
    private UI_InventoryStorage uI_InventoryStorage;
    [SerializeField]
    public bool ambilStorage = false;
    [SerializeField]
    public bool simpanStorage = false;
    private int selectedItemIndex = 0; // Indeks item yang sedang dipilih dalam inventori
    private int selectedItemIndexPlayer = 0; // Indeks item yang dipilih dalam UI Player
    private int selectedItemIndexStorage = 0;


    // Tombol-tombol pergerakan yang dapat dikustomisasi oleh pemain
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private void Awake()
    {
        // Membuat instance baru dari InventoryPlayer2 dan menghubungkannya dengan inventory
        inventory = new Inventory(UseItem);
        inventoryStorage = new InventoryStorage(UseItem);
        playerInteract = GetComponent<PlayerInteract>();


        // Mengatur inventory UI dengan inventory yang telah dibuat dan mengirimkan referensi ke Player2Controller
        uI_Inventory.SetInventory(inventory, this);
        uI_InventoryStorage.SetInventory(inventoryStorage, this);
        uI_Inventory.gameObject.SetActive(false);
        uI_InventoryStorage.gameObject.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari GameObject
    }

    void Update()
    {
        // Mendapatkan input dari pemain
        if (Input.GetKeyDown(KeyCode.X) && !tradeOpen && !storageInteract.panelOnPlayer1)
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
        else if (!playerMovementEnabled && menuOpened && !tradeOpen)
        {
            HandleInventoryInput();
        }
        if (!playerMovementEnabled && menuOpened && tradeOpen && simpanStorage)
        {
            HandleSaveItemInput();
            if (Input.GetKeyDown(KeyCode.X))
            {
                CloseTrade();
                CloseMenu();
            }
        }
        else if (!playerMovementEnabled && menuOpened && tradeOpen && ambilStorage)
        {
            HandleRetriveItemInput();
            if (Input.GetKeyDown(KeyCode.X))
            {
                CloseTrade();
                CloseMenu();
            }
        }

        if (!menuOpened)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                playerInteract.DetectInteractableObjects();
                ItemInteract itemInteract = playerInteract.GetInteractableObject() as ItemInteract;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

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
                UseKey(item);
                break;
            case Item.ItemType.Buku:
                Debug.Log("Buku digunakan");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.Buku, amount = 1 }); // Menghapus buku dari inventori
                break;
            case Item.ItemType.Kunci2:
                UseKey(item);
                break;
        }
    }

    public int GetSelectedItemIndex()
    {
        return selectedItemIndex;
    }

    public void OpenMenu()
    {
        rb.velocity = Vector2.zero; // Hentikan pergerakan
        transform.position = lastPlayerPosition;
        menuOpened = true;
        uI_Inventory.gameObject.SetActive(true); // Mengaktifkan UI InventoryPlayer2
        playerMovementEnabled = false;
    }

    public void CloseMenu()
    {
        menuOpened = false;
        uI_Inventory.gameObject.SetActive(false);
        playerMovementEnabled = true;
    }

    public void OpenTrade()
    {
        uI_InventoryStorage.gameObject.SetActive(true);
        tradeOpen = true;
        isItemSelected = false;
    }

    public void CloseTrade()
    {
        uI_InventoryStorage.gameObject.SetActive(false);
        tradeOpen = false;
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Mendapatkan item yang sedang dipilih
            Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventory.UseItem(selectedItemAt); // Menggunakan item yang dipilih
            }
        }
    }
    public void UseKey(Item item)
    {
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
    }

    public void AddItemToInventory(Item item)
    {
        inventory.AddItem(item);
    }

    public void HandleSaveItemInput()
    {
        uI_InventoryStorage.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectedItemIndexPlayer > 0)
            {
                selectedItemIndexPlayer--;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndexPlayer).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Player
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndexPlayer);

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedItemIndexPlayer < inventory.GetItemList().Count - 1)
            {
                selectedItemIndexPlayer++;
                Debug.Log("Selected item : " + inventory.GetItemAtIndex(selectedItemIndexPlayer).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Player
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndexPlayer);


            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.C) && storageInteract.isSelecting == false && tradeOpen)
            {
                // Mendapatkan item yang sedang dipilih dari inventory UI Player
                Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndexPlayer);

                if (selectedItemAt != null)
                {
                    inventoryStorage.AddItem(selectedItemAt); // Menggunakan item yang dipilih
                    inventory.RemoveItem(selectedItemAt);


                }
            }
        }
        else
        {
            // Jika pemain belum memilih item, izinkan mereka untuk memilih item terlebih dahulu.
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && tradeOpen)
            {
                isItemSelected = true;
            }
        }
    }

    public void HandleRetriveItemInput()
    {
        uI_Inventory.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (selectedItemIndexStorage > 0)
            {
                selectedItemIndexStorage--;

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Storage
                uI_InventoryStorage.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedItemIndexStorage < inventoryStorage.GetItemList().Count - 1)
            {
                selectedItemIndexStorage++;

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Storage
                uI_InventoryStorage.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.C) && storageInteract.isSelecting == false && inventory.GetItemList().Count < 4 && tradeOpen)
            {
                // Mendapatkan item yang sedang dipilih dari inventory UI Storage
                Item selectedItemAt = inventoryStorage.GetItemAtIndex(selectedItemIndexStorage);

                if (selectedItemAt != null)
                {
                    inventory.AddItem(selectedItemAt);
                    inventoryStorage.RemoveItem(selectedItemAt); // Menggunakan item yang dipilih


                }
            }
        }
        else
        {
            // Jika pemain belum memilih item, izinkan mereka untuk memilih item terlebih dahulu.
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) && tradeOpen)
            {
                isItemSelected = true;
            }
        }
    }

    public bool IsInteracting()
    {
        if (!menuOpened)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                playerInteract.DetectInteractableObjects();
                ItemInteract itemInteract = playerInteract.GetInteractableObject() as ItemInteract;
            }
        }
        return IsInteracting();
    }


}
