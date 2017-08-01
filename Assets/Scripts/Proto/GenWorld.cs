﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenWorld : MonoBehaviour
{
    public static GenWorld _instance;
    public GameObject gameManager;
    private Manager managerScript;

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    public bool gameOver;

    public bool isMainMenu;
    public bool isPaused;

    float PowerSupply;
    public float powerSupply
    {
        get { return PowerSupply; }
        set { PowerSupply = value; }
    }

    float PowerStored = 100f;

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

    public Dictionary<string, BuildingType> buildings;
    public Dictionary<OreTypes, int> Resources;

    public List<House> houses = new List<House>();
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

            foreach (Button b in buttons) {
                if (value.ore == null) {
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

    public Sprite[] sprites;

    public GameObject[][] tiles;

    // Use this for initialization
    void Start()
    {
        if (_instance == null) _instance = this;
        else Debug.LogError("YOU HAVE FUCKED UP. You have more than one World gen class");

        isMainMenu = SceneManager.GetActiveScene().name == "_Menu";

        if (managerScript == null && !isMainMenu)
        {
            managerScript = gameManager.GetComponent<Manager>();
        }

        if (!isMainMenu)
            buildingPanel.SetActive(false);

        buildings = new Dictionary<string, BuildingType>();
        registerBuildings();

        Resources = new Dictionary<OreTypes, int>();
        Resources.Add(OreTypes.Coal, 0);
        Resources.Add(OreTypes.Copper, 10);
        //Resources.Add(OreTypes.Iron, 0);
        Resources.Add(OreTypes.Wood, 100);


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
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
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

    void Update()
    {
        if (isMainMenu || Manager._instance.isPaused) return;

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

        int count = 0;

        int totalMiners = 0;
        int totalJacks = 0;
        int totalPowerWorkers = 0;

        foreach (House house in houses)
        {
            count += house.occupancy;

            totalMiners += house.miners;
            totalJacks += house.lumberjacks;
            totalPowerWorkers += house.power;
        }

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

        GameObject.Find("PeopleCount").GetComponent<UnityEngine.UI.Text>().text = "Normal: " + count;

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
            Debug.Log(powerSupply);
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
    }

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

    public Vector3 getTileCoord(Vector3 vector)
    {
        return vector / 1.28f;
    }

    public void buildOnTile(string building) {
        foreach (KeyValuePair<OreTypes, int> cost in buildings[building].costs) {
            GenWorld._instance.Resources[cost.Key] -= cost.Value;
        }
        buildTile.building = buildings[building];
        buildingPanel.SetActive(false);
        BuildTile = null;
    }
}
