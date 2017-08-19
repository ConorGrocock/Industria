using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class House : Building
{
    public float babyTime = 10f;
    [SerializeField]
    private float timeToNextBaby = 0f;

    public new Sprite sprite
    {
        get
        {
            return Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite;
        }
    }

    float basePowerDraw = 1.5f;

    [Space(20)]
    public int occupancy = 2;
    public int maxCapacity = 5;
    public bool full;

    public int lumberProvided = 0;
    public int minerProvided = 0;
    public int newHouseRadius = 5;

    public List<Villager> occupants = new List<Villager>();

    public GameObject hoverPanel;
    private GameObject hoverPanelInstance;

    private Image personOne;
    private Text personOneName;

    private Image personTwo;
    private Text personTwoName;

    private Image personThree;
    private Text personThreeName;

    private Image personFour;
    private Text personFourName;

    private Image personFive;
    private Text personFiveName;

    private Text powerDrawText;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        hoverPanel = Resources.Load("Prefabs/UI/HouseHoverPanel", typeof(GameObject)) as GameObject;

        BuildingManager._instance.houses.Add(this);
        timeToNextBaby = babyTime;
        babyTime = Mathf.Min((1 / Mathf.Max(1, (BuildingManager._instance.houses.Count / 10))) * babyTime,120);

        occupants.Add(new Villager(this));
        occupants[0].role = VillagerRole.Miner;
        occupants.Add(new Villager(this));
        occupants[1].role = VillagerRole.Miner;

        updateHeads();

    }

    public int miners = 0;
    public int lumberjacks = 0;
    public int power = 0;

    public void updateHeads()
    {
        List<Villager> roles = new List<Villager>();
        int miners = 0;
        int lumber = 0;
        foreach (Villager oc in occupants)
            if (oc.role == VillagerRole.None || (oc.role == VillagerRole.Miner && minerProvided < ++miners) || (oc.role == VillagerRole.Lumberjack && lumberProvided < ++lumber)) roles.Add(oc);
        displayHeads(roles);
    }

    int lastPopulation = 0;

    // Update is called once per frame
    void Update()
    {
        if (Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        if (currentMenu == GenWorld._instance.menu && currentMenu != null)
        {
            //if (lastPopulation != occupants.Count)
            //{
            //    GenWorld._instance.closeMenu();
            //    guiUpdate = true;
            //    clickMenu(gameObject, tile.createMenu());
            //    lastPopulation = occupants.Count;
            //}
        }

        int nminers = 0;
        int nlumberjacks = 0;
        int npower = 0;
        for (int i = 0; i < occupants.Count; i++)
        {
            switch (occupants[i].role)
            {
                case VillagerRole.None:
                    npower++;
                    break;
                case VillagerRole.Miner:
                    nminers++;
                    break;
                case VillagerRole.Lumberjack:
                    nlumberjacks++;
                    break;
            }
        }
        if (npower != power) updateHeads();
        if (nlumberjacks != lumberjacks) updateHeads();
        if (nminers != miners) updateHeads();
        miners = nminers;
        lumberjacks = nlumberjacks;
        power = npower;

        if (timeToNextBaby <= 0)
        {
            timeToNextBaby = babyTime;
            babyTime *= 1.5f;
            full = occupants.Count >= maxCapacity;

            if (occupants.Count >= maxCapacity)
            {
                float xOffset = Random.Range(-newHouseRadius, newHouseRadius);
                float yOffset = Random.Range(-newHouseRadius, newHouseRadius);

                Vector3 cPos = UtilityManager._instance.getTileCoord(transform.position);

                if (GenWorld._instance.tiles.Length - 1 > (int)(cPos.x + xOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)].Length - 1 > (int)(cPos.y + yOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)] != null)
                {
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null)
                    {
                        cPos = UtilityManager._instance.getTileCoord(new Vector3((cPos.x + xOffset), (cPos.y + yOffset)));
                        xOffset = Random.Range(-2, 2);
                        yOffset = Random.Range(-2, 2);
                    }
                    if (GenWorld._instance.tiles.Length > (int)(cPos.x + xOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)].Length > (int)(cPos.y + yOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null)
                    {
                        newHouseRadius++;
                        return;
                    }
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null)
                    {
                        cPos = UtilityManager._instance.getTileCoord(new Vector3((cPos.x + xOffset), (cPos.y + yOffset)));
                        xOffset = Random.Range(-newHouseRadius, newHouseRadius);
                        yOffset = Random.Range(-newHouseRadius, newHouseRadius);
                    }

                    if (GenWorld._instance.tiles.Length - 1 > (int)(cPos.x + xOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)].Length - 1 > (int)(cPos.y + yOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null)
                    {
                        newHouseRadius++;
                        return;
                    }
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building == null)
                        GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building = BuildingManager._instance.buildings["House"];
                }
                //Spawn new house
                //Where?
                //How?
                //Spawn time of new house?
            }
            else
            {
                occupants.Add(new Villager(this));
                //updateMenu();
            }
        }
        timeToNextBaby -= Time.deltaTime;
        updateMenu();
        this.powerDraw = basePowerDraw * (occupants.Count);

        foreach (Villager villager in occupants)
        {
            villager.Update();
        }
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 0);
        required.Add(OreTypes.Wood, 5);
        BuildingManager._instance.buildings.Add("House", new BuildingType("House", this, Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite, required, KeyCode.None));
    }
    
    int shown = 0;

    Dictionary<Dropdown, Image> dropdowns = new Dictionary<Dropdown, Image>();
    GameObject currentMenu = null;

    bool guiUpdate = false;

    public void dropDownChange()
    {
        int i = 0;

        foreach (KeyValuePair<Dropdown, Image> entry in dropdowns)
        {
            if (occupants.Count < i + 1) continue;
            if (occupants[i] == null) occupants[i] = new Villager();

            bool set = false;

            switch (entry.Key.value)
            {
                case 0:
                    if (occupants[i].role == VillagerRole.None) set = true;
                    occupants[i].role = VillagerRole.None;
                    break;
                case 1:
                    if (occupants[i].role == VillagerRole.Miner) set = true;
                    occupants[i].role = VillagerRole.Miner;
                    break;
                case 2:
                    if (occupants[i].role == VillagerRole.Lumberjack) set = true;
                    occupants[i].role = VillagerRole.Lumberjack;
                    break;
            }
            /*if (!set || !invShown) */
            if (entry.Value != null && !entry.Value.IsDestroyed()) entry.Value.sprite = occupants[i].getSprite();
            i++;
        }

        updateHeads();
    }

    public void updateMenu()
    {
        if (GameObject.Find("Occupants") == null || this.currentMenu == null) return;
        GameObject.Find("Occupants").GetComponent<UnityEngine.UI.Text>().text = occupants.Count + "";
        GameObject.Find("VillagerTime").GetComponent<UnityEngine.UI.Text>().text = Mathf.RoundToInt(timeToNextBaby) + "";

        GameObject person = Resources.Load<GameObject>("Prefabs/UI/UIPerson");
        GameObject[] profiles = new GameObject[occupants.Count];
        for (int i = 0; i < occupants.Count; i++)
        {
            if (i < shown) continue;
            profiles[i] = Instantiate(person);
            profiles[i].transform.SetParent(this.currentMenu.transform);
            RectTransform rt = (RectTransform)profiles[i].transform;
            rt.pivot = new Vector2(1f, 0f);
            rt.anchorMin = new Vector2(0, 0);
            rt.anchorMax = new Vector2(0, 0);

            profiles[i].transform.localPosition = new Vector3((i * 170) - 260, -100, 1);
            profiles[i].GetComponentInChildren<Text>().text = occupants[i].Vname;
            profiles[i].GetComponentInChildren<Dropdown>().value = (int)occupants[i].role;
            profiles[i].GetComponentInChildren<Dropdown>().onValueChanged.AddListener((action) => { dropDownChange(); });
            dropdowns.Add(profiles[i].GetComponentInChildren<Dropdown>(), profiles[i].GetComponentInChildren<Image>());
            //profiles[i].GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Villager/" + Random.Range(1, 4));
        }

        dropDownChange();

        shown = occupants.Count;
    }

    public override void clickMenu(GameObject top, GameObject panel)
    {
        if (GenWorld._instance.menu != panel)
        {
            shown = 0;
            GenWorld._instance.menu = panel;
        }
        else if (!guiUpdate) return;

        guiUpdate = false;

        dropdowns.Clear();

        currentMenu = panel;

        updateMenu();
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

                personOne = hoverPanelInstance.transform.Find("PersonOne").gameObject.GetComponent<Image>();
                personOneName = personOne.transform.Find("Name").gameObject.GetComponent<Text>();

                personTwo = hoverPanelInstance.transform.Find("PersonTwo").gameObject.GetComponent<Image>();
                personTwoName = personTwo.transform.Find("Name").gameObject.GetComponent<Text>();

                personThree = hoverPanelInstance.transform.Find("PersonThree").gameObject.GetComponent<Image>();
                personThreeName = personThree.transform.Find("Name").gameObject.GetComponent<Text>();

                personFour = hoverPanelInstance.transform.Find("PersonFour").gameObject.GetComponent<Image>();
                personFourName = personFour.transform.Find("Name").gameObject.GetComponent<Text>();

                personFive = hoverPanelInstance.transform.Find("PersonFive").gameObject.GetComponent<Image>();
                personFiveName = personFive.transform.Find("Name").gameObject.GetComponent<Text>();

                powerDrawText = hoverPanelInstance.transform.Find("PowerDraw").gameObject.GetComponent<Text>();
            }

            personOne.gameObject.SetActive(true);
            personTwo.gameObject.SetActive(true);
            personThree.gameObject.SetActive(true);
            personFour.gameObject.SetActive(true);
            personFive.gameObject.SetActive(true);

            hoverPanelInstance.transform.position = new Vector3(Input.mousePosition.x + 105, Input.mousePosition.y - 105);

            switch (occupants.Count)
            {
                case (2):
                    personThree.gameObject.SetActive(false);
                    personFour.gameObject.SetActive(false);
                    personFive.gameObject.SetActive(false);
                    break;
                case (3):
                    personFour.gameObject.SetActive(false);
                    personFive.gameObject.SetActive(false);
                    break;
                case (4):
                    personFive.gameObject.SetActive(false);
                    break;
                case (5):
                    break;
                default:
                    Debug.LogError("[House] [OnHover] Unexpected occupant count for hover panel: " + occupants.Count);
                    break;
            }

            int i = 1;

            foreach (Villager occupant in occupants)
            {
                switch (i)
                {
                    case (1):
                        personOne.sprite = occupant.getHead();
                        personOneName.text = occupant.Vname;
                        break;
                    case (2):
                        personTwo.sprite = occupant.getHead();
                        personTwoName.text = occupant.Vname;
                        break;
                    case (3):
                        personThree.sprite = occupant.getHead();
                        personThreeName.text = occupant.Vname;
                        break;
                    case (4):
                        personFour.sprite = occupant.getHead();
                        personFourName.text = occupant.Vname;
                        break;
                    case (5):
                        personFive.sprite = occupant.getHead();
                        personFiveName.text = occupant.Vname;
                        break;
                    default:
                        Debug.LogError("[House] [OnHover] Unexpected iterator for setting details on hover panel: " + i);
                        break;
                }

                i++;
            }

            powerDrawText.text = "Power Draw: " + powerDraw;
        }
        else
        {
            Debug.LogError("Hover panel is null!");
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

        personOne = null;
        personOneName = null;

        personTwo = null;
        personTwoName = null;

        personThree = null;
        personThreeName = null;

        personFour = null;
        personFourName = null;

        personFive = null;
        personFiveName = null;

        powerDrawText = null;

        //spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
