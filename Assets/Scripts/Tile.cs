using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tile;

    /// <summary>
    /// Building on this tile
    /// </summary>
    public BuildingType Building;
    public BuildingType building {
        get {
            return Building;
        }

        set {
            GameObject init = Instantiate(tile);
            init.GetComponent<SpriteRenderer>().sprite = value.sprite;
            init.AddComponent(value.script.GetType());
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -10);
            Building = value;
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
    }
}
