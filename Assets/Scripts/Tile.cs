using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tile;

    /// <summary>
    /// Building on this tile
    /// </summary>
    public GameObject building {
        get {
            return building;
        }

        set {
            GameObject init = Instantiate(value);
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -10);
        }
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
