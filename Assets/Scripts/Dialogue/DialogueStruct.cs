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

    public DialogueScript[] scriptsToRun;

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
        if (scriptsToRun != null)
        {
            for (int i = 0; i < scriptsToRun.Length; i++)
            {
                if (scriptsToRun[i] != null)
                    scriptsToRun[i].structAttachedTo = this;
                else
                    Debug.LogError("[DialogueStruct] [Scripts] Element " + i + " is null!");
            }
        }

        typingSoundScript = GetComponent<TypingSoundScript>();

        if (typingSoundScript != null)
        {
            typingSoundScript.structAttachedTo = this;
        }
        else
        {
            Debug.LogError("[DialogueStruct] Could not find typing sound script! On: " + transform.name);
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
            Debug.LogError("[DialogueStruct] [ChangeSpeakerImage] Dialogue struct is not attached to any dialogue! Are you calling this from Awake? Text: " + sentence + " On: " + transform.name);
        }
    }

    public void OnDialogueChange(Dialogue dialogue)
    {
        dialogueAttachedTo = dialogue;

        if (scriptsToRun != null)
        {
            for (int i = 0; i < scriptsToRun.Length; i++)
            {
                if (scriptsToRun[i] != null)
                    scriptsToRun[i].structAttachedTo = this;
                else
                    Debug.LogError("[DialogueStruct] [Scripts] Element " + i + " is null!");
            }
        }

        if (typingSoundScript != null)
        {
            typingSoundScript.structAttachedTo = this;
        }
        else
        {
            Debug.LogError("[DialogueStruct] Could not find typing sound script! On: " + transform.name);
        }
    }
}
