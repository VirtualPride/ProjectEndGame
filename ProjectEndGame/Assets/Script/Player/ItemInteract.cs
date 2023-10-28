using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteract : MonoBehaviour, IInteractable
{
    private string interactText = "ambil";

    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        ItemWorld itemWorld = gameObject.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            Item item = itemWorld.GetItem(); // Dapatkan item dari ItemWorld

            Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
            Player2Controller player2Controller = FindObjectOfType<Player2Controller>();

            if (item != null)
            {

                if (Input.GetKeyDown(KeyCode.C))
                {
                    // Jika player 1 lebih dekat, tambahkan item ke inventory Player 1
                    if (player1Controller != null && player1Controller.inventory.GetItemList().Count < 4)
                    {
                        player1Controller.AddItemToInventory(item);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.M))
                {
                    // Jika player 2 lebih dekat, tambahkan item ke inventory Player 2
                    if (player2Controller != null && player2Controller.inventoryPlayer2.GetItemList().Count < 4)
                    {
                        player2Controller.AddItemToInventory(item);
                    }
                }

                itemWorld.DestroySelf(); // Hancurkan objek ItemWorld
            }
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
