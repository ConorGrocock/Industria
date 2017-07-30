using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingType{
    public Component script;
    public Sprite sprite;

    public BuildingType(Component script, Sprite sprite) {
        this.script = script;
        this.sprite = sprite;
    }
}

