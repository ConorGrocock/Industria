using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueResponseType
{
    CONTINUE,
    YES_NO
}

public class DialogueStruct : MonoBehaviour
{
    public DialogueResponseType responseType;
    public DialogueTrigger yesResponseTree;
    public DialogueTrigger noResponseTree;

    [TextArea(3, 10)]
    public string sentence;
}
