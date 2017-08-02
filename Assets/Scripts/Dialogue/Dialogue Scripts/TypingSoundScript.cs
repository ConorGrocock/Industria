using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypingSoundScript : DialogueScript
{
    public AudioSource typingSound;

	public override void OnStart()
    {
	}

    public override void OnCharacterTyped()
    {
        if (typingSound != null)
            typingSound.Play();
    }

    public override void OnFinish()
    {
    }
}
