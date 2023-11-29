using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera player1MainCamera;
    public Camera player2MainCamera;
    public Camera player1PuzzleGerakCamera;
    public Camera player2PuzzleGerakCamera;
    public Camera player1RuanganTestCam;
    public Camera player2RuanganTestCam;
    public Camera RuanganCamTestGabungan;
    [HideInInspector] public bool otherPlayer1CameraOn = false;
    [HideInInspector] public bool otherPlayer2CameraOn = false;

    private void Awake()
    {
        player1MainCamera.enabled = true;
        player2MainCamera.enabled = true;
        player1PuzzleGerakCamera.enabled = false;
        player1RuanganTestCam.enabled = false;
        player2PuzzleGerakCamera.enabled = false;
        player2RuanganTestCam.enabled = false;
        RuanganCamTestGabungan.enabled = false;
    }

    private void Update()
    {
        // CheckOtherPlayer1Camera();
        // CheckOtherPlayer2Camera();
    }

    public void CheckOtherPlayer1Camera()
    {
        if (player1MainCamera.enabled == true || player1PuzzleGerakCamera.enabled == true || player1RuanganTestCam.enabled == true)
        {
            player1MainCamera.enabled = false;
        }
        else
        {
            player1MainCamera.enabled = true;
        }
    }

    public void CheckOtherPlayer2Camera()
    {
        if (player2MainCamera.enabled == true || player2PuzzleGerakCamera.enabled == true || player2RuanganTestCam.enabled == true)
        {
            player2MainCamera.enabled = false;
        }
        else
        {
            player2MainCamera.enabled = true;
        }
    }

}
