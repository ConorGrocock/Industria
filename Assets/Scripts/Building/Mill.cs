using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : Building {
    
    public new Sprite sprite {
        get {
            return Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;
    
    // Use this for initialization
    protected void Start (){
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void register() {
        GenWorld._instance.buildings.Add("Mill", new BuildingType(this, Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite));
    }
}
