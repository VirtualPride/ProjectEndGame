using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInteract : MonoBehaviour, IInteractable
{
    private string interactText = "Storage";
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private List<GameObject> buttonOpsi = new List<GameObject>();
    private int selectedButtonIndex = 0;
    [HideInInspector]
    public bool isSelecting = false;

    public string GetInteractText()
    {
        return interactText;
    }

    private void Awake()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        if (panel.activeSelf == true)
        {
            HandleButtonSelectionInput();
        }

    }
    public void Interact()
    {
        Player1Controller playerController = FindObjectOfType<Player1Controller>();
        playerController.lastPlayerPosition = playerController.transform.position;
        playerController.playerMovementEnabled = false;
        panel.SetActive(true);
        isSelecting = true;

    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void ambilBtn()
    {
        Player1Controller playerController = FindObjectOfType<Player1Controller>();
        playerController.lastPlayerPosition = playerController.transform.position;
        playerController.ambilStorage = true;
        playerController.simpanStorage = false;
        playerController.OpenMenu();
        playerController.OpenTrade();
        panel.SetActive(false);
    }

    public void simpanBtn()
    {
        Player1Controller playerController = FindObjectOfType<Player1Controller>();
        playerController.lastPlayerPosition = playerController.transform.position;
        playerController.ambilStorage = false;
        playerController.simpanStorage = true;
        playerController.OpenMenu();
        playerController.OpenTrade();
        panel.SetActive(false);
    }

    private void UpdateButtonHighlight()
    {
        for (int i = 0; i < buttonOpsi.Count; i++)
        {
            Image buttonImage = buttonOpsi[i].GetComponent<Image>();
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
            buttonOpsi[selectedButtonIndex].GetComponent<Button>().onClick.Invoke();
            isSelecting = false; // Setel kembali isSelecting ke false setelah memilih
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            isSelecting = false;
            panel.SetActive(false);
            Player1Controller playerController = FindObjectOfType<Player1Controller>();
            playerController.playerMovementEnabled = true;
        }

    }
}
