using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _instance;

    private Queue<DialogueStruct> dStructQueue;

    public Text speakerNameText;
    public Image speakerImage;
    public Text dialogueText;

    public GameObject dialogueBox;

    [Header("Response Buttons")]
    public Button continueButton;
    public Button yesButton;
    public Button noButton;

    private Dialogue currentDialogue;
    private DialogueStruct currentDialogueStruct;
    private bool endOfSentence;

    private float secondsPerLetter;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogError("[DialogueManager] There are multiple dialogue managers in the scene!");
    }

    void Start()
    {
        dStructQueue = new Queue<DialogueStruct>();
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

        currentDialogue = dialogue;
        currentDialogue.isCurrentlyDisplayed = false;

        if (!Manager._instance.isPaused && currentDialogue.pauseGame)
        {
            Manager._instance.Pause();
        }

        speakerNameText.text = currentDialogue.speakerName;

        if (speakerImage != null)
            speakerImage.sprite = currentDialogue.speakerImage;
        else
            Debug.LogError("[DialogueManager] No speaker image assigned to dialogue! Name: " + dialogue.speakerName);

        dStructQueue.Clear();

        foreach (DialogueStruct dStruct in currentDialogue.sentences)
        {
            dStructQueue.Enqueue(dStruct);
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

        if (dStructQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (currentDialogueStruct != null)
        {
            if (currentDialogueStruct.scriptToRun != null && !endOfSentence)
                currentDialogueStruct.scriptToRun.OnFinish();

            if (currentDialogueStruct.typingSoundScript != null)
            {
                if (!endOfSentence)
                    currentDialogueStruct.typingSoundScript.OnFinish();
            }
            else
            {
                Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + currentDialogueStruct.sentence);
            }
        }

        currentDialogueStruct = dStructQueue.Dequeue();

        if (currentDialogueStruct == null)
        {
            Debug.LogError("[DialogueManager] You have not assigned a dialogue struct!");
            return;
        }

        secondsPerLetter = currentDialogueStruct.secondsPerLetter;

        string sentence = currentDialogueStruct.sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

        if (currentDialogueStruct.responseType == DialogueResponseType.CONTINUE)
        {
            if (continueButton != null)
                continueButton.gameObject.SetActive(true);
            else
                Debug.LogError("[DialogueManager] You have not assigned the continue button to the dialogue manager!");
        }
        else if (currentDialogueStruct.responseType == DialogueResponseType.YES_NO)
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
        if (currentDialogueStruct.scriptToRun != null)
            currentDialogueStruct.scriptToRun.OnStart();

        if (currentDialogueStruct.typingSoundScript != null)
        {
            currentDialogueStruct.typingSoundScript.OnStart();
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

            if (currentDialogueStruct.scriptToRun != null)
                currentDialogueStruct.scriptToRun.OnCharacterTyped();

            if (currentDialogueStruct.typingSoundScript != null)
            {
                currentDialogueStruct.typingSoundScript.OnCharacterTyped();
            }
            else
            {
                Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + sentence);
            }

            yield return new WaitForSeconds(secondsPerLetter);
        }

        if (currentDialogueStruct.scriptToRun != null)
            currentDialogueStruct.scriptToRun.OnFinish();

        if (currentDialogueStruct.typingSoundScript != null)
        {
            currentDialogueStruct.typingSoundScript.OnFinish();
        }
        else
        {
            Debug.LogError("[DialogueManager] Typing sound script is null for the current dialogue! Text: " + sentence);
        }

        endOfSentence = true;
    }

    public void OnYesResponse()
    {
        if (currentDialogueStruct.yesResponseTree != null)
            currentDialogueStruct.yesResponseTree.TriggerDialogue();
        else
            Debug.LogError("[DialogueManager] You have not assigned a yes response trigger to the current dialogue! Text: " + currentDialogueStruct.sentence);
    }

    public void OnNoResponse()
    {
        if (currentDialogueStruct.noResponseTree != null)
            currentDialogueStruct.noResponseTree.TriggerDialogue();
        else
            Debug.LogError("[DialogueManager] You have not assigned a no response trigger to the current dialogue! Text: " + currentDialogueStruct.sentence);
    }

    public void ChangeSpeakerImage(Sprite speakerImage)
    {
        currentDialogue.speakerImage = speakerImage;
        this.speakerImage.sprite = speakerImage;
    }

    private void EndDialogue()
    {
        currentDialogue.isCurrentlyDisplayed = false;
        dialogueBox.SetActive(false);
        Manager._instance.Unpause();
    }
}
