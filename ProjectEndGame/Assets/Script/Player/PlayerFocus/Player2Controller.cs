using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed = 5.0f;
    [HideInInspector] public Rigidbody2D rb;
    private bool menuOpened = false;
    public InventoryPlayer2 inventoryPlayer2;
    public StorageInteract storageInteract;
    public InventoryStorageManager inventoryStorageManager;
    public Player2State player2State;
    private Item.ItemType keyDoorType;
    private bool nearDoor;
    private bool canOpenDoor;
    private bool tradeOpen;
    private bool isItemSelected = false;
    private KeyDoor currentKeyDoor;
    private PlayerInteract playerInteract;
    [HideInInspector] public bool playerMovementEnabled = true;
    [HideInInspector] public Vector3 lastPlayerPosition;
    [SerializeField] private UI_InventoryPlayer2 uI_InventoryPlayer2;
    [SerializeField] private UI_InventoryStorage2 uI_InventoryStorage2;
    private int selectedItemIndex = 0;
    private int selectedItemIndexPlayer = 0;
    private int selectedItemIndexStorage = 0;
    public KeyCode moveUpKey = KeyCode.UpArrow;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.LeftArrow;
    public KeyCode moveRightKey = KeyCode.RightArrow;

    private void Awake()
    {
        inventoryPlayer2 = new InventoryPlayer2(UseItem);
        GameObject inventoryStorageGO = GameObject.Find("UI_InventoryStorage2");
        if (inventoryStorageGO != null)
        {
            uI_InventoryStorage2 = inventoryStorageGO.AddComponent<UI_InventoryStorage2>();
        }
        else
        {
            Debug.LogError("GameObject 'InventoryStorageGameObject' not found.");
        }

        playerInteract = GetComponent<PlayerInteract>();
        uI_InventoryPlayer2.SetInventory(inventoryPlayer2, this);
        uI_InventoryStorage2.SetInventory(inventoryStorageManager.inventoryStorage, inventoryStorageManager);
        uI_InventoryPlayer2.gameObject.SetActive(false);
        uI_InventoryStorage2.gameObject.SetActive(false);


    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player2State = Player2State.Idle;
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
        menuOpened = true;
        uI_InventoryPlayer2.gameObject.SetActive(true); // Mengaktifkan UI inventoryPlayer2
    }

    public void CloseMenu()
    {
        menuOpened = false;
        uI_InventoryPlayer2.gameObject.SetActive(false);
    }

    public void OpenTrade()
    {
        uI_InventoryStorage2.gameObject.SetActive(true);
        tradeOpen = true;
        isItemSelected = false;
    }

    public void CloseTrade()
    {
        uI_InventoryStorage2.gameObject.SetActive(false);
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndex > 0)
            {
                selectedItemIndex--;
                uI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndex < inventoryPlayer2.GetItemList().Count - 1)
            {
                selectedItemIndex++;
                uI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndex);
            }
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Item selectedItemAt = inventoryPlayer2.GetItemAtIndex(selectedItemIndex);

            if (selectedItemAt != null)
            {
                inventoryPlayer2.UseItem(selectedItemAt);
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
        }
    }

    public void AddItemToInventory(Item item)
    {
        inventoryPlayer2.AddItem(item);
    }

    public void HandleSaveItemInput()
    {
        uI_InventoryStorage2.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndexPlayer > 0)
            {
                selectedItemIndexPlayer--;
                uI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndexPlayer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndexPlayer < inventoryPlayer2.GetItemList().Count - 1)
            {
                selectedItemIndexPlayer++;
                uI_InventoryPlayer2.SetSelectedItemHighlight(selectedItemIndexPlayer);
            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.M) && storageInteract.isSelecting == false && tradeOpen)
            {
                Item selectedItemAt = inventoryPlayer2.GetItemAtIndex(selectedItemIndexPlayer);
                if (selectedItemAt != null)
                {
                    inventoryStorageManager.inventoryStorage.AddItem(selectedItemAt);
                    inventoryPlayer2.RemoveItem(selectedItemAt);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) && tradeOpen)
            {
                isItemSelected = true;
            }
        }
    }

    public void HandleRetriveItemInput()
    {
        uI_InventoryPlayer2.SetSelectedItemHighlight(-1);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedItemIndexStorage > 0)
            {
                selectedItemIndexStorage--;
                uI_InventoryStorage2.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedItemIndexStorage < inventoryStorageManager.inventoryStorage.GetItemList().Count - 1)
            {
                selectedItemIndexStorage++;
                uI_InventoryStorage2.SetSelectedItemHighlight(selectedItemIndexStorage);

            }
        }
        if (isItemSelected)
        {
            if (Input.GetKeyDown(KeyCode.M) && storageInteract.isSelecting == false && inventoryPlayer2.GetItemList().Count < 4 && tradeOpen)
            {
                Item selectedItemAt = inventoryStorageManager.inventoryStorage.GetItemAtIndex(selectedItemIndexStorage);

                if (selectedItemAt != null)
                {
                    inventoryPlayer2.AddItem(selectedItemAt);
                    inventoryStorageManager.inventoryStorage.RemoveItem(selectedItemAt);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) && tradeOpen)
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
        switch (player2State)
        {
            case Player2State.Idle:
                playerMovementEnabled = true;
                break;
            case Player2State.OpenMenu:
                StopPlayer();
                break;
            case Player2State.SaveItem:
                StopPlayer();
                break;
            case Player2State.RetriveItem:
                StopPlayer();
                break;
            case Player2State.Interact:
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
        if (Input.GetKeyDown(KeyCode.N) && player2State == Player2State.Idle && !storageInteract.panelOnPlayer2)
        {
            lastPlayerPosition = transform.position;
            player2State = Player2State.OpenMenu;
            OpenMenu();
        }
        else if (Input.GetKeyDown(KeyCode.N) && player2State == Player2State.OpenMenu)
        {
            player2State = Player2State.Idle;
            CloseMenu();
        }

        if (player2State == Player2State.Idle)
        {
            HandleMovementInput();
        }
        else if (player2State == Player2State.OpenMenu)
        {
            HandleInventoryInput();
        }
        if (player2State == Player2State.SaveItem)
        {
            HandleSaveItemInput();
            if (Input.GetKeyDown(KeyCode.N) && player2State == Player2State.SaveItem)
            {
                player2State = Player2State.Idle;
                CloseTrade();
                CloseMenu();
            }
        }
        else if (player2State == Player2State.RetriveItem)
        {
            HandleRetriveItemInput();
            if (Input.GetKeyDown(KeyCode.N) && player2State == Player2State.RetriveItem)
            {
                player2State = Player2State.Idle;
                CloseTrade();
                CloseMenu();
            }
        }

        if (player2State == Player2State.Idle)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                playerInteract.DetectInteractableObjects();
                ItemInteract itemInteract = playerInteract.GetInteractableObject() as ItemInteract;
            }
        }
    }

}
