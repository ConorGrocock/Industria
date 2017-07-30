using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType{
    public Component script;
    public Sprite sprite;
    public String name;

    public BuildingType(String name, Component script, Sprite sprite) {
        this.name = name;
        this.script = script;
        this.sprite = sprite;
    }
}

