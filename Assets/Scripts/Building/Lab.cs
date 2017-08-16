using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab : Building
{
    public GameObject Villager;

    protected override void Start()
    {
        base.Start();
    }
	
	void Update()
    {
        
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 2);
        required.Add(OreTypes.Wood, 5);
        BuildingManager._instance.buildings.Add("Lab", new BuildingType("Lab", this, Resources.Load("Sprites/Building/Lab/1", typeof(Sprite)) as Sprite, required, KeyCode.R));
    }
}
