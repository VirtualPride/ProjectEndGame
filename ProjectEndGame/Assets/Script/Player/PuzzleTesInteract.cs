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
        if (player1Move == true || player2Move == true)
        {
            HandleQuitPuzzleInput();
            Debug.Log("ini hidup");
        }
    }
    public void Interact()
    {
        if (ruanganTestController.player1InRoom && ruanganTestController.player2InRoom)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                MovementDisabled();
                CameraOn();
                player1Move = true;
                LastPosition();
                MovementDisabled();

                playerInteractUI.IsInteracting();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                MovementDisabled();
                CameraOn();
                player2Move = true;
                LastPosition();
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
            MovementEnabled();
            if (player1Move == true)
            {
                player1Move = false;
            }
            else if (player2Move == true)
            {
                player2Move = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {

            CmaeraOff();
            MovementEnabled();
            if (player1Move == true)
            {
                player1Move = false;
            }
            else if (player2Move == true)
            {
                player2Move = false;
            }
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
        player1Controller.player1State = Player1State.Interact;
        player2Controller.player2State = Player2State.Interact;
    }
    private void MovementEnabled()
    {
        player1Controller.player1State = Player1State.Idle;
        player2Controller.player2State = Player2State.Idle;
    }

    private void LastPosition()
    {
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.transform.position = player1Controller.lastPlayerPosition;
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.transform.position = player2Controller.lastPlayerPosition;
    }
}
