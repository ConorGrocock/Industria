using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : DialogueScript
{
    public GameObject arrowContainer;

    [Range(0, 100)]
    public float arrowX;
    [Range(0, 100)]
    public float arrowY;

    [Range(0.0f, 360.0f)]
    public float arrowContainerRotation;

    public override void OnStart()
    {
        if (arrowContainer != null)
        {
            arrowX = (arrowX / 100) * (Screen.width);
            arrowY = (arrowY / 100) * (Screen.height);
            Vector3 arrowPos = new Vector3(arrowX, arrowY);

            arrowContainer.SetActive(true);
            arrowContainer.transform.position = arrowPos;
            arrowContainer.transform.rotation = Quaternion.Euler(0, 0, arrowContainerRotation);
        }
        else
        {
            Debug.LogError("[ArrowScript] You haven't added the arrow container!");
        }
    }

    public override void OnUpdate()
    {
    }

    public override void OnCharacterTyped()
    {
    }

    public override void OnFinish()
    {
    }

    public override void OnEndOfStruct()
    {
        if (arrowContainer != null)
        {
            arrowContainer.SetActive(false);
        }
        else
        {
            Debug.LogError("[ArrowScript] You haven't added the arrow container!");
        }
    }
}
