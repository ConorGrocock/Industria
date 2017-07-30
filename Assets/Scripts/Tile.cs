using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /// <summary>
    /// Building on this tile
    /// </summary>
    public BuildingType building;
    public GameObject tile;
    public bool hover = false;

    // Use this for initialization
    void Start()
    {

    }
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject init = Instantiate(tile);
            init.GetComponent<SpriteRenderer>().sprite = building.sprite;
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -10);
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0.5f, 0.7f);
    }

    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
