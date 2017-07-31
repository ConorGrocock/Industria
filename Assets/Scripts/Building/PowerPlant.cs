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
        GenWorld._instance.powerSupply += powerPerSecond * Time.deltaTime;
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
            powerGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            powerGo.transform.localPosition = new Vector3(-400, 240, 1);
            powerGo.name = "PowerGraph";
            power.color = Color.black;
            power.transform.localScale = new Vector3(1, 0.25f, 1);

            GameObject powerBackGo = new GameObject();
            powerBackGo.transform.parent = panel.transform;
            Image powerBack = powerBackGo.AddComponent<Image>();
            powerBackGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            powerBackGo.transform.localPosition = new Vector3(-400, 240, -1);
            powerBackGo.name = "powerBackGo";
            powerBack.color = Color.yellow;
            powerBack.transform.localScale = new Vector3(Mathf.Min((GenWorld._instance.powerSupply / GenWorld._instance.powerDraw),1), 0.25f, 1);


            GameObject powerInfomation = new GameObject();
            Text info = powerInfomation.AddComponent<Text>();
            info.text = string.Format("{0} Power generated \n {1} Power drawn", Mathf.Round(GenWorld._instance.powerSupply), Mathf.Round(GenWorld._instance.powerDraw));
            powerInfomation.transform.parent = panel.transform;
            powerInfomation.transform.localPosition = new Vector3(-400, 175);
            info.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            powerInfomation.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
