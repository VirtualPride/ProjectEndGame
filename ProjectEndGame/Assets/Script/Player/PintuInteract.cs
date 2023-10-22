using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PintuInteract : MonoBehaviour, IInteractable
{
    private string interactText = "buka pintu";
    public string GetInteractText()
    {
        return interactText;
    }

    public void Interact()
    {
        Debug.Log("Interact with door");
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
