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
            GameObject init = Instantiate(building, new Vector3(transform.position.x * 0.28f, transform.position.y * 0.28f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
