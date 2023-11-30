using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteract : MonoBehaviour, IInteractable
{
    public Message[] messages;
    public Message2[] messages2;
    public Actor[] actors;

	private string interactText = "Talk";

	public string GetInteractText()
	{
		return interactText;
	}

	public Transform GetTransform()
	{
		throw new System.NotImplementedException();
	}

	public void Interact()
	{
        if (Input.GetKeyDown(KeyCode.C))
        {
			FindObjectOfType<DialogueManage>().OpenDialogue(messages, messages2, actors);
		} else if (Input.GetKeyDown(KeyCode.M))
		{
			FindObjectOfType<DialogueManage2>().OpenDialogue(messages, messages2, actors);
		}
	}

	public void StartDialogue()
	{
        FindObjectOfType<DialogueManage>().OpenDialogue(messages, messages2, actors);
	}
}

[System.Serializable]
public class Message
{
    public int actorId;
    [TextArea(3, 10)]
    public string message;
}

[System.Serializable]
public class Message2
{
    public int actorId;
    [TextArea(3, 10)]
    public string message;
}

[System.Serializable]
public class Actor
{
    public string name;
    public Sprite sprite;
}
