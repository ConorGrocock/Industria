using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueResponseType
{
    CONTINUE,
    YES_NO
}

[RequireComponent(typeof(TypingSoundScript))]
public class DialogueStruct : MonoBehaviour
{
    [HideInInspector]
    public Dialogue dialogueAttachedTo;

    public DialogueScript scriptToRun;

    public float secondsPerCharacter = 0.05f;

    public DialogueResponseType responseType;

    public DialogueTrigger yesResponseTree;
    public DialogueTrigger noResponseTree;

    [HideInInspector]
    public DialogueScript typingSoundScript;

    [TextArea(3, 10)]
    public string sentence;

    void Awake()
    {
        if (scriptToRun != null)
            scriptToRun.structAttachedTo = this;

        typingSoundScript = GetComponent<TypingSoundScript>();

        if (typingSoundScript != null)
        {
            typingSoundScript.structAttachedTo = this;
        }
        else
        {
            Debug.LogError("[DialogueStruct] Could not find typing sound script!");
        }
    }

    public void ChangeSpeakerImage(Sprite speakerImage)
    {
        if (dialogueAttachedTo != null)
        {
            dialogueAttachedTo.ChangeSpeakerImage(speakerImage);
        }
        else
        {
            Debug.LogError("[DialogueStruct] [ChangeSpeakerImage] Dialogue struct is not attached to any dialogue! Are you calling this from Awake? Text: " + sentence);
        }
    }

    public void OnDialogueChange(Dialogue dialogue)
    {
        dialogueAttachedTo = dialogue;

        if (scriptToRun != null)
            scriptToRun.structAttachedTo = this;

        if (typingSoundScript != null)
        {
            typingSoundScript.structAttachedTo = this;
        }
        else
        {
            Debug.LogError("[DialogueStruct] Could not find typing sound script!");
        }
    }
}
