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
            GameObject init = Instantiate(building);
            Vector3 pos = init.transform.position;
            pos.x = transform.position.x;
            pos.y = transform.position.y;
            init.transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
