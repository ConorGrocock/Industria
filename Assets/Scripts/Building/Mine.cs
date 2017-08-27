using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mine : Building
{
    /// <summary>
    /// Mining speed
    /// amount of ore to mine per second.
    /// </summary>
    public float miningSpeed = 2.0f;
    float timeSinceLastMine;
    public int workers = 0;
    public int maxWorkers = 5;

    public new Sprite sprite
    {
        get
        {
            return Resources.Load("Sprites/Building/Mine/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;

    public GameObject hoverPanel;
    private GameObject hoverPanelInstance;

    private Text oreTypeText;
    private Text assignMoreText;

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
    private Slider miningProgressSlider;

    private List<Villager> occupants = new List<Villager>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        timeSinceLastMine = Time.time;
        hoverPanel = Resources.Load("Prefabs/UI/MineHoverPanel", typeof(GameObject)) as GameObject;
    }

    // Update is called once per frame
    int i = 0;
    int lastWorkers = 0;
    protected override void Update()
    {
        base.Update();

        if (Time.time - timeSinceLastMine >= 1f / miningSpeed)
        {
            timeSinceLastMine = Time.time;
            //if (tile.ore.amount >= (int)miningSpeed)
            //{
            //tile.ore.amount -= (int)miningSpeed;
            if (tile.ore == null) return;
            GenWorld._instance.Resources[tile.ore.type] += (int)workers;
            //}
        }

        if (workers != lastWorkers)
        {
            lastWorkers = workers;
            occupants = new List<Villager>();
            for (int i = 0; i < workers; i++)
            {
                Villager villager = new Villager();
                villager.role = VillagerRole.Miner;
                occupants.Add(villager);
            }
            displayHeads(occupants);
        }
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Wood, 5);
        BuildingManager._instance.buildings.Add("Mine", new BuildingType("Mine", this, Resources.Load("Sprites/Building/Mine/1", typeof(Sprite)) as Sprite, required, InputManager._instance.mineHotkey, MineType.Shaft));
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
                assignMoreText = hoverPanelInstance.transform.Find("AssignMoreText").gameObject.GetComponent<Text>();

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
                miningProgressSlider = hoverPanelInstance.transform.Find("MiningProgress").gameObject.GetComponent<Slider>();
            }

            if (tile.ore != null)
            {
                switch (tile.ore.type)
                {
                    case (OreTypes.Coal):
                        oreTypeText.text = "Mining: Coal";
                        break;
                    case (OreTypes.Copper):
                        oreTypeText.text = "Mining: Copper";
                        break;
                    case (OreTypes.Iron):
                        oreTypeText.text = "Mining: Iron";
                        break;
                    default:
                        Debug.LogError("[Mine] [OnHover] Unexpected ore type: " + tile.ore.type);
                        break;
                }
            }
            else
            {
                Debug.LogError("[Mine] [OnHover] Mine is not on an ore! (tile.ore is null)");
            }

            personOne.gameObject.SetActive(true);
            personTwo.gameObject.SetActive(true);
            personThree.gameObject.SetActive(true);
            personFour.gameObject.SetActive(true);
            personFive.gameObject.SetActive(true);

            assignMoreText.gameObject.SetActive(false);

            hoverPanelInstance.transform.position = new Vector3(Input.mousePosition.x + 115, Input.mousePosition.y - 115);

            switch (occupants.Count)
            {
                case (0):
                    personOne.gameObject.SetActive(false);
                    personTwo.gameObject.SetActive(false);
                    personThree.gameObject.SetActive(false);
                    personFour.gameObject.SetActive(false);
                    personFive.gameObject.SetActive(false);

                    assignMoreText.gameObject.SetActive(true);
                    break;
                case (1):
                    personTwo.gameObject.SetActive(false);
                    personThree.gameObject.SetActive(false);
                    personFour.gameObject.SetActive(false);
                    personFive.gameObject.SetActive(false);
                    break;
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
                    Debug.LogError("[Mine] [OnHover] Unexpected occupant count for hover panel: " + occupants.Count);
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
                        Debug.LogError("[Mine] [OnHover] Unexpected iterator for setting details on hover panel: " + i);
                        break;
                }

                i++;
            }

            powerDrawText.text = "Power Draw: " + powerDraw;
            miningProgressSlider.value = (Time.time - timeSinceLastMine) / (1.0f / miningSpeed);
        }
        else
        {
            Debug.LogError("[Mine] [OnHover] Hover panel is null!");
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
        assignMoreText = null;

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
        miningProgressSlider = null;

        //spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
