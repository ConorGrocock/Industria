using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType{
    public Component script;
    public Sprite sprite;
    public String name;
    public KeyCode hotkey;

    public Dictionary<OreTypes, int> costs;

    public bool buildable {
        get {
            foreach (KeyValuePair<OreTypes, int> cost in costs) {
                if (GenWorld._instance.Resources[cost.Key] < cost.Value) return false;
            }
            return true;
        }
    }

    public BuildingType(String name, Component script, Sprite sprite, Dictionary<OreTypes, int> costs, KeyCode hotkey) {
        this.name = name;
        this.script = script;
        this.sprite = sprite;
        this.costs = costs;
        this.hotkey = hotkey;
    }
}

