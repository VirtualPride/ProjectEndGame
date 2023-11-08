using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTesInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Player1Controller player1Controller;
    [SerializeField] private Player2Controller player2Controller;
    [SerializeField] private Camera puzzleGerakCamPlayer1;
    [SerializeField] private Camera puzzleGerakCamPlayer2;
    [SerializeField] private RuanganTestController ruanganTestController;
    [SerializeField] public bool player1Move = false;
    [SerializeField] public bool player2Move = false;
    [SerializeField] private PlayerInteractUI playerInteractUI;
    private string interactText = "Mulai";

    private void Awake()
    {
        puzzleGerakCamPlayer1.enabled = false;
        puzzleGerakCamPlayer2.enabled = false;
    }
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
                MovementDisabled();
                playerInteractUI.IsInteracting();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                CameraOn();
                player1Controller.playerMovementEnabled = false;
                MovementDisabled();
                playerInteractUI.IsInteracting();
            }
        }

    }
    private void HandleQuitPuzzleInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            CmaeraOff();
            player1Controller.playerMovementEnabled = true;
            player2Controller.playerMovementEnabled = true;

        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            CmaeraOff();
            player1Controller.playerMovementEnabled = true;
            player2Controller.playerMovementEnabled = true;
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

    private void MovementDisabled()
    {
        player2Controller.rb.velocity = Vector2.zero;
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.transform.position = player2Controller.lastPlayerPosition;
        player2Controller.playerMovementEnabled = false;
        player1Controller.rb.velocity = Vector2.zero;
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.transform.position = player1Controller.lastPlayerPosition;
        player1Controller.playerMovementEnabled = false;
    }
}
