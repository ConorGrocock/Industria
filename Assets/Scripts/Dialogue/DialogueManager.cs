using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> dialogueQueue;

    public Text speakerNameText;
    public Image speakerImage;
    public Text dialogueText;

    public float secondsPerLetter;

    public GameObject dialogueBox;

	void Start()
    {
        dialogueQueue = new Queue<string>();
	}

    public void StartDialogue(Dialogue dialogue)
    {
        dialogueBox.SetActive(true);

        speakerNameText.text = dialogue.speakerName;
        speakerImage.sprite = dialogue.speakerImage;

        dialogueQueue.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(secondsPerLetter);
        }
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Manager._instance.Unpause();
    }
}
