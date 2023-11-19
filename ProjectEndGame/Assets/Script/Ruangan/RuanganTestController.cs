using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuanganTestController : MonoBehaviour, IRuangan
{
    [HideInInspector] public bool player1InRoom;
    [HideInInspector] public bool player2InRoom;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Player1Controller player1Controller;
    [SerializeField] private Player2Controller player2Controller;
    public BoxCollider2D roomCollider;
    public Camera RuanganCamGabungan;

    private GameObject batas;

    private GameObject player1;
    private GameObject player2;

    private void Awake()
    {
        CekKamerea();

    }
    private void Update()
    {
        CekRuangan();
    }
    public void CekKamerea()
    {

        player1 = GameObject.Find("Player1_Test");
        player2 = GameObject.Find("Player2_Test");
        batas = GameObject.Find("BarTengah");
    }

    public void CekRuangan()
    {
        player1InRoom = roomCollider.OverlapPoint(player1.transform.position);
        player2InRoom = roomCollider.OverlapPoint(player2.transform.position);

        bool player1Idle = player1Controller.player1State == Player1State.Idle;
        bool player2Idle = player2Controller.player2State == Player2State.Idle;
        bool player1OpenMenu = player1Controller.player1State == Player1State.OpenMenu;
        bool player2OpenMenu = player2Controller.player2State == Player2State.OpenMenu;

        if (player1InRoom && player1Idle || player1InRoom && player1OpenMenu)
        {
            cameraManager.player1RuanganTestCam.enabled = true;
        }
        else
        {
            cameraManager.player1RuanganTestCam.enabled = false;
        }

        if (player2InRoom && player2Idle || player2InRoom && player2OpenMenu)
        {
            cameraManager.player2RuanganTestCam.enabled = true;
        }
        else
        {
            cameraManager.player2RuanganTestCam.enabled = false;
        }

        if (player1InRoom && player2InRoom && player1Idle && player2Idle || player1InRoom && player2InRoom && player1OpenMenu && player2OpenMenu || player1InRoom && player2InRoom && player1Idle && player2OpenMenu || player1InRoom && player2InRoom && player1OpenMenu && player2Idle)
        {
            RuanganCamGabungan.enabled = true;
            batas.SetActive(false);
        }
        else
        {
            RuanganCamGabungan.enabled = false;
            batas.SetActive(true);
        }

        // Check jika tidak ada pemain di ruangan, hidupkan kamera utama
        if (!player1InRoom)
        {
            cameraManager.player1MainCamera.enabled = true;
        }
        else if (!player2InRoom)
        {
            cameraManager.player2MainCamera.enabled = true;
        }

    }

}
