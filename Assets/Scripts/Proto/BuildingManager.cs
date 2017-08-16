using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviourSingleton<BuildingManager>
{
    public int cPop = 0;
    public int totalMiners = 0;
    public int totalJacks = 0;
    public int totalPowerWorkers = 0;
    public int maxPopulation = 0;
    public int maxMineWorkers = 0;
    public int maxMillWorkers = 0;

    [Header("UI")]
    public Text ResourceCount;
    public Text UIBarPower;
    public GameObject PowerForground;

    void Start()
    {
		
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

        foreach (House house in GenWorld._instance.houses)
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

        GameObject.Find("PeopleCount").GetComponent<Text>().text = string.Format("Total: {0}/{1}  Miners: {2}/{3}  Lumberjacks: {4}/{5}", cPop, maxPopulation, totalMiners, maxMineWorkers, totalJacks, maxMillWorkers);
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

        foreach (KeyValuePair<string, BuildingType> bType in GenWorld._instance.buildings)
        {
            if (Input.GetKey(bType.Value.hotkey))
            {
                if (GenWorld._instance.hoverTile != null)
                {
                    if (GenWorld._instance.hoverTile.building == null)
                    {
                        if (bType.Value.buildable && UtilityManager._instance.canBuild(GenWorld._instance.hoverTile, bType.Value))
                        {
                            GenWorld._instance.buildOnTile(bType.Key);
                        }
                    }
                }
            }
        }

        GenWorld._instance.powerSupply = 0;
        float powerLimitOverall = 0;

        foreach (Building building in Building.buildings)
        {
            if (building.tile.building.name != "PowerPlant")
                powerLimitOverall += building.powerLimit;
        }

        foreach (PowerPlant building in GenWorld._instance.plants)
        {
            GenWorld._instance.powerSupply += building.powerStored;
            building.powerStored = 0;
        }

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in GenWorld._instance.Resources)
        {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }

        ResourceCount.text = resource;
        GenWorld._instance.PowerStored += (Mathf.Round(GenWorld._instance.powerSupply) * Time.deltaTime) - (Mathf.Round(GenWorld._instance.powerDraw) * Time.deltaTime);
        UIBarPower.text = string.Format("{0} Power stored  {1} Power generated  {2} Power drawn", Mathf.Round(GenWorld._instance.PowerStored), Mathf.Round(GenWorld._instance.powerSupply), Mathf.Round(GenWorld._instance.powerDraw));

        PowerForground.transform.localScale = new Vector3(Mathf.Min(/*(GenWorld._instance.powerSupply / GenWorld._instance.powerDraw)*/GenWorld._instance.PowerStored / powerLimitOverall, 1), 1, 1);

        if (GenWorld._instance.houses.Count > Mathf.Min(10, Mathf.Pow(2f, GenWorld._instance.expandCount)))
        {
            GenWorld._instance.expandMap(5);
            GenWorld._instance.expandCount++;
        }
    }
}
