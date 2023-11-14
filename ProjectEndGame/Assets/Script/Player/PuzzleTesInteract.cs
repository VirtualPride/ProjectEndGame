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
    [SerializeField] private PuzzleMove puzzleMove;
    [SerializeField] private ObstacleMove obstacleMove;
    public bool inPuzzle = false;
    private string interactText = "Mulai";

    private void Awake()
    {
        puzzleGerakCamPlayer1.enabled = false;
        puzzleGerakCamPlayer2.enabled = false;
    }
    private void Update()
    {
        CheckInPuzzle();
        if (inPuzzle == true)
        {
            if (player1Move == true || player2Move == true)
            {
                HandleQuitPuzzleInput();
            }

            if (puzzleMove.isFinish == true)
            {
                CameraOff();
                MovementEnabled();
                interactText = "Puzzle Selesai";
            }


        }

    }
    public void Interact()
    {
        if (puzzleMove.isFinish == false)
        {
            if (ruanganTestController.player1InRoom && ruanganTestController.player2InRoom)
            {
                interactText = "Mulai";
                if (Input.GetKeyDown(KeyCode.C))
                {
                    MovementDisabled();
                    CameraOn();
                    player1Move = true;
                    LastPosition();
                    MovementDisabled();
                }
                else if (Input.GetKeyDown(KeyCode.M))
                {
                    MovementDisabled();
                    CameraOn();
                    player2Move = true;
                    LastPosition();
                    MovementDisabled();
                }
            }
            else
            {
                interactText = "Bawa Temanmu";
            }
        }
    }
    private void HandleQuitPuzzleInput()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
           
            CameraOff();
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
     
            CameraOff();
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
    private void CameraOff()
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

    private void CheckInPuzzle()
    {
        if (puzzleGerakCamPlayer1.enabled == true && puzzleGerakCamPlayer2.enabled == true)
        {
            inPuzzle = true;
        }
        else
        {
            inPuzzle = false;
        }
    }
}
