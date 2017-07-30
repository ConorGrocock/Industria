using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    /// <summary>
    /// Building on this tile
    /// </summary>
    BuildingType Building;
    public BuildingType building
    {
        get
        {
            return Building;
        }

        set
        {
            GameObject init = Instantiate(tile);
            init.GetComponent<SpriteRenderer>().sprite = value.sprite;
            init.AddComponent(value.script.GetType());
            Destroy(init.GetComponent<BoxCollider>());
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -10);
            Building = value;
        }
    }

    public GameObject tile;
    public bool hover = false;

    // Use this for initialization
    void Start()
    {

    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            building = GenWorld._instance.buildings["House"];
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0.5f, 0.7f);
    }

    public void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
