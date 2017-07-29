﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviour
{

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    public GameObject Building;

    public Sprite[] sprites;

    // Use this for initialization
    void Start()
    {
        for (float y = (-(float)worldHeight / 2f); y < ((float)worldHeight / 2f) + 1; y++)
        {
            for (float x = (-(float)worldWidth / 2f); x < (float)worldWidth / 2f; x++)
            {
                Sprite cSprite = sprites[sprites.Length-1];//sprites[Random.Range(0, sprites.Length)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;

                cTrans.x = x * 1.28f;
                cTrans.y = y * 1.28f;

                cTile.transform.position = cTrans;
                cTile.transform.parent = Parent.transform;


                GameObject init = Instantiate(Building);
                init.transform.parent = Parent.transform;
                init.transform.position = cTrans;



                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;
                cTile.GetComponent<Tile>().building = Building;
            }
        }
    }
}
