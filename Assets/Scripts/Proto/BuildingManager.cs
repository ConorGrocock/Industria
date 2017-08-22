﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviourSingleton<BuildingManager>
{
    /// <summary>
    /// Current population
    /// </summary>
    public int cPop = 0;
    /// <summary>
    /// Total number of miners
    /// </summary>
    public int totalMiners = 0;
    /// <summary>
    /// Total number of lumberjacks
    /// </summary>
    public int totalJacks = 0;
    /// <summary>
    /// Total number of power workers.
    /// 
    /// Not used at the moment.
    /// </summary>
    public int totalPowerWorkers = 0;
    /// <summary>
    /// Maximum number of people
    /// Sum of houses'
    /// </summary>
    public int maxPopulation = 0;
    /// <summary>
    /// Maximum number of workers needed to work all the mines
    /// </summary>
    public int maxMineWorkers = 0;

    /// <summary>
    /// Maximum number of workers needed to work all the lumber mills
    /// </summary>
    public int maxMillWorkers = 0;

    [Header("UI")]
    public Text PeopleCount;
    public Text ResourceCount;
    public Text UIBarPower;
    public GameObject PowerForground;

    [Header("Building")]
    public Tile hoverTile;
    public GameObject buildingPanel;

    /// <summary>
    /// A list of building types that can be matched to a string
    /// </summary>
    [HideInInspector]
    public Dictionary<string, BuildingType> buildings;

    /// <summary>
    /// List of houses
    /// </summary>
    [HideInInspector]
    public List<House> houses = new List<House>();

    /// <summary>
    /// List of power plants
    /// </summary>
    [HideInInspector]
    public List<PowerPlant> plants = new List<PowerPlant>();
    
    Tile BuildTile;
    /// <summary>
    /// Tile the user selects to build a building on
    /// </summary>
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

    void Awake()
    {
        buildings = new Dictionary<string, BuildingType>();
        registerBuildings();
    }

    /// <summary>
    /// <para>Register buildings</para>
    /// <para>Buidlings should have there register methods callded here</para>
    /// </summary>
    void registerBuildings()
    {
        // TODO: Get rid of "new" keyword
        new House().register();
        new Mine().register();
        new Mill().register();
        //new Lab().register();
        new PowerPlant().register();
    }

    void Update()
    {
        if (Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        cPop = 0;
        totalMiners = 0;
        totalJacks = 0;
        totalPowerWorkers = 0;
        maxPopulation = 0;

        maxMineWorkers = 0;
        maxMillWorkers = 0;
        foreach (Building building in Building.buildings)
        {
            switch (building.tile.buildingType.name)
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
            totalJacks = Mathf.Min(maxMillWorkers, totalJacks + house.lumberjacks);

            house.lumberProvided = totalJacks - jo;
            house.minerProvided = totalMiners - mi;
            //totalPowerWorkers += house.power;
        }

        PeopleCount.text = string.Format("Total: {0}/{1}  Miners: {2}/{3}  Lumberjacks: {4}/{5}", cPop, maxPopulation, totalMiners, maxMineWorkers, totalJacks, maxMillWorkers);
        foreach (Building building in Building.buildings)
        {
            switch (building.tile.buildingType.name)
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
                    if (hoverTile.buildingType == null)
                    {
                        if (bType.Value.buildable && UtilityManager._instance.canBuild(hoverTile, bType.Value))
                        {
                            buildOnTile(bType.Key);
                        }
                    }
                }
            }
        }

        PowerManager._instance.powerSupply = 0;
        float powerLimitOverall = 0;

        foreach (Building building in Building.buildings)
        {
            if (building.tile.buildingType.name != "PowerPlant")
                powerLimitOverall += building.powerLimit;
        }

        foreach (PowerPlant building in plants)
        {
            PowerManager._instance.powerSupply += building.powerStored;
            building.powerStored = 0;
        }

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in GenWorld._instance.Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }

        ResourceCount.text = resource;
        PowerManager._instance.powerStored += (Mathf.Round(PowerManager._instance.powerSupply) * Time.deltaTime) - (Mathf.Round(PowerManager._instance.powerDraw) * Time.deltaTime);
        updatePowerInfo();

        PowerForground.transform.localScale = new Vector3(Mathf.Min(/*(GenWorld._instance.powerSupply / GenWorld._instance.powerDraw)*/PowerManager._instance.powerStored / powerLimitOverall, 1), 1, 1);

        if (houses.Count > Mathf.Min(10, Mathf.Pow(2f, GenWorld._instance.expandCount)))
        {
            GenWorld._instance.expandMap(5);
            GenWorld._instance.expandCount++;
        }
    }

    /// <summary>
    /// Update the infomation bar at the top of the screen
    /// </summary>
    public void updateInformationBar()
    {
        int count = 0;

        foreach (House house in houses)
        {
            count += house.occupancy;
        }

        PeopleCount.text = "Normal: " + count;

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in GenWorld._instance.Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }

        ResourceCount.text = resource;
        UIBarPower.text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(PowerManager._instance.powerStored), Mathf.Round(PowerManager._instance.powerSupply), Mathf.Round(PowerManager._instance.powerDraw));
    }

    /// <summary>
    /// Updates the power information on the information bar.
    /// Exists so it can be called from <see cref="PowerManager"/> on game over.
    /// </summary>
    public void updatePowerInfo()
    {
        UIBarPower.text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(PowerManager._instance.powerStored), Mathf.Round(PowerManager._instance.powerSupply), Mathf.Round(PowerManager._instance.powerDraw));
    }

    /// <summary>
    /// Build a building on a tile
    /// </summary>
    /// <param name="building">The tyoe of building to be built, As registered in the <see cref="Buildings"/>building</see> dictionary</param>
    public void buildOnTile(string building)
    {
        foreach (KeyValuePair<OreTypes, int> cost in buildings[building].costs)
        {
            GenWorld._instance.Resources[cost.Key] -= cost.Value;
        }

        if (buildTile != null) buildTile.buildingType = buildings[building];
        else if (hoverTile != null) hoverTile.buildingType = buildings[building];
        buildingPanel.SetActive(false);
        buildTile = null;
    }
}
