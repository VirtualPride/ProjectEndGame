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

            // Cari objek Player1Controller dalam hierarki
            Player1Controller playerController = FindObjectOfType<Player1Controller>();

            if (playerController != null && item != null)
            {
                if (playerController.inventory.GetItemList().Count < 4)
                {
                    playerController.AddItemToInventory(item); // Tambahkan item ke inventory
                    itemWorld.DestroySelf(); // Hancurkan objek ItemWorld
                }
                else if (item.IsStackable() && playerController.inventory.HasItemInInventory(item))
                {
                    playerController.AddItemToInventory(item); // Tambahkan item ke inventory
                    itemWorld.DestroySelf(); // Hancurkan objek ItemWorld
                }

            }
        }
    }


    public Transform GetTransform()
    {
        return transform;
    }

}

