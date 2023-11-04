using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuanganTestController : MonoBehaviour
{
    [HideInInspector] public bool player1InRoom;
    [HideInInspector] public bool player2InRoom;
    public BoxCollider2D roomCollider; // Anda harus menetapkan BoxCollider2D ini dalam inspektor
    public Camera Player1RuanganCam;
    public Camera Player2RuanganCam;
    public Camera RuanganCamGabungan;

    private GameObject batas;

    private GameObject player1;
    private GameObject player2;
    private Camera camera1;
    private Camera camera2;

    private void Start()
    {
        cekCamera();
        disabledCamera();

    }

    private void Update()
    {
        cekRuangan();
    }

    private void cekCamera()
    {
        // Mengambil referensi ke objek Player1 dan Player2
        camera1 = GameObject.Find("Player1Camera").GetComponent<Camera>();
        camera2 = GameObject.Find("Player2Camera").GetComponent<Camera>();
        player1 = GameObject.Find("Player1_Test");
        player2 = GameObject.Find("Player2_Test");
        batas = GameObject.Find("BarTengah");
    }

    private void disabledCamera()
    {
        camera1.enabled = false;
        camera2.enabled = false;
        RuanganCamGabungan.enabled = false;
    }

    private void cekRuangan()
    {
        // Mengecek apakah posisi Player1 berada dalam collider ruangan
        player1InRoom = roomCollider.OverlapPoint(player1.transform.position);

        // Mengecek apakah posisi Player2 berada dalam collider ruangan
        player2InRoom = roomCollider.OverlapPoint(player2.transform.position);

        // Mengatur kamera sesuai dengan status pemain dalam ruangan
        if (player1InRoom)
        {
            Player1RuanganCam.enabled = true;
            camera1.enabled = false;
        }
        else
        {
            camera1.enabled = true;
            Player1RuanganCam.enabled = false;
        }

        if (player2InRoom)
        {
            Player2RuanganCam.enabled = true;
            camera2.enabled = false;
        }
        else
        {
            camera2.enabled = true;
            Player2RuanganCam.enabled = false;
        }

        if (player1InRoom && player2InRoom)
        {
            camera1.enabled = false;
            camera2.enabled = false;
            RuanganCamGabungan.enabled = true;
            batas.SetActive(false);

        }
        else
        {
            RuanganCamGabungan.enabled = false;
            batas.SetActive(true);
        }
        if (!player1InRoom && !player2InRoom)
        {
            // Matikan semua kamera
            Player1RuanganCam.enabled = false;
            Player2RuanganCam.enabled = false;
            RuanganCamGabungan.enabled = false;

            // Aktifkan batas jika ruangan kosong
            batas.SetActive(true);
        }
    }

}
