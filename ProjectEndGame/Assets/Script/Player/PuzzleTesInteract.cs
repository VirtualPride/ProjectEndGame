using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PuzzleTesInteract : MonoBehaviour, IInteractable
{
    [SerializeField] Player1Controller player1Controller;
    [SerializeField] Player2Controller player2Controller;
    [SerializeField] private RuanganTestController ruanganTestController;
    [SerializeField] private PuzzleMove puzzleMove;
    [SerializeField] private ObstacleMove obstacleMove;
    [SerializeField] private CameraManager cameraManager;
    [HideInInspector] public bool player1Move;
    [HideInInspector] public bool player2Move;
    [HideInInspector] public bool inPuzzle;

    private string interactText = "Mulai Puzzle";

    private void Update()
    {
        CheckInPuzzle();
        if (inPuzzle)
        {
            HandleQuitPuzzleInput();
        }
    }
    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        bool player1PuzzleCamActive = cameraManager.player1PuzzleGerakCamera.enabled;
        bool player2PuzzleCamActive = cameraManager.player2PuzzleGerakCamera.enabled;

        if (Input.GetKeyDown(KeyCode.C) && !player1PuzzleCamActive)
        {
            cameraManager.player1PuzzleGerakCamera.enabled = true;
            player1Controller.player1State = Player1State.Interact;
            LastPositionPlayer1();
        }
        else if (Input.GetKeyDown(KeyCode.M) && !player2PuzzleCamActive)
        {
            cameraManager.player2PuzzleGerakCamera.enabled = true;
            player2Controller.player2State = Player2State.Interact;
            LastPositionPlayer2();
        }
    }
    private void LastPositionPlayer1()
    {
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.transform.position = player1Controller.lastPlayerPosition;
    }

    private void LastPositionPlayer2()
    {
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.transform.position = player2Controller.lastPlayerPosition;
    }

    private void CheckInPuzzle()
    {
        if (cameraManager.player1PuzzleGerakCamera.enabled == true || cameraManager.player2PuzzleGerakCamera.enabled == true)
        {
            inPuzzle = true;
        }
        else
        {
            inPuzzle = false;
        }
    }

    private void HandleQuitPuzzleInput()
    {
        if (cameraManager.player1PuzzleGerakCamera.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                cameraManager.player1RuanganTestCam.enabled = true;
                cameraManager.player1PuzzleGerakCamera.enabled = false;
                player1Controller.player1State = Player1State.Idle;
            }
        }
        else if (cameraManager.player2PuzzleGerakCamera.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                cameraManager.player2RuanganTestCam.enabled = true;
                cameraManager.player2PuzzleGerakCamera.enabled = false;
                player2Controller.player2State = Player2State.Idle;
            }
        }
    }



}
