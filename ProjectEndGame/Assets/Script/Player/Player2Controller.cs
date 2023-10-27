using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5.0f; // Kecepatan karakter
    [HideInInspector]
    public Rigidbody2D rb;
    private bool menuOpened = false;
    // Status menu inventoryPlayer2
    public InventoryPlayer2 inventoryPlayer2; // Referensi ke objek InventoryPlayer2
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
    private UI_InventoryPlayer2 UI_InventoryPlayer2; // Referensi ke UI InventoryPlayer2
    [SerializeField]
    private UI_InventoryStoragePlayer2 uI_InventoryStorage;
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
    public KeyCode moveLeftKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;

    private void Awake()
    {
        // Membuat instance baru dari InventoryPlayer2 dan menghubungkannya dengan inventoryPlayer2
        inventoryPlayer2 = new InventoryPlayer2(UseItem);
        inventoryStorage = new InventoryStorage(UseItem);
        playerInteract = GetComponent<PlayerInteract>();


        // Mengatur inventoryPlayer2 UI dengan inventoryPlayer2 yang telah dibuat dan mengirimkan referensi ke Player2Controller
        UI_InventoryPlayer2.SetInventory(inventoryPlayer2, this);
        uI_InventoryStorage.SetInventory(inventoryStorage, this);
        UI_InventoryPlayer2.gameObject.SetActive(false);
        uI_InventoryStorage.gameObject.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Mengambil komponen Rigidbody2D dari GameObject
    }

    void Update()
    {
        // Mendapatkan input dari pemain
        if (Input.GetKeyDown(KeyCode.N) && !tradeOpen && !storageInteract.panelOnPlayer1)
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                CloseTrade();
                CloseMenu();
            }
        }
        else if (!playerMovementEnabled && menuOpened && tradeOpen && ambilStorage)
        {
            HandleRetriveItemInput();
            if (Input.GetKeyDown(KeyCode.N))
            {
                CloseTrade();
                CloseMenu();
            }
        }

        if (!menuOpened)
        {
            if (Input.GetKeyDown(KeyCode.M))
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
                inventoryPlayer2.RemoveItem(new Item { itemType = Item.ItemType.Buku, amount = 1 }); // Menghapus buku dari inventori
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
        UI_InventoryPlayer2.gameObject.SetActive(true); // Mengaktifkan UI InventoryPlayer2
        playerMovementEnabled = false;
    }

    public void CloseMenu()
    {
        menuOpened = false;
        UI_InventoryPlayer2.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
                Debug.Log("Selected item : " + inventoryPlayer2.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                UI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndex < inventoryPlayer2.GetItemList().Count - 1)
            {
                selectedItemIndex++;
                Debug.Log("Selected item : " + inventoryPlayer2.GetItemAtIndex(selectedItemIndex).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select
                UI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndex);
            }
        }

        // Memeriksa input untuk menggunakan item dengan tombol "M"
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Mendapatkan item yang sedang dipilih
            Item selectedItemAt = inventoryPlayer2.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventoryPlayer2.UseItem(selectedItemAt); // Menggunakan item yang dipilih
            }
        }
    }
    public void UseKey(Item item)
    {
        if (nearDoor && keyDoorType == item.itemType)
        {
            canOpenDoor = true;
            inventoryPlayer2.RemoveItem(item);
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
        inventoryPlayer2.AddItem(item);
    }

    public void HandleSaveItemInput()
    {
        uI_InventoryStorage.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndexPlayer > 0)
            {
                selectedItemIndexPlayer--;
                Debug.Log("Selected item : " + inventoryPlayer2.GetItemAtIndex(selectedItemIndexPlayer).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Player
                UI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndexPlayer);

            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndexPlayer < inventoryPlayer2.GetItemList().Count - 1)
            {
                selectedItemIndexPlayer++;
                Debug.Log("Selected item : " + inventoryPlayer2.GetItemAtIndex(selectedItemIndexPlayer).itemType);

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Player
                UI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndexPlayer);


            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.M) && storageInteract.isSelecting == false && tradeOpen)
            {
                // Mendapatkan item yang sedang dipilih dari inventoryPlayer2 UI Player
                Item selectedItemAt = inventoryPlayer2.GetItemAtIndex(selectedItemIndexPlayer);

                if (selectedItemAt != null)
                {
                    inventoryStorage.AddItem(selectedItemAt); // Menggunakan item yang dipilih
                    inventoryPlayer2.RemoveItem(selectedItemAt);


                }
            }
        }
        else
        {
            // Jika pemain belum memilih item, izinkan mereka untuk memilih item terlebih dahulu.
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) && tradeOpen)
            {
                isItemSelected = true;
            }
        }
    }

    public void HandleRetriveItemInput()
    {
        UI_InventoryPlayer2.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndexStorage > 0)
            {
                selectedItemIndexStorage--;

                // Panggil SetSelectedItemHighlight untuk mengatur tampilan gambar select di UI Storage
                uI_InventoryStorage.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
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
            if (Input.GetKeyDown(KeyCode.M) && storageInteract.isSelecting == false && inventoryPlayer2.GetItemList().Count < 4 && tradeOpen)
            {
                // Mendapatkan item yang sedang dipilih dari inventoryPlayer2 UI Storage
                Item selectedItemAt = inventoryStorage.GetItemAtIndex(selectedItemIndexStorage);

                if (selectedItemAt != null)
                {
                    inventoryPlayer2.AddItem(selectedItemAt);
                    inventoryStorage.RemoveItem(selectedItemAt); // Menggunakan item yang dipilih


                }
            }
        }
        else
        {
            // Jika pemain belum memilih item, izinkan mereka untuk memilih item terlebih dahulu.
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) && tradeOpen)
            {
                isItemSelected = true;
            }
        }
    }



}
