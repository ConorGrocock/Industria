using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueScript : MonoBehaviour
{
    [HideInInspector]
    public DialogueStruct structAttachedTo;

    public abstract void OnStart();
    public abstract void OnCharacterTyped();
    public abstract void OnFinish();
}
