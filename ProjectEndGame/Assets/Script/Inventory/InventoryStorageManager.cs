using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStorageManager : MonoBehaviour
{
    [SerializeField] private Player1Controller player1Controller;
    [SerializeField] private Player2Controller player2Controller;
    [HideInInspector] public InventoryStorage inventoryStorage;
    [HideInInspector] public StorageInteract storageInteract;
    private GameObject tradePlayer1;
    private GameObject tradePlayer2;
    private int selectedItemIndex;
    // Start is called before the first frame update
    private void Awake()
    {
        inventoryStorage = new InventoryStorage(UseItem);
        tradePlayer1 = GameObject.Find("UI_InventoryStorage");
        tradePlayer2 = GameObject.Find("UI_InventoryStorage2");
    }
    public int GetSelectedItemIndex()
    {
        if (tradePlayer1.activeSelf)
        {
            selectedItemIndex = player1Controller.GetSelectedItemIndex();
        }
        else if (tradePlayer2.activeSelf)
        {
            selectedItemIndex = player2Controller.GetSelectedItemIndex();
        }
        return selectedItemIndex;
    }
    void UseItem(Item item)
    {

    }
}
