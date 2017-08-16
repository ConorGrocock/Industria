﻿using UnityEngine;

public class UtilityManager : MonoBehaviourSingleton<UtilityManager>
{
    public Bounds GetOrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        Bounds bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public Vector3 getTileCoord(Vector3 vector)
    {
        return vector / 1.28f;
    }

    public bool canBuild(Tile tile, BuildingType building)
    {
        if (tile.ore == null)
        {
            if (building.name == "Lab") return false;
            else if (building.name == "PowerPlant" && BuildingManager._instance.buildings["PowerPlant"].buildable) return true;
            else return false;
        }
        else if (building.name == "Mill" && (tile.ore != null || tile.ore.mine == MineType.Mill) && BuildingManager._instance.buildings["Mill"].buildable) return true;
        else if (building.name == "Mine" && (tile.ore != null || tile.ore.mine == MineType.Shaft) && BuildingManager._instance.buildings["Mine"].buildable) return true;
        else return false;
    }

    public string randomName()
    {
        return names[Random.Range(0, names.Length - 1)];
    }

    private string[] names = new string[]
    {
        "OLIVER ",
        "JACK ",
        "HARRY ",
        "GEORGE ",
        "JACOB ",
        "CHARLIE ",
        "NOAH ",
        "WILLIAM ",
        "THOMAS ",
        "OSCAR ",
        "JAMES ",
        "MUHAMMAD ",
        "HENRY ",
        "ALFIE ",
        "LEO ",
        "JOSHUA ",
        "FREDDIE ",
        "ETHAN ",
        "ARCHIE ",
        "ISAAC ",
        "JOSEPH ",
        "ALEXANDER ",
        "SAMUEL ",
        "DANIEL ",
        "LOGAN ",
        "EDWARD ",
        "LUCAS ",
        "MAX ",
        "MOHAMMED ",
        "BENJAMIN ",
        "MASON ",
        "HARRISON ",
        "THEO ",
        "JAKE ",
        "SEBASTIAN ",
        "FINLEY ",
        "ARTHUR ",
        "ADAM ",
        "DYLAN ",
        "RILEY ",
        "ZACHARY ",
        "TEDDY ",
        "DAVID ",
        "TOBY ",
        "THEODORE ",
        "ELIJAH ",
        "MATTHEW ",
        "JENSON ",
        "JAYDEN ",
        "HARVEY "
    };
}
