using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueScript : DialogueScript
{
	public override void OnStart()
    {
        Debug.Log("The dialogue has started!");
	}

    public override void OnUpdate()
    {
    }

    public override void OnCharacterTyped()
    {
        Debug.Log("A character was typed!");
    }

    public override void OnFinish()
    {
        Debug.Log("The dialogue is finished!");
	}

    public override void OnEndOfStruct()
    {
        Debug.Log("End of struct!");
    }
}
