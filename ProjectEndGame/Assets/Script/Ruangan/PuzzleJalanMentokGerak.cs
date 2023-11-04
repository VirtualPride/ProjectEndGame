using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleJalanMentokGerak : MonoBehaviour
{

    public BoxCollider2D roomCollider; // Anda harus menetapkan BoxCollider2D ini dalam inspektor
    public Camera Player1RuanganCam;
    public Camera Player2RuanganCam;

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
    }


}
