using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building
{

    /// <summary>
    /// Mining speed
    /// amount of ore to mine per second.
    /// </summary>
    float miningSpeed = 5;
    float timeSinceLastMine;
    public int workers = 0;
    public int maxWorkers = 5;

    public new Sprite sprite
    {
        get
        {
            return Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        timeSinceLastMine = Time.time;

        if (this.tile.ore == null) Destroy(this.gameObject);
    }

    int lastWorkers = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeSinceLastMine > 1)
        {
            timeSinceLastMine = Time.time;
            if (tile.ore.amount > (int)miningSpeed)
            {
                tile.ore.amount -= (int)miningSpeed;
                GenWorld._instance.Resources[tile.ore.type] += (int)miningSpeed * workers;
            }
        }

        if (workers != lastWorkers)
        {
            lastWorkers = workers;
            List<Villager> villagers = new List<Villager>();
            for (int i = 0; i < workers; i++)
            {
                Villager villager = new Villager();
                villager.role = VillagerRole.Lumberjack;
                villagers.Add(villager);
            }
            displayHeads(villagers);
        }
    }

    public void register() {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 6);
        required.Add(OreTypes.Wood, 10);
        GenWorld._instance.buildings.Add("Mill", new BuildingType("Mill", this, Resources.Load<Sprite>("Sprites/Building/Mill/1"), required, KeyCode.Q, MineType.Mill));
    }
}
