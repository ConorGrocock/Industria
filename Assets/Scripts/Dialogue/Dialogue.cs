using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string speakerName;
    public Sprite speakerImage;
    public bool pauseGame = true;

    [HideInInspector]
    public bool isCurrentlyDisplayed = false;

    public DialogueStruct[] sentences;

    void Awake()
    {
        int i = 0;

        foreach (DialogueStruct dStruct in sentences)
        {
            if (dStruct != null)
                dStruct.dialogueAttachedTo = this;
            else
                Debug.LogError("[Dialogue] Element " + i + " is null!");

            i++;
        }
    }

    public void ChangeSpeakerImage(Sprite speakerImage)
    {
        this.speakerImage = speakerImage;

        if (isCurrentlyDisplayed)
        {
            DialogueManager._instance.ChangeSpeakerImage(speakerImage);
        }
    }

    public void SetCurrentDisplayed(bool value)
    {
        isCurrentlyDisplayed = value;

        int i = 0;

        foreach (DialogueStruct dStruct in sentences)
        {
            if (dStruct != null)
                dStruct.OnDialogueChange(this);
            else
                Debug.LogError("[Dialogue] Element " + i + " is null!");

            i++;
        }
    }
}
