using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            if (GenWorld._instance.Resources[OreTypes.Coal] > 1) {
                GenWorld._instance.Resources[OreTypes.Coal]--;
                cBurnTime = coalBurnTime;
            }
        }
        GenWorld._instance.power += powerPerSecond * Time.deltaTime;
    }

    public void register() {
        GenWorld._instance.buildings.Add("PowerPlant", new BuildingType("PowerPlant", this, Resources.Load("Sprites/Building/PowerPlant/1", typeof(Sprite)) as Sprite));
    }
    public bool menuDrawn = false;
    public override void clickMenu(GameObject top, GameObject panel) {
        if (!menuDrawn) {
            menuDrawn = true;
            GameObject powerGo = new GameObject();
            powerGo.transform.parent = panel.transform;
            Image power = powerGo.AddComponent<Image>();
            powerGo.transform.localPosition = new Vector3(-200, 150);
            powerGo.name = "PowerGraph";
            power.color = Color.yellow;

            GameObject coalGo = new GameObject();
            coalGo.transform.parent = panel.transform;
            Image coal = coalGo.AddComponent<Image>();
            coalGo.transform.localPosition = new Vector3(-400, 150);
            coalGo.name = "CoalGraph";
            coal.color = Color.red;
        }
    }
}
