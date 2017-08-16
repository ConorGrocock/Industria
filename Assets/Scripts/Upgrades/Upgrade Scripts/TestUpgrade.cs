using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUpgrade : UpgradeScript
{
    public float newMiningSpeed = 4.0f;

    public override void ApplyUpgradeEffect()
    {
        Debug.Log("Unlocked mine upgrade!");
        ((Mine)BuildingManager._instance.buildings["Mine"].script).miningSpeed = newMiningSpeed;
        Debug.Log(((Mine)BuildingManager._instance.buildings["Mine"].script).miningSpeed);
    }

    public override void OnUpgradeUpdate()
    {
        int i = 0;

        foreach (Mine mine in FindObjectsOfType<Mine>())
        {
            FindObjectsOfType<Mine>()[i].miningSpeed = newMiningSpeed;
            i++;
        }
    }
}
