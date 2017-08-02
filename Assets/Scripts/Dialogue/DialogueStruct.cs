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
    public DialogueScript scriptToRun;

    public float secondsPerLetter = 0.05f;

    public DialogueResponseType responseType;

    public DialogueTrigger yesResponseTree;
    public DialogueTrigger noResponseTree;

    [HideInInspector]
    public DialogueScript typingSoundScript;

    [TextArea(3, 10)]
    public string sentence;

    void Awake()
    {
        typingSoundScript = GetComponent<TypingSoundScript>();

        if (typingSoundScript == null)
        {
            Debug.LogError("[DialogueStruct] Could not find typing sound script!");
        }
    }
}
