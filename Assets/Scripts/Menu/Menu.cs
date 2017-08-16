using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{
    public GameObject[] objects = new GameObject[5];

    // Use this for initialization
    void Start()
    {
    }

    int clicksRequired = 1;

    void OnEnable()
    {
        //foreach (GameObject obj in objects)
        //{
        //    if (obj == null) continue;
        //    GameObject newObject = Instantiate(obj) as GameObject;
        //    newObject.transform.SetParent(GameObject.Find("Scroll View").transform, false);
        //}
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    void OnDisable()
    {
        clicksRequired = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BuildingManager._instance.buildTile == null && clicksRequired == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            if (clicksRequired != 0)
            {
                if (BuildingManager._instance.buildTile != null) clicksRequired--;
                return;
            }

            if (!IsPointerOverUIObject())
            {
                gameObject.SetActive(false);
                BuildingManager._instance.buildTile = null;
            }
        }
    }
}
