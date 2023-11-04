using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTesInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Camera puzzleGerakCamPlayer1;
    [SerializeField] private Camera puzzleGerakCamPlayer2;
    [SerializeField] private RuanganTestController ruanganTestController;
    [SerializeField] public bool player1Move = false;
    [SerializeField] public bool player2Move = false;
    private string interactText = "Mulai";

    private void Update()
    {
        HandleQuitPuzzleInput();
    }
    public void Interact()
    {
        if (ruanganTestController.player1InRoom && ruanganTestController.player2InRoom)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                CameraOn();
                player1Move = true;
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                CameraOn();
                player2Move = true;
            }
        }

    }
    private void HandleQuitPuzzleInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CmaeraOff();
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            CmaeraOff();
        }
    }
    public Transform GetTransform()
    {
        return transform;
    }

    public string GetInteractText()
    {
        return interactText;
    }
    private void CameraOn()
    {
        puzzleGerakCamPlayer1.enabled = true;
        puzzleGerakCamPlayer2.enabled = true;
    }
    private void CmaeraOff()
    {
        puzzleGerakCamPlayer1.enabled = false;
        puzzleGerakCamPlayer2.enabled = false;
    }
}
