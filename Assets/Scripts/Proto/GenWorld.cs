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

public class GenWorld : MonoBehaviour
{
    public static GenWorld _instance;
    public float minOrthoSize = 1.0f;
    public float maxOrthoSize = 10.0f;
    public float zoomSpeed = 0.5f;
    public GameObject gameManager;
    private Manager managerScript;

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public bool isMainMenu;
    [HideInInspector]
    public bool isPaused;

    float PowerSupply;
    public float powerSupply
    {
        get { return PowerSupply; }
        set { PowerSupply = value; }
    }

    public float PowerStored = 100f;

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
        if (_instance == null) _instance = this;
        else Debug.LogError("YOU HAVE FUCKED UP. You have more than one World gen class");

        isMainMenu = SceneManager.GetActiveScene().name == "_Menu";

        if (managerScript == null && !isMainMenu)
        {
            managerScript = gameManager.GetComponent<Manager>();
        }

        if (buildingPanel == null) buildingPanel = GameObject.Find("BuildingPanel");

        if (!isMainMenu)
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
        if (!isMainMenu)
            updateInfomationBar();
    }

    void updateInfomationBar()
    {
        int count = 0;

        foreach (House house in houses)
        {
            count += house.occupancy;
        }

        GameObject.Find("PeopleCount").GetComponent<UnityEngine.UI.Text>().text = "Normal: " + count;

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }

        GameObject.Find("ResourceCount").GetComponent<UnityEngine.UI.Text>().text = resource;
        GameObject.Find("UIBarPower").GetComponent<Text>().text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(this.PowerStored), Mathf.Round(this.powerSupply), Mathf.Round(this.powerDraw));
    }

    private Bounds OrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        Bounds bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public int cPop = 0;
    public int totalMiners = 0;
    public int totalJacks = 0;
    public int totalPowerWorkers = 0;
    public int maxPopulation = 0;
    public int maxMineWorkers = 0;
    public int maxMillWorkers = 0;

    void Update()
    {
        if (isMainMenu || Manager._instance.isPaused) return;

        int j = 0;

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
            if (managerScript == null)
            {
                managerScript = gameManager.GetComponent<Manager>();
            }

            PowerStored = 0;
            Time.timeScale = 0;
            managerScript.ShowGameOver();
            gameOver = true;
        }


        cPop = 0;
        totalMiners = 0;
        totalJacks = 0;
        totalPowerWorkers = 0;
        maxPopulation = 0;

        maxMineWorkers = 0;
        maxMillWorkers = 0;
        foreach (Building building in Building.buildings)
        {
            switch (building.tile.building.name)
            {
                case "Mine":
                    Mine mine = (Mine)building;
                    maxMineWorkers += mine.maxWorkers;
                    break;
                case "Mill":
                    Mill mill = (Mill)building;
                    maxMillWorkers += mill.maxWorkers;
                    break;
            }
        }

        foreach (House house in houses)
        {
            cPop += house.occupancy;
            maxPopulation += house.maxCapacity;

            int mi = totalMiners;
            int jo = totalJacks;

            totalMiners = Mathf.Min(maxMineWorkers, totalMiners + house.miners);
            totalJacks  = Mathf.Min(maxMillWorkers, totalJacks + house.lumberjacks);
            
            house.lumberProvided = totalJacks - jo;
            house.minerProvided = totalMiners - mi;
            //totalPowerWorkers += house.power;
        }

        GameObject.Find("PeopleCount").GetComponent<UnityEngine.UI.Text>().text = string.Format("Total: {0}/{1}  Miners: {2}/{3}  Lumberjacks: {4}/{5}", cPop, maxPopulation, totalMiners, maxMineWorkers, totalJacks, maxMillWorkers);
        foreach (Building building in Building.buildings)
        {
            switch (building.tile.building.name)
            {
                case "Mine":
                    Mine mine = (Mine)building;
                    if (mine.workers >= mine.maxWorkers) continue;
                    int inc = Mathf.Min(mine.maxWorkers, totalMiners);
                    mine.workers = inc;
                    totalMiners -= inc;
                    break;
                case "Mill":
                    Mill mill = (Mill)building;
                    if (mill.workers >= mill.maxWorkers) continue;
                    inc = Mathf.Min(mill.maxWorkers, totalJacks);
                    mill.workers = inc;
                    totalJacks -= inc;
                    break;
            }
        }

        foreach (KeyValuePair<string, BuildingType> bType in buildings)
        {
            if (Input.GetKey(bType.Value.hotkey))
            {
                if (hoverTile != null)
                {
                    if (hoverTile.building == null)
                    {
                        if (bType.Value.buildable && canBuild(hoverTile, bType.Value))
                        {
                            GenWorld._instance.buildOnTile(bType.Key);
                        }
                    }
                }
            }
        }

        this.PowerSupply = 0;
        float powerLimitOverall = 0;

        foreach (Building building in Building.buildings)
        {
            if (building.tile.building.name != "PowerPlant")
                powerLimitOverall += building.powerLimit;
        }

        foreach (PowerPlant building in plants)
        {
            this.PowerSupply += building.powerStored;
            building.powerStored = 0;
        }

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }
        GameObject.Find("ResourceCount").GetComponent<UnityEngine.UI.Text>().text = resource;
        PowerStored += (Mathf.Round(this.PowerSupply) * Time.deltaTime) - (Mathf.Round(this.powerDraw) * Time.deltaTime);
        GameObject.Find("UIBarPower").GetComponent<Text>().text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(this.PowerStored), Mathf.Round(this.powerSupply), Mathf.Round(this.powerDraw));

        GameObject PowerForground = GameObject.Find("PowerForground");
        PowerForground.transform.localScale = new Vector3(Mathf.Min(/*(GenWorld._instance.powerSupply / GenWorld._instance.powerDraw)*/this.PowerStored / powerLimitOverall, 1), 1, 1);

        if (Input.GetKey(KeyCode.UpArrow)) if (OrthographicBounds().max.y < (worldHeight + 2) * 1.28) Camera.main.transform.Translate(new Vector3(0, 1));
        if (Input.GetKey(KeyCode.DownArrow)) if (OrthographicBounds().min.y > -2 * 1.28) Camera.main.transform.Translate(new Vector3(0, -1));
        if (Input.GetKey(KeyCode.LeftArrow)) if (OrthographicBounds().min.x > -2 * 1.28) Camera.main.transform.Translate(new Vector3(-1, 0));
        if (Input.GetKey(KeyCode.RightArrow)) if (OrthographicBounds().max.x < (worldWidth + 2) * 1.28) Camera.main.transform.Translate(new Vector3(1, 0));
        if (Input.GetKey(KeyCode.PageDown)) Camera.main.orthographicSize += zoomSpeed;
        if (Input.GetKey(KeyCode.PageUp)) Camera.main.orthographicSize -= zoomSpeed;

        if (Camera.main.orthographicSize < minOrthoSize)
        {
            Camera.main.orthographicSize = minOrthoSize;
        }

        if (Camera.main.orthographicSize > maxOrthoSize)
        {
            Camera.main.orthographicSize = maxOrthoSize;
        }

        if (houses.Count % Mathf.Pow(3, expandCount + 1) == 0)
        {
            expandMap(5);
            expandCount++;
        }
    }
    [SerializeField]int expandCount = 0;

    public static GameObject menu;

    public void closeMenu()
    {
        Destroy(menu);
        menu = null;
    }

    void registerBuildings()
    {
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

        for (int ores = 0; ores < Random.Range(6, 10); ores++)
        {
            GameObject cTile = null;

            while (cTile == null || cTile.GetComponent<Tile>().building != null || cTile.GetComponent<Tile>().ore != null)
            {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            switch (Random.Range(0, 4))
            {
                case (0):
                case (2):
                    {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                        break;
                    }
                case (1):
                    {
                        cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
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
                        break;
                    }
            }
        }

        maxOrthoSize += 2;
    }

    public Vector3 getTileCoord(Vector3 vector)
    {
        return vector / 1.28f;
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

    bool canBuild(Tile tile, BuildingType building)
    {
        if (tile.ore == null)
        {
            if (building.name == "Lab") return false;
            else if (building.name == "PowerPlant" && buildings["PowerPlant"].buildable) return true;
            else return false;
        }
        else if (building.name == "Mill" && (tile.ore != null || tile.ore.mine == MineType.Mill)  && buildings["Mill"].buildable) return true;
        else if (building.name == "Mine" && (tile.ore != null || tile.ore.mine == MineType.Shaft) && buildings["Mine"].buildable) return true;
        else return false;
    }
}
