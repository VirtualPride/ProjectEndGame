using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed = 5.0f;
    [HideInInspector] public Rigidbody2D rb;
    private bool menuOpened = false;
    public Inventory inventory;
    public StorageInteract storageInteract;
    public InventoryStorageManager inventoryStorageManager;
    public Player1State player1State;
    private Item.ItemType keyDoorType;
    private bool nearDoor;
    private bool canOpenDoor;
    private bool tradeOpen;
    private bool isItemSelected = false;
    private KeyDoor currentKeyDoor;
    private PlayerInteract playerInteract;
    [HideInInspector] public bool playerMovementEnabled = true;
    [HideInInspector] public Vector3 lastPlayerPosition;
    [SerializeField] private UI_Inventory uI_Inventory;
    [SerializeField] private UI_InventoryStorage uI_InventoryStorage;
    private int selectedItemIndex = 0;
    private int selectedItemIndexPlayer = 0;
    private int selectedItemIndexStorage = 0;
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;

    private void Awake()
    {

        inventory = new Inventory(UseItem);
        playerInteract = GetComponent<PlayerInteract>();
        uI_Inventory.SetInventory(inventory, this);
        uI_InventoryStorage.SetInventory(inventoryStorageManager.inventoryStorage, inventoryStorageManager);
        uI_Inventory.gameObject.SetActive(false);
        uI_InventoryStorage.gameObject.SetActive(false);

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player1State = Player1State.Idle;
    }

    void Update()
    {
        HandleState();
        HandleInput();

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
        menuOpened = true;
        uI_Inventory.gameObject.SetActive(true); // Mengaktifkan UI InventoryPlayer2
    }

    public void CloseMenu()
    {
        menuOpened = false;
        uI_Inventory.gameObject.SetActive(false);
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
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        if (playerMovementEnabled)
        {
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
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedItemIndex < inventory.GetItemList().Count - 1)
            {
                selectedItemIndex++;
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventory.UseItem(selectedItemAt);
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
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndexPlayer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedItemIndexPlayer < inventory.GetItemList().Count - 1)
            {
                selectedItemIndexPlayer++;
                uI_Inventory.SetSelectedItemHighlight(selectedItemIndexPlayer);
            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.C) && storageInteract.isSelecting == false && tradeOpen)
            {
                Item selectedItemAt = inventory.GetItemAtIndex(selectedItemIndexPlayer);
                if (selectedItemAt != null)
                {
                    inventoryStorageManager.inventoryStorage.AddItem(selectedItemAt);
                    inventory.RemoveItem(selectedItemAt);
                }
            }
        }
        else
        {
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
                uI_InventoryStorage.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (selectedItemIndexStorage < inventoryStorageManager.inventoryStorage.GetItemList().Count - 1)
            {
                selectedItemIndexStorage++;
                uI_InventoryStorage.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.C) && storageInteract.isSelecting == false && inventory.GetItemList().Count < 4 && tradeOpen)
            {
                Item selectedItemAt = inventoryStorageManager.inventoryStorage.GetItemAtIndex(selectedItemIndexStorage);

                if (selectedItemAt != null)
                {
                    inventory.AddItem(selectedItemAt);
                    inventoryStorageManager.inventoryStorage.RemoveItem(selectedItemAt);
                }
            }
        }
        else
        {
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

    private void HandleState()
    {
        switch (player1State)
        {
            case Player1State.Idle:
                playerMovementEnabled = true;
                break;
            case Player1State.OpenMenu:
                StopPlayer();
                break;
            case Player1State.SaveItem:
                StopPlayer();
                break;
            case Player1State.RetriveItem:
                StopPlayer();
                break;
            case Player1State.Interact:
                StopPlayer();
                break;
        }
    }

    private void StopPlayer()
    {
        rb.velocity = Vector2.zero;
        transform.position = lastPlayerPosition;
        playerMovementEnabled = false;
    }

    private void HandleInput()
    {

        if (Input.GetKeyDown(KeyCode.X) && player1State == Player1State.Idle && !storageInteract.panelOnPlayer1)
        {
            lastPlayerPosition = transform.position;
            player1State = Player1State.OpenMenu;
            OpenMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X) && player1State == Player1State.OpenMenu)
        {
            player1State = Player1State.Idle;
            CloseMenu();
        }

        if (player1State == Player1State.Idle)
        {
            HandleMovementInput();
        }
        else if (player1State == Player1State.OpenMenu)
        {
            HandleInventoryInput();
        }
        if (player1State == Player1State.SaveItem)
        {
            HandleSaveItemInput();
            if (Input.GetKeyDown(KeyCode.X) && player1State == Player1State.SaveItem)
            {
                player1State = Player1State.Idle;
                CloseTrade();
                CloseMenu();
            }
        }
        else if (player1State == Player1State.RetriveItem)
        {
            HandleRetriveItemInput();
            if (Input.GetKeyDown(KeyCode.X) && player1State == Player1State.RetriveItem)
            {
                player1State = Player1State.Idle;
                CloseTrade();
                CloseMenu();
            }
        }

        if (player1State == Player1State.Idle && Input.GetKeyDown(KeyCode.C))
        {
            playerInteract.DetectInteractableObjects();
            ItemInteract itemInteract = playerInteract.GetInteractableObject() as ItemInteract;
        }
    }


}