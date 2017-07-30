using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    int clicksRequired = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (GenWorld._instance.buildTile == null)
            {
                gameObject.SetActive(false);
                clicksRequired = 1;
                return;
            }
            if (clicksRequired != 0)
            {
                if (GenWorld._instance.buildTile != null) clicksRequired--;
                return;
            }

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                clicksRequired = 1;
                gameObject.SetActive(false);
                GenWorld._instance.buildTile = null;
            }
        }
    }
}
