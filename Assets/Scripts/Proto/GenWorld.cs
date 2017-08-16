using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ResourceArray
{
    public OreTypes type;
    public int amount;
}

public class GenWorld : MonoBehaviourSingleton<GenWorld>
{
    public GameObject gameManager;

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    public float powerSupply;

    public float PowerStored = 100.0f;

    public float powerDraw
    {
        get
        {
            float p = 0f;
            foreach (Building building in Building.buildings)
            {
                p += building.powerDraw;
            }
            return p;
        }
    }

    [HideInInspector]
    public Dictionary<string, BuildingType> buildings;
    [HideInInspector]
    public Dictionary<OreTypes, int> Resources;

    public bool applyInspectorValsForResources = false;
    public ResourceArray[] resourcesInspector;

    [HideInInspector]
    public List<House> houses = new List<House>();
    [HideInInspector]
    public List<PowerPlant> plants = new List<PowerPlant>();
    public GameObject buildingPanel;
    Tile BuildTile;
    public Tile buildTile
    {
        get
        {
            return BuildTile;
        }
        set
        {
            BuildTile = value;
            if (value == null) { buildingPanel.SetActive(false); return; }
            buildingPanel.SetActive(true);
            Button[] buttons = buildingPanel.GetComponentsInChildren<Button>();

            foreach (Button b in buttons)
            {
                if (value.ore == null)
                {
                    if (b.name == "Lab") b.interactable = false;
                    else if (b.name == "Power plant" && buildings["PowerPlant"].buildable) b.interactable = true;
                    else b.interactable = false;
                    continue;
                }
                if (b.name == "Lumber Mill" && value.ore.mine == MineType.Mill && buildings["Mill"].buildable) b.interactable = true;
                else if (b.name == "Mine" && value.ore.mine == MineType.Shaft && buildings["Mine"].buildable) b.interactable = true;
                else b.interactable = false;
                if (b.name == "Lab") b.interactable = false;
            }
        }
    }

    public Tile hoverTile;

    public Sprite[] sprites;

    public GameObject[][] tiles;

    // Use this for initialization
    void Awake()
    {
        if (!Application.isEditor) applyInspectorValsForResources = false;

        if (buildingPanel == null) buildingPanel = GameObject.Find("BuildingPanel");

        if (!Manager._instance.isMainMenu)
            buildingPanel.SetActive(false);

        buildings = new Dictionary<string, BuildingType>();
        registerBuildings();

        Resources = new Dictionary<OreTypes, int>();

        resourcesInspector = new ResourceArray[3];

        for (int i = 0; i < resourcesInspector.Length; i++)
        {
            resourcesInspector[i] = new ResourceArray();
        }

        resourcesInspector[0].type = OreTypes.Coal;
        resourcesInspector[0].amount = 0;

        resourcesInspector[1].type = OreTypes.Copper;
        resourcesInspector[1].amount = 10;

        resourcesInspector[2].type = OreTypes.Wood;
        resourcesInspector[2].amount = 100;

        for (int i = 0; i < resourcesInspector.Length; i++)
        {
            Resources.Add(resourcesInspector[i].type, resourcesInspector[i].amount);
        }

        Camera.main.transform.Translate(new Vector3((worldWidth / 2) * 1.28f, ((worldHeight / 2) * 1.28f) - 0.5f));

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

        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().building = buildings["House"];
        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().topBuilding.powerDraw = 1f;
        tiles[(worldWidth / 2) + Random.Range(1, 3)][(worldHeight / 2) + Random.Range(1, 3)].GetComponent<Tile>().building = buildings["PowerPlant"];

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
                tile.building = buildings["Mine"];
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
        if (!Manager._instance.isMainMenu)
            updateInfomationBar();
    }

    void updateInfomationBar()
    {
        int count = 0;

        foreach (House house in houses)
        {
            count += house.occupancy;
        }

        GameObject.Find("PeopleCount").GetComponent<Text>().text = "Normal: " + count;

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }

        GameObject.Find("ResourceCount").GetComponent<Text>().text = resource;
        GameObject.Find("UIBarPower").GetComponent<Text>().text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(PowerStored), Mathf.Round(powerSupply), Mathf.Round(powerDraw));
    }

    

    void Update()
    {
        if (Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        if (applyInspectorValsForResources)
        {
            Resources.Clear();

            for (int i = 0; i < resourcesInspector.Length; i++)
            {
                Resources.Add(resourcesInspector[i].type, resourcesInspector[i].amount);
            }
        }

        if (PowerStored < 0)
        {
            PowerStored = 0;
            Time.timeScale = 0;
            Manager._instance.ShowGameOver();
        }
    }

    public int expandCount = 1;

    public static GameObject menu;

    public void closeMenu()
    {
        Destroy(menu);
        menu = null;
    }

    void registerBuildings()
    {
        // TODO: Get rid of "new" keyword
        new House().register();
        new Mine().register();
        new Mill().register();
        //new Lab().register();
        new PowerPlant().register();
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

        for (int ores = 0; ores < Random.Range(6, 20); ores++)
        {
            GameObject cTile = null;

            while (cTile == null || cTile.GetComponent<Tile>().building != null || cTile.GetComponent<Tile>().ore != null)
            {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            switch (Random.Range(0, 7))
            {
                case (0):
                case (1):
                case (2):
                case (4): {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                        break;
                    }
                case (5):
                    {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                        break;
                    }
                case (6):
                    {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                        break;
                    }
            }
        }

        InputManager._instance.maximumZoomSize += Mathf.Pow(2, expandCount);
    }

    public void buildOnTile(string building)
    {
        foreach (KeyValuePair<OreTypes, int> cost in buildings[building].costs)
        {
            GenWorld._instance.Resources[cost.Key] -= cost.Value;
        }
        if (BuildTile != null) buildTile.building = buildings[building];
        else if (hoverTile != null) hoverTile.building = buildings[building];
        buildingPanel.SetActive(false);
        BuildTile = null;
    }

    
}
