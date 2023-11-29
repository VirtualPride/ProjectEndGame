using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManage : MonoBehaviour
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public Animator animator;
    [SerializeField]
    private float textSpeed;

    Message[] currentMessages;
    Message2[] nextMessages;
    Actor[] currentActors;
    int activeMessage;
    private bool isDone;
    public static bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive == true)
        {
            NextMessage();
        }
        // kondisi misi selesai
        if (Input.GetKeyDown(KeyCode.W))
		{
            isDone = true;
            Debug.Log("isDone = " + isDone);
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
        } else if(isDone == true && nextMessages.Length > 0)
        {
            DisplayMessage2();
		} else
		{
            DisplayMessage();
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
    }

    public void NextMessage()
	{
        activeMessage++;
        if (isDone == false && activeMessage < currentMessages.Length)
        {
            DisplayMessage();
        } else if (activeMessage < nextMessages.Length && isDone == true)
        {
            DisplayMessage2();
        } else if (activeMessage < currentMessages.Length && nextMessages.Length == 0 && isDone == true)
		{
            DisplayMessage();
        } else
		{
            Debug.Log("Conversation ended!");
            isActive = false;
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
