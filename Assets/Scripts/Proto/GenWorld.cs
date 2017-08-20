using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviourSingleton<GenWorld>
{
    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    [HideInInspector]
    public Dictionary<OreTypes, int> Resources;

    public Sprite[] sprites;
    public GameObject[][] tiles;

    public int expandCount = 1;

    public GameObject menu;

    void Start()
    {
        Resources = new Dictionary<OreTypes, int>();

        Resources.Add(OreTypes.Coal, 0);
        Resources.Add(OreTypes.Copper, 10);
        Resources.Add(OreTypes.Wood, 100);

        Camera.main.transform.Translate(new Vector3((worldWidth / 2) * 1.28f, ((worldHeight / 2) * 1.28f) - 0.5f));

        GenerateWorld();

        if (!Manager._instance.isMainMenu)
            BuildingManager._instance.updateInformationBar();
    }

    void GenerateWorld()
    {
        tiles = new GameObject[worldWidth][];

        for (int x = 0; x < worldWidth; x++)
        {
            tiles[x] = new GameObject[worldHeight];
            for (int y = 0; y < worldHeight; y++)
            {
                Sprite cSprite = sprites[Random.Range(0, sprites.Length - 1)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;

                cTrans.x = x * 1.28f;
                cTrans.y = y * 1.28f;
                cTrans.z = 100;

                cTile.transform.position = cTrans;
                cTile.transform.parent = Parent.transform;

                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;

                cTile.GetComponent<Tile>().tile = Tile;
                tiles[x][y] = cTile;
            }
        }

        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().building = BuildingManager._instance.buildings["House"];
        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().topBuilding.powerDraw = 1f;
        tiles[(worldWidth / 2) + Random.Range(1, 3)][(worldHeight / 2) + Random.Range(1, 3)].GetComponent<Tile>().building = BuildingManager._instance.buildings["PowerPlant"];

        int copper = 0, wood = 0, coal = 0;

        for (int ores = 0; ores < Random.Range(6, 10); ores++)
        {
            GameObject cTile = null;

            while (cTile == null || cTile.GetComponent<Tile>().building != null || cTile.GetComponent<Tile>().ore != null)
            {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            if (coal == 0)
            {
                Tile tile = cTile.GetComponent<Tile>();
                tile.ore = new Ore(OreTypes.Coal, 1000);
                tile.building = BuildingManager._instance.buildings["Mine"];
                coal++;
            }
            else if (copper == 0)
            {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                copper++;
            }
            //else if (iron == 0)
            //{
            //    cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
            //    iron++;
            //}
            else if (wood == 0)
            {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                wood++;
            }
            else
            {
                switch (Random.Range(0, 4))
                {
                    case (0):
                        {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                            coal++;
                            break;
                        }
                    case (1):
                    case 2:
                        {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                            copper++;
                            break;
                        }
                    //case (2):
                    //    {
                    //        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
                    //        iron++;
                    //        break;
                    //    }
                    case (3):
                        {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                            wood++;
                            break;
                        }
                }
            }
        }
    }

    public void closeMenu()
    {
        Destroy(menu);
        menu = null;
    }

    public void expandMap(int amount)
    {
        GameObject[][] oldTiles = tiles;
        worldWidth += amount;
        worldHeight += amount;
        tiles = new GameObject[worldWidth][];

        for (int x = 0; x < worldWidth; x++)
        {
            tiles[x] = new GameObject[worldHeight];

            for (int y = 0; y < worldHeight; y++)
            {
                if (oldTiles.Length - 1 >= x && oldTiles[x] != null)
                {
                    if (oldTiles[x].Length - 1 >= y && oldTiles[x][y] != null)
                    {
                        tiles[x][y] = oldTiles[x][y];
                        continue;
                    }
                }

                Sprite cSprite = sprites[Random.Range(0, sprites.Length - 1)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;

                cTrans.x = x * 1.28f;
                cTrans.y = y * 1.28f;
                cTrans.z = 100;

                cTile.transform.position = cTrans;
                cTile.transform.parent = Parent.transform;

                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;

                cTile.GetComponent<Tile>().tile = Tile;
                tiles[x][y] = cTile;
            }
        }

        generateOres(6,20);

        InputManager._instance.maximumZoomSize += Mathf.Pow(2, expandCount);
    }

    void generateOres(int minimum, int maximum) {
        for (int ores = 0; ores < Random.Range(minimum, maximum); ores++) {
            GameObject cTile = null;
            while (cTile == null || cTile.GetComponent<Tile>().building != null || cTile.GetComponent<Tile>().ore != null) {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            cTile.GetComponent<Tile>().generateOre();
        }
    }
}
