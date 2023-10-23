using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInteract : MonoBehaviour, IInteractable
{
    private string interactText = "Storage";
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {

        Player1Controller playerController = FindObjectOfType<Player1Controller>();
        playerController.lastPlayerPosition = playerController.transform.position;
        playerController.OpenMenu();
        playerController.OpenTrade();

    }
    public Transform GetTransform()
    {
        return transform;
    }
}
