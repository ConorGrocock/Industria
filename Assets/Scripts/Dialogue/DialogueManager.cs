using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueStruct> dialogueQueue;

    public Text speakerNameText;
    public Image speakerImage;
    public Text dialogueText;

    public GameObject dialogueBox;

    [Header("Response Buttons")]
    public Button continueButton;
    public Button yesButton;
    public Button noButton;

    private DialogueStruct currentDialogue;
    private bool endOfSentence;

    private float secondsPerLetter;

    void Start()
    {
        dialogueQueue = new Queue<DialogueStruct>();
	}

    public void StartDialogue(Dialogue dialogue)
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(true);
        }
        else
        {
            Debug.LogError("[DialogueManager] You have not assigned the dialogue box to the dialogue manager!");
            Manager._instance.Unpause();
            return;
        }

        if (dialogue == null)
        {
            Debug.LogError("[DialogueManager] The dialogue to be displayed is null!");
            Manager._instance.Unpause();
            return;
        }

        speakerNameText.text = dialogue.speakerName;

        if (speakerImage != null)
            speakerImage.sprite = dialogue.speakerImage;
        else
            Debug.LogError("[DialogueManager] No speaker image assigned to dialogue! Name: " + dialogue.speakerName);

        dialogueQueue.Clear();

        foreach (DialogueStruct sentence in dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence); // TODO: because yes (needs refactoring)
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);
        else
            Debug.LogError("[DialogueManager] You have not assigned the continue button to the dialogue manager!");

        if (yesButton != null)
            yesButton.gameObject.SetActive(false);
        else
            Debug.LogError("[DialogueManager] You have not assigned the yes button to the dialogue manager!");

        if (noButton != null)
            noButton.gameObject.SetActive(false);
        else
            Debug.LogError("[DialogueManager] You have not assigned the no button to the dialogue manager!");

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (currentDialogue != null)
        {
            if (currentDialogue.scriptToRun != null && !endOfSentence)
                currentDialogue.scriptToRun.OnFinish();

            if (currentDialogue.typingSoundScript != null)
            {
                if (!endOfSentence)
                    currentDialogue.typingSoundScript.OnFinish();
            }
            else
            {
                Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + currentDialogue.sentence);
            }
        }

        currentDialogue = dialogueQueue.Dequeue();

        if (currentDialogue == null)
        {
            Debug.LogError("[DialogueManager] You have not assigned a dialogue struct!");
            return;
        }

        secondsPerLetter = currentDialogue.secondsPerLetter;

        string sentence = currentDialogue.sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (currentDialogue.responseType == DialogueResponseType.CONTINUE)
        {
            if (continueButton != null)
                continueButton.gameObject.SetActive(true);
            else
                Debug.LogError("[DialogueManager] You have not assigned the continue button to the dialogue manager!");
        }
        else if (currentDialogue.responseType == DialogueResponseType.YES_NO)
        {
            if (yesButton != null)
                yesButton.gameObject.SetActive(true);
            else
                Debug.LogError("[DialogueManager] You have not assigned the yes button to the dialogue manager!");

            if (noButton != null)
                noButton.gameObject.SetActive(true);
            else
                Debug.LogError("[DialogueManager] You have not assigned the no button to the dialogue manager!");
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (currentDialogue.scriptToRun != null)
            currentDialogue.scriptToRun.OnStart();

        if (currentDialogue.typingSoundScript != null)
        {
            currentDialogue.typingSoundScript.OnStart();
        }
        else
        {
            Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + sentence);
        }

        endOfSentence = false;
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (currentDialogue.scriptToRun != null)
                currentDialogue.scriptToRun.OnCharacterTyped();

            if (currentDialogue.typingSoundScript != null)
            {
                currentDialogue.typingSoundScript.OnCharacterTyped();
            }
            else
            {
                Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + sentence);
            }

            yield return new WaitForSeconds(secondsPerLetter);
        }

        if (currentDialogue.scriptToRun != null)
            currentDialogue.scriptToRun.OnFinish();

        if (currentDialogue.typingSoundScript != null)
        {
            currentDialogue.typingSoundScript.OnFinish();
        }
        else
        {
            Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + sentence);
        }

        endOfSentence = true;
    }

    public void OnYesResponse()
    {
        if (currentDialogue.yesResponseTree != null)
            currentDialogue.yesResponseTree.TriggerDialogue();
        else
            Debug.LogError("[DialogueManager] You have not assigned a yes response trigger to the current dialogue! Text: " + currentDialogue.sentence);
    }

    public void OnNoResponse()
    {
        if (currentDialogue.noResponseTree != null)
            currentDialogue.noResponseTree.TriggerDialogue();
        else
            Debug.LogError("[DialogueManager] You have not assigned a no response trigger to the current dialogue! Text: " + currentDialogue.sentence);
    }

    private void EndDialogue()
    {
        dialogueBox.SetActive(false);
        Manager._instance.Unpause();
    }
}
