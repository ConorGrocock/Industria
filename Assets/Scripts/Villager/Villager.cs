using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VillagerRole
{
    None,
    Miner,
    Lumberjack
}

public class Villager
{
    private SpriteRenderer spriteRenderer;

    public string Vname;
    public House house;

    public VillagerRole role;

    public Villager(House house = null)
    {
        Vname = UtilityManager._instance.randomName();
        this.house = house;
        role = VillagerRole.None;
    }

    public void Update()
    {
        if (role == VillagerRole.None)
        {
            if (BuildingManager._instance.maxMineWorkers > BuildingManager._instance.totalMiners)
            {
                role = VillagerRole.Miner;
                house.updateHeads();
            }
            else if (BuildingManager._instance.maxMillWorkers > BuildingManager._instance.totalJacks)
            {
                role = VillagerRole.Lumberjack;
                house.updateHeads();
            }
        }
    }

    public Sprite getSprite()
    {
        switch (role)
        {
            case VillagerRole.None:
                return Resources.Load<Sprite>("Sprites/Villager/3");
            case VillagerRole.Miner:
                return Resources.Load<Sprite>("Sprites/Villager/1");
            case VillagerRole.Lumberjack:
                return Resources.Load<Sprite>("Sprites/Villager/4");
            default:
                return null;
        }
    }

    public Sprite getHead()
    {
        switch (role)
        {
            case VillagerRole.None:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/3");
            case VillagerRole.Miner:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/1");
            case VillagerRole.Lumberjack:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/4");
            default:
                return null;
        }
    }

    public void assigned()
    {
        house.updateHeads();
    }
}
