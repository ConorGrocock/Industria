using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Building
{

    /// <summary>
    /// Mining speed
    /// amount of ore to mine per second.
    /// </summary>
    public float miningSpeed = 2.0f;
    float timeSinceLastMine;
    public int workers = 0;
    public int maxWorkers = 5;

    public new Sprite sprite
    {
        get
        {
            return Resources.Load("Sprites/Building/Mine/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        timeSinceLastMine = Time.time;
    }

    // Update is called once per frame
    int i = 0;
    int lastWorkers = 0;
    void Update()
    {
        if (Time.time - timeSinceLastMine >= 1f / miningSpeed)
        {
            timeSinceLastMine = Time.time;
            //if (tile.ore.amount >= (int)miningSpeed)
            //{
            //tile.ore.amount -= (int)miningSpeed;
            if (tile.ore == null) return;
            GenWorld._instance.Resources[tile.ore.type] += (int)workers;
            //}
        }

        if (workers != lastWorkers)
        {
            lastWorkers = workers;
            List<Villager> villagers = new List<Villager>();
            for (int i = 0; i < workers; i++)
            {
                Villager villager = new Villager();
                villager.role = VillagerRole.Miner;
                villagers.Add(villager);
            }
            displayHeads(villagers);
        }
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Wood, 5);
        GenWorld._instance.buildings.Add("Mine", new BuildingType("Mine", this, Resources.Load("Sprites/Building/Mine/1", typeof(Sprite)) as Sprite, required, KeyCode.W, MineType.Shaft));
    }
}
