using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{

    public Dialogue dialogue;
	private string interactText = "Talk";

	public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

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
			FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			FindObjectOfType<DialogueManagerPlayer2>().StartDialogue(dialogue);
		}
	}

}
