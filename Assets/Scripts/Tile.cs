using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /// <summary>
    /// Building on this tile
    /// </summary>
    public GameObject building;

    // Use this for initialization
    void Start()
    {

    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
            GameObject init = Instantiate(building);
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0);
            Debug.Log("==========");
            Debug.Log(mouse);
            Debug.Log(transform.position);
            Debug.Log(init.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
