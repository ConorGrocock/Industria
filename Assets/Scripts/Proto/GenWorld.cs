﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviour
{
    public static GenWorld _instance;

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    public Dictionary<string, BuildingType> buildings;


    public Sprite[] sprites;

    public GameObject[][] tiles;

    private int iron, copper, wood, coal;

    // Use this for initialization
    void Start()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError("YOU HAVE FUCKED UP. You have more than one World gen class" );

        buildings = new Dictionary<string, BuildingType>();
        registerBuildings();

        Camera.main.transform.Translate(new Vector3((worldWidth / 2) * 1.28f, ((worldHeight / 2) * 1.28f) - 0.5f));

        tiles = new GameObject[worldWidth][];
        for (int x = 0; x < worldWidth; x++) {
            tiles[x] = new GameObject[worldHeight];
            for (int y = 0; y < worldHeight; y++) {
                Sprite cSprite = sprites[Random.Range(0, sprites.Length-1)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;
                
                cTrans.x = x * 1.28f;
                cTrans.y = y * 1.28f;
                cTrans.z = 100;

                cTile.transform.position = cTrans;
                cTile.transform.parent = Parent.transform;

                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;


                if(Random.Range(0, 20) == 0) {
                    if(cTile.GetComponent<Tile>().building != null) {
                        return;
                    }
                    if (coal == 0) {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                        coal++;
                    }
                    else if (copper == 0) {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                        copper++;
                    }
                    else if (iron == 0) {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
                        iron++;
                    }
                    else if (wood == 0) {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                        wood++;
                    }
                    else {
                        switch (Random.Range(0, 4)) {
                            case (0): {
                                    cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                                    coal++;
                                    break;
                                }
                            case (1): {
                                    cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                                    copper++;
                                    break;
                                }
                            case (2): {
                                    cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
                                    iron++;
                                    break;
                                }
                            case (3): {
                                    cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                                    wood++;
                                    break;
                                }
                        }
                    }
                }
                
                cTile.GetComponent<Tile>().tile = Tile;
                tiles[x][y] = cTile;
            }
        }
        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().building = buildings["House"];
    }

    void registerBuildings() {
        new House().register();
    }

    public Vector3 getTileCoord(Vector3 vector) {
        return vector / 1.28f;
    }
}
