using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractUIPlayer2 : MonoBehaviour
{
    [SerializeField] private new GameObject gameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Player2Controller player2Controller;

    private void Update()
    {

        if (playerInteract.GetInteractableObject() != null && player2Controller.player2State == Player2State.Idle)
        {

            Show(playerInteract.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }
    private void Show(IInteractable interactable)
    {
        gameObject.SetActive(true);
        textMeshProUGUI.text = interactable.GetInteractText();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        textMeshProUGUI.text = "";
    }
}
