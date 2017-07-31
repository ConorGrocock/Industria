using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : Building {

    public float coalBurnTime = 10f;
    float cBurnTime = 0;
    float powerPerSecond = 1f;
    
    // Use this for initialization
    void Start (){
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
        cBurnTime -= Time.deltaTime;
        if(cBurnTime < 0) {
            GenWorld._instance.Resources[OreTypes.Coal]--;
        }
        GenWorld._instance.power += powerPerSecond * Time.deltaTime;
    }

    public void register() {
        GenWorld._instance.buildings.Add("POwerPlant", new BuildingType("PowerPlant", this, Resources.Load("Sprites/Building/PowerPlant/1", typeof(Sprite)) as Sprite));
    }
}
