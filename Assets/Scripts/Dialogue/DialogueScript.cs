using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class DialogueScript : MonoBehaviour
{
    [HideInInspector]
    public DialogueStruct structAttachedTo;

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnCharacterTyped();
    public abstract void OnFinish();
    public abstract void OnEndOfStruct();

    void Update()
    {
        if (structAttachedTo.dialogueAttachedTo.isCurrentlyDisplayed)
            OnUpdate();
    }
}
