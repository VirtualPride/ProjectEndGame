using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManage2 : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public Animator animator;
    [SerializeField]
    private float textSpeed;

    [SerializeField]
    private float timeLimit;
    private float currentTimeLimit;

    Message[] currentMessages;
    Message2[] nextMessages;
    Actor[] currentActors;
    int activeMessage;
    private bool isDisplayMessage = false;
    private bool isDone;
    public static bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        currentTimeLimit = timeLimit;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && isActive == true)
        {
            NextMessage();
        }
        // kondisi misi selesai
        if (Input.GetKeyDown(KeyCode.Tab))
		{
            isDone = true;
            Debug.Log("isDone = " + isDone);
		}

        // Auto next Dialogue
        if (isActive == true && isDisplayMessage == true)
        {
            if (currentTimeLimit > 0)
            {
                currentTimeLimit -= Time.deltaTime;
                Debug.Log(currentTimeLimit);
            } else
            {
                NextMessage();
            }
        }
    }

    public void OpenDialogue(Message[] messages, Message2[] messages2, Actor[] actors)
	{
        Debug.Log(isDone);
        animator.SetBool("IsOpen", true);
        currentMessages = messages;
        nextMessages = messages2;
        currentActors = actors;
        isActive = true;
        activeMessage = 0;
        // kondisi misi selesai (opsional codingan bisa ganti setiap saat)
        if (isDone == false)
		{
            DisplayMessage();
            isDisplayMessage = true;
        } else if(isDone == true && nextMessages.Length > 0)
        {
            DisplayMessage2();
            isDisplayMessage = true;
		} else
		{
            DisplayMessage();
			isDisplayMessage = true;
		}
        Debug.Log("Started conversation! Loaded messages: " + messages.Length);
	}

    void DisplayMessage()
	{
        Message messageToDisplay = currentMessages[activeMessage];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(messageToDisplay.message + "..."));
        //messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        Debug.Log(activeMessage);
		isDisplayMessage = false;
        currentTimeLimit = timeLimit;
	}

    void DisplayMessage2()
    {
        Message2 message2ToDisplay = nextMessages[activeMessage];
        StopAllCoroutines();
        StartCoroutine(TypeSentence(message2ToDisplay.message));
        //messageText.text = messageToDisplay.message;

        Actor actorToDisplay = currentActors[message2ToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.sprite;
        Debug.Log(activeMessage);
		isDisplayMessage = false;
		currentTimeLimit = timeLimit;
	}

    public void NextMessage()
	{
        activeMessage++;
        if (isDone == false && activeMessage < currentMessages.Length)
        {
            DisplayMessage();
			isDisplayMessage = true;
		} else if (activeMessage < nextMessages.Length && isDone == true)
        {
            DisplayMessage2();
			isDisplayMessage = true;
		} else if (activeMessage < currentMessages.Length && nextMessages.Length == 0 && isDone == true)
		{
            DisplayMessage();
			isDisplayMessage = true;
		} else
		{
            Debug.Log("Conversation ended!");
            isActive = false;
            isDisplayMessage = false;
            animator.SetBool("IsOpen", false);
		}
    }

    IEnumerator TypeSentence (string message)
    {
        messageText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSeconds(textSpeed); 
        }
    }
}
