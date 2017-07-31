using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab : Building {
    
    public GameObject Villager;
    
    // Use this for initialization
    void Start (){
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    public void register() {
        GenWorld._instance.buildings.Add("Lab", new BuildingType("Lab", this, Resources.Load("Sprites/Building/Lab/1", typeof(Sprite)) as Sprite));
    }
}
