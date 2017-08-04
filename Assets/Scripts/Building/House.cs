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

    List<Villager> occupants = new List<Villager>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        GenWorld._instance.houses.Add(this);
        timeToNextBaby = babyTime;

        occupants.Add(new Villager());
        occupants[0].role = VillagerRole.Miner;
        occupants.Add(new Villager());
        occupants[1].role = VillagerRole.None;

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
        if (GenWorld._instance.isMainMenu || Manager._instance.isPaused) return;

        if (currentMenu == GenWorld.menu && currentMenu != null)
        {
            //if (lastPopulation != occupants.Count)
            //{
            //    GenWorld._instance.closeMenu();
            //    guiUpdate = true;
            //    clickMenu(gameObject, tile.createMenu());
            //    lastPopulation = occupants.Count;
            //}
        }

        miners = 0;
        lumberjacks = 0;
        power = 0;
        for (int i = 0; i < occupants.Count; i++)
        {
            switch (occupants[i].role)
            {
                case VillagerRole.None:
                    power++;
                    break;
                case VillagerRole.Miner:
                    miners++;
                    break;
                case VillagerRole.Lumberjack:
                    lumberjacks++;
                    break;
            }
        }

        if (timeToNextBaby <= 0)
        {
            timeToNextBaby = babyTime;
            babyTime *= 1.5f;
            full = occupants.Count >= maxCapacity;

            if (occupants.Count >= maxCapacity)
            {
                float xOffset = Random.Range(-2, 2);
                float yOffset = Random.Range(-2, 2);

                Vector3 cPos = GenWorld._instance.getTileCoord(transform.position);

                if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)] != null)
                {
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null)
                    {
                        cPos = GenWorld._instance.getTileCoord(new Vector3((cPos.x + xOffset), (cPos.y + yOffset)));
                        xOffset = Random.Range(-2, 2);
                        yOffset = Random.Range(-2, 2);
                    }
                    if (GenWorld._instance.tiles.Length - 1 > (int)(cPos.x + xOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)].Length - 1 > (int)(cPos.y + yOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null)
                    {
                        return;
                    }
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null)
                    {
                        cPos = GenWorld._instance.getTileCoord(new Vector3((cPos.x + xOffset), (cPos.y + yOffset)));
                        xOffset = Random.Range(-2, 2);
                        yOffset = Random.Range(-2, 2);
                    }

                    if (GenWorld._instance.tiles.Length - 1 > (int)(cPos.x + xOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)].Length - 1 > (int)(cPos.y + yOffset) && GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null)
                    {
                        return;
                    }
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building == null)
                        GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building = GenWorld._instance.buildings["House"];
                }
                //Spawn new house
                //Where?
                //How?
                //Spawn time of new house?
            }
            else
            {
                if (GenWorld._instance.maxMineWorkers > GenWorld._instance.totalMiners)
                    occupants.Add(new Villager(VillagerRole.Miner));
                else if (GenWorld._instance.maxMillWorkers > GenWorld._instance.totalJacks)
                    occupants.Add(new Villager(VillagerRole.Lumberjack));
                else occupants.Add(new Villager((VillagerRole)Random.Range(0, 3)));
            }
        }
        timeToNextBaby -= Time.deltaTime;
        this.powerDraw = basePowerDraw * (occupants.Count);
    }

    public void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 0);
        required.Add(OreTypes.Wood, 5);
        GenWorld._instance.buildings.Add("House", new BuildingType("House", this, Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite, required, KeyCode.None));
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

    public override void clickMenu(GameObject top, GameObject panel)
    {
        if (GenWorld.menu != panel)
        {
            shown = 0;
            GenWorld.menu = panel;
        }
        else if (!guiUpdate) return;

        guiUpdate = false;

        dropdowns.Clear();

        currentMenu = panel;

        GameObject.Find("Occupants").GetComponent<UnityEngine.UI.Text>().text = occupants.Count + "";
        GameObject person = Resources.Load<GameObject>("Prefabs/UI/UIPerson");
        GameObject[] profiles = new GameObject[occupants.Count];
        for (int i = 0; i < occupants.Count; i++)
        {
            if (i < shown) continue;
            //profiles[i] = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIPerson"));
            profiles[i] = Instantiate(person);
            profiles[i].transform.SetParent(panel.transform);
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
}