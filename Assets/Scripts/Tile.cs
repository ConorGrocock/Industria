using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /// <summary>
    /// Building on this tile
    /// </summary>
    public Sprite building;
    public GameObject tile;

    // Use this for initialization
    void Start()
    {

    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject init = Instantiate(tile);
            init.GetComponent<SpriteRenderer>().sprite = building;
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -100);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
