using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerPlant : Building
{
    public float coalBurnTime = 1.5f;
    float cBurnTime = 0;
    float powerPerSecond = 3f;
    public float powerStored = 0f;

    public GameObject hoverPanel;
    private GameObject hoverPanelInstance;

    private Text oreTypeText;

    private Slider burningProgressSlider;
    private Text generatingText;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        powerDraw = 0.0f;
        hoverPanel = Resources.Load("Prefabs/UI/PlantHoverPanel", typeof(GameObject)) as GameObject;
        BuildingManager._instance.plants.Add(this);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        cBurnTime -= Time.deltaTime;//s
        if (cBurnTime <= 0)
        {
            if (GenWorld._instance.Resources[OreTypes.Coal] >= 1)
            {
                GenWorld._instance.Resources[OreTypes.Coal]--;
                cBurnTime = coalBurnTime;
                hideWarningSign();
            }
            else
            {
                showWarningSign(true);
            }
        }
        else
        {
            powerStored += (powerPerSecond);//* Time.deltaTime);
            hideWarningSign();
        }
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 1);
        required.Add(OreTypes.Wood, 10);
        BuildingManager._instance.buildings.Add("PowerPlant", new BuildingType("PowerPlant", this, Resources.Load("Sprites/Building/PowerPlant/1", typeof(Sprite)) as Sprite, required, KeyCode.E));
    }

    public bool menuDrawn = false;
    public override void clickMenu(GameObject top, GameObject panel)
    {
        //if (GenWorld.menu == panel) menuDrawn = false;
        //else GenWorld.menu = panel;
        //if (!menuDrawn)
        //{
        //    menuDrawn = true;
        //    GameObject powerGo = new GameObject();
        //    powerGo.transform.parent = panel.transform;
        //    Image power = powerGo.AddComponent<Image>();
        //    powerGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //    powerGo.transform.localPosition = new Vector3(-400, 240, 1);
        //    powerGo.name = "PowerGraph";
        //    power.color = Color.black;
        //    power.transform.localScale = new Vector3(1, 0.25f, 1);

        //    GameObject powerBackGo = new GameObject();
        //    powerBackGo.transform.parent = panel.transform;
        //    Image powerBack = powerBackGo.AddComponent<Image>();
        //    powerBackGo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        //    powerBackGo.transform.localPosition = new Vector3(-400, 240, -1);
        //    powerBackGo.name = "powerBackGo";
        //    powerBack.color = Color.yellow;
        //    powerBack.transform.localScale = new Vector3(Mathf.Min((GenWorld._instance.powerSupply / GenWorld._instance.powerDraw), 1), 0.25f, 1);


        //    GameObject powerInfomation = new GameObject();
        //    Text info = powerInfomation.AddComponent<Text>();
        //    info.text = string.Format("{0} Power generated \n {1} Power drawn", Mathf.Round(GenWorld._instance.powerSupply), Mathf.Round(GenWorld._instance.powerDraw));
        //    powerInfomation.transform.parent = panel.transform;
        //    powerInfomation.transform.localPosition = new Vector3(-400, 175);
        //    info.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        //    powerInfomation.transform.localScale = new Vector3(1, 1, 1);

        //    Text[] textPanels = panel.GetComponentsInChildren<Text>();
        //    foreach (Text text in textPanels) {
        //        //if (text.name == "Occupants") text.text = workers.ToString();
        //    }
        //}
    }

    public override void OnHover()
    {
        base.OnHover();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (hoverPanel != null)
        {
            if (hoverPanelInstance == null)
            {
                hoverPanelInstance = Instantiate(hoverPanel);
                hoverPanelInstance.transform.SetParent(canvasTransform);

                oreTypeText = hoverPanelInstance.transform.Find("OreTypeText").gameObject.GetComponent<Text>();

                burningProgressSlider = hoverPanelInstance.transform.Find("BurningProgress").gameObject.GetComponent<Slider>();
                generatingText = hoverPanelInstance.transform.Find("GeneratingText").gameObject.GetComponent<Text>();
            }

            hoverPanelInstance.transform.position = new Vector3(Input.mousePosition.x + 115, Input.mousePosition.y - 50);

            if (cBurnTime <= 0 && GenWorld._instance.Resources[OreTypes.Coal] <= 0)
            {
                oreTypeText.text = "Burning: Coal (EMPTY)";
                burningProgressSlider.value = 0;
                generatingText.text = "0 power per second";
            }
            else
            {
                oreTypeText.text = "Burning: Coal";
                burningProgressSlider.value = cBurnTime / coalBurnTime;
                generatingText.text = powerPerSecond + " power per second";
            }
        }
        else
        {
            Debug.LogError("[PowerPlant] [OnHover] Hover panel is null!");
        }

        //spriteRenderer.color = new Color(0.2f, 0.7f, 0.3f);
    }

    public override void OnHoverEnd()
    {
        base.OnHoverEnd();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (hoverPanelInstance != null)
        {
            Destroy(hoverPanelInstance);
            hoverPanelInstance = null;
        }

        oreTypeText = null;

        burningProgressSlider = null;
        generatingText = null;

        //spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
