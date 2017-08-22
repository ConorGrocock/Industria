using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GenWorld : MonoBehaviourSingleton<GenWorld>
{
    /// <summary>
    /// Height of the world
    /// </summary>
    public int worldHeight = 10;
    /// <summary>
    /// Width of the world
    /// </summary>
    public int worldWidth = 10;
    /// <summary>
    /// Tile prefab
    /// </summary>
    public GameObject Tile;
    /// <summary>
    /// Gameobject to attach the tiles to
    /// </summary>
    public GameObject Parent;

    /// <summary>
    /// Resource array
    /// </summary>
    public Dictionary<OreTypes, int> Resources;

    /// <summary>
    /// Ground textures
    /// </summary>
    public Sprite[] sprites;

    /// <summary>
    /// World array
    /// </summary>
    public GameObject[][] tiles;

    /// <summary>
    /// Number of times the world has been expanded
    /// </summary>
    public int expandCount = 1;

    /// <summary>
    /// Instance of the menu
    /// </summary>
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


    /// <summary>
    /// Generate a brand new world
    /// </summary>
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

        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().buildingType = BuildingManager._instance.buildings["House"];
        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().topBuilding.powerDraw = 1f;
        tiles[(worldWidth / 2) + Random.Range(1, 3)][(worldHeight / 2) + Random.Range(1, 3)].GetComponent<Tile>().buildingType = BuildingManager._instance.buildings["PowerPlant"];
        
        generateOres(10, 20);
    }

    /// <summary>
    /// Close any open menus
    /// </summary>
    public void closeMenu()
    {
        Destroy(menu);
        menu = null;
    }

    /// <summary>
    /// Expand the current map
    /// </summary>
    /// <param name="amount">Number of tiles to expand the map</param>
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

    /// <summary>
    /// Generate the ores for the map
    /// </summary>
    /// <param name="minimum">Minimum number of ores</param>
    /// <param name="maximum">Maximum number of ores</param>
    void generateOres(int minimum, int maximum) {
        for (int ores = 0; ores < Random.Range(minimum, maximum); ores++) {
            GameObject cTile = null;
            while (cTile == null || cTile.GetComponent<Tile>().buildingType != null || cTile.GetComponent<Tile>().ore != null) {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            cTile.GetComponent<Tile>().generateOre();
        }
    }
}
