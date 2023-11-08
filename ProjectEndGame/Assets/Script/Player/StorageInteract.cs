using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInteract : MonoBehaviour, IInteractable
{
    private string interactText = "Storage";
    [SerializeField] private GameObject panelPlayer1;
    [SerializeField] private GameObject panelPlayer2;
    [SerializeField] private List<GameObject> buttonOpsiPlayer1 = new List<GameObject>();
    [SerializeField] private List<GameObject> buttonOpsiPlayer2 = new List<GameObject>(); private int selectedButtonIndex = 0;
    [HideInInspector] public bool isSelecting = false;
    [HideInInspector] public bool panelOnPlayer1 = false;
    [HideInInspector] public bool panelOnPlayer2 = false;
    [SerializeField] private Player1Controller player1Controller;
    [SerializeField] private Player2Controller player2Controller;

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
        if (Input.GetKeyDown(KeyCode.C) && !panelOnPlayer2)
        {
            player1Controller.player1State = Player1State.Interact;
            player1Controller.rb.velocity = Vector2.zero;
            player1Controller.lastPlayerPosition = player1Controller.transform.position;
            player1Controller.transform.position = player1Controller.lastPlayerPosition;
            player1Controller.playerMovementEnabled = false;
            panelOnPlayer1 = true;
            panelPlayer1.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && !panelOnPlayer1)
        {
            player2Controller.Player2State = Player2State.Interact;
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
        player1Controller.player1State = Player1State.RetriveItem;
        Player1OpenTrade();
    }

    public void simpanBtnPlayer1()
    {
        player1Controller.player1State = Player1State.SaveItem;
        Player1OpenTrade();
    }

    public void ambilBtnPlayer2()
    {
        player2Controller.Player2State = Player2State.RetriveItem;
        Player2OpenTrade();
    }

    public void simpanBtnPlayer2()
    {
        player2Controller.Player2State = Player2State.SaveItem;
        Player2OpenTrade();
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
            buttonOpsiPlayer1[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
            isSelecting = false;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            isSelecting = false;
            panelPlayer1.SetActive(false);
            Player1Controller player1Controller = FindObjectOfType<Player1Controller>();
            player1Controller.playerMovementEnabled = true;
            player1Controller.player1State = Player1State.Idle;

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

    private void Player1OpenTrade()
    {
        player1Controller.lastPlayerPosition = player1Controller.transform.position;
        player1Controller.OpenMenu();
        player1Controller.OpenTrade();
        panelPlayer1.SetActive(false);
    }

    private void Player2OpenTrade()
    {
        player2Controller.lastPlayerPosition = player2Controller.transform.position;
        player2Controller.OpenMenu();
        player2Controller.OpenTrade();
        panelPlayer2.SetActive(false);
    }


}
