using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviour
{

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;

    // Use this for initialization
    void Start()
    {
        for (int i = -worldHeight / 2; i < worldHeight / 2; i++)
        {
            for (int j = -worldWidth / 2; j < worldWidth / 2; j++)
            {
                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;
                cTrans.x = i * Tile.GetComponent<Renderer>().bounds.size.x;
                cTrans.y = j * Tile.GetComponent<Renderer>().bounds.size.y;
                cTile.transform.position = cTrans;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
