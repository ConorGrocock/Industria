﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building {

    /// <summary>
    /// Mining speed
    /// amount of ore to mine per second.
    /// </summary>
    float miningSpeed = 10;
    float timeSinceLastMine;

    public new Sprite sprite {
        get {
            return Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;

    // Use this for initialization
    void Start() {

        base.Start();
        timeSinceLastMine = Time.time;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - timeSinceLastMine > 1) {
            timeSinceLastMine = Time.time;
            if (tile.ore.amount > (int)miningSpeed) {
                tile.ore.amount -= (int)miningSpeed;
                GenWorld._instance.Resources[tile.ore.type] += (int)miningSpeed;
            }
        }
    }

    public void register() {
        GenWorld._instance.buildings.Add("Mill", new BuildingType("Mill", this, Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite));
    }
}
