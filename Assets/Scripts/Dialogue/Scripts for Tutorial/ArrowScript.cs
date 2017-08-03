using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : DialogueScript
{
    public GameObject arrowContainer;
    public Vector2 arrowContainerPosition;

    [Range(0.0f, 360.0f)]
    public float arrowContainerRotation;

    public override void OnStart()
    {
        if (arrowContainer != null)
        {
            arrowContainer.SetActive(true);
            arrowContainer.transform.position = new Vector3(arrowContainerPosition.x, arrowContainerPosition.y, 0);
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
