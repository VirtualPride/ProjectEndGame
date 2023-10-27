using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInteract : MonoBehaviour, IInteractable
{
    private string interactText = "Storage";
    [SerializeField]
    private GameObject panelPlayer1;
    [SerializeField]
    private GameObject panelPlayer2;
    [SerializeField]
    private List<GameObject> buttonOpsiPlayer1 = new List<GameObject>();
    [SerializeField]
    private List<GameObject> buttonOpsiPlayer2 = new List<GameObject>();

    private int selectedButtonIndex = 0;
    [HideInInspector]
    public bool isSelecting = false;
    [HideInInspector]
    public bool panelOnPlayer1 = false;
    [HideInInspector]
    public bool panelOnPlayer2 = false;

    public string GetInteractText()
    {
        return interactText;
    }

    private void Awake()
    {
        panelPlayer1.SetActive(false);
        panelPlayer2.SetActive(false);
    }
    private void Update()
    {
        if (panelPlayer1.activeSelf == true)
        {
            HandleButtonSelectionInput();
            panelOnPlayer1 = true;
        }
        else
        {
            panelOnPlayer1 = false;
        }
        if (panelPlayer2.activeSelf == true)
        {
            HandleButtonSelectionInputPlayer2();
            panelOnPlayer2 = true;
        }
        else
        {
            panelOnPlayer2 = false;
        }

    }
    public void Interact()
    {

        Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
        Player2Controller player2Controller = FindObjectOfType<Player2Controller>();
        float distanceToPlayer1 = Vector3.Distance(player1Controller.transform.position, transform.position);
        float distanceToPlayer2 = Vector3.Distance(player2Controller.transform.position, transform.position);
        if (distanceToPlayer1 < distanceToPlayer2)
        {
            player1Controller.rb.velocity = Vector2.zero;
            player1Controller.lastPlayerPosition = player1Controller.transform.position;
            player1Controller.transform.position = player1Controller.lastPlayerPosition;
            player1Controller.playerMovementEnabled = false;
            panelOnPlayer1 = true;
            panelPlayer1.SetActive(true);
        }
        else
        {
            player2Controller.rb.velocity = Vector2.zero;
            player2Controller.lastPlayerPosition = player2Controller.transform.position;
            player2Controller.transform.position = player2Controller.lastPlayerPosition;
            player2Controller.playerMovementEnabled = false;
            panelOnPlayer2 = true;
            panelPlayer2.SetActive(true);
        }

        isSelecting = true;

    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void ambilBtnPlayer1()
    {
        Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.ambilStorage = true;
        player1Controller.simpanStorage = false;
        player1Controller.OpenMenu();
        player1Controller.OpenTrade();
        panelPlayer1.SetActive(false);
    }

    public void simpanBtnPlayer1()
    {
        Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.ambilStorage = false;
        player1Controller.simpanStorage = true;
        player1Controller.OpenMenu();
        player1Controller.OpenTrade();
        panelPlayer1.SetActive(false);
    }

    public void ambilBtnPlayer2()
    {
        Player2Controller player2Controller = FindObjectOfType<Player2Controller>();
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.ambilStorage = true;
        player2Controller.simpanStorage = false;
        player2Controller.OpenMenu();
        player2Controller.OpenTrade();
        panelPlayer2.SetActive(false);
    }

    public void simpanBtnPlayer2()
    {
        Player2Controller player2Controller = FindObjectOfType<Player2Controller>();
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.ambilStorage = false;
        player2Controller.simpanStorage = true;
        player2Controller.OpenMenu();
        player2Controller.OpenTrade();
        panelPlayer2.SetActive(false);
    }

    private void UpdateButtonHighlight()
    {
        if (panelOnPlayer1)
        {
            for (int i = 0; i < buttonOpsiPlayer1.Count; i++)
            {
                Image buttonImage = buttonOpsiPlayer1[i].GetComponent<Image>();
                if (i == selectedButtonIndex)
                {
                    // Jika tombol dipilih, ubah warna tombol menjadi lebih gelap
                    buttonImage.color = Color.white;
                }
                else
                {
                    // Jika tombol tidak dipilih, kembalikan warna aslinya
                    buttonImage.color = Color.gray;
                }
            }
        }
        else if (panelOnPlayer2)
        {
            for (int i = 0; i < buttonOpsiPlayer2.Count; i++)
            {
                Image buttonImage = buttonOpsiPlayer2[i].GetComponent<Image>();
                if (i == selectedButtonIndex)
                {
                    // Jika tombol dipilih, ubah warna tombol menjadi lebih gelap
                    buttonImage.color = Color.white;
                }
                else
                {
                    // Jika tombol tidak dipilih, kembalikan warna aslinya
                    buttonImage.color = Color.gray;
                }
            }
        }

    }

    private void SelectNextButton()
    {
        selectedButtonIndex = 1;
        UpdateButtonHighlight();
    }

    private void SelectPreviousButton()
    {
        selectedButtonIndex = 0;
        UpdateButtonHighlight();
    }

    public void HandleButtonSelectionInput()
    {
        // Tombol-tombol navigasi keyboard, misalnya "A" dan "D"
        if (Input.GetKeyDown(KeyCode.A))
        {
            SelectPreviousButton();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SelectNextButton();
        }
        else if (Input.GetKeyDown(KeyCode.C) && isSelecting)
        {
            // Tombol "C" untuk memilih tombol yang dipilih
            buttonOpsiPlayer1[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
            isSelecting = false; // Setel kembali isSelecting ke false setelah memilih
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            isSelecting = false;
            panelPlayer1.SetActive(false);
            Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
            player1Controller.playerMovementEnabled = true;
        }

    }
    public void HandleButtonSelectionInputPlayer2()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SelectPreviousButton();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SelectNextButton();
        }
        else if (Input.GetKeyDown(KeyCode.M) && isSelecting)
        {
            buttonOpsiPlayer2[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
            isSelecting = false;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            isSelecting = false;
            panelPlayer2.SetActive(false);
            Player2Controller player2Controller = FindObjectOfType<Player2Controller>();
            player2Controller.playerMovementEnabled = true;
        }
    }
}
