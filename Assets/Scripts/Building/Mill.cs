using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mill : Building
{

    /// <summary>
    /// Mining speed
    /// amount of ore to mine per second.
    /// </summary>
    float miningSpeed = 5;
    float timeSinceLastMine;
    public int workers = 0;
    public int maxWorkers = 5;

    public new Sprite sprite
    {
        get
        {
            return Resources.Load("Sprites/Building/Mill/1", typeof(Sprite)) as Sprite;
        }
    }

    public GameObject Villager;

    public GameObject hoverPanel;
    private GameObject hoverPanelInstance;

    private Text generatingText;
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
    private Slider millingProgressSlider;

    private List<Villager> occupants = new List<Villager>();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        timeSinceLastMine = Time.time;
        hoverPanel = Resources.Load("Prefabs/UI/MillHoverPanel", typeof(GameObject)) as GameObject;
        if (tile.ore == null) Destroy(gameObject);
    }

    int lastWorkers = 0;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timeSinceLastMine > 1)
        {
            timeSinceLastMine = Time.time;
            if (tile.ore.amount > (int)miningSpeed && workers > 0)
            {
                tile.ore.amount -= (int)miningSpeed;
                GenWorld._instance.Resources[tile.ore.type] += (int)miningSpeed * workers;
            }
        }

        if (workers != lastWorkers)
        {
            lastWorkers = workers;
            occupants = new List<Villager>();
            for (int i = 0; i < workers; i++)
            {
                Villager villager = new Villager();
                villager.role = VillagerRole.Lumberjack;
                occupants.Add(villager);
            }
            displayHeads(occupants);
        }
    }

    public override void register()
    {
        Dictionary<OreTypes, int> required = new Dictionary<OreTypes, int>();
        required.Add(OreTypes.Copper, 6);
        required.Add(OreTypes.Wood, 10);
        BuildingManager._instance.buildings.Add("Mill", new BuildingType("Mill", this, Resources.Load<Sprite>("Sprites/Building/Mill/1"), required, KeyCode.Q, MineType.Mill));
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

                generatingText = hoverPanelInstance.transform.Find("GeneratingText").gameObject.GetComponent<Text>();
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
                millingProgressSlider = hoverPanelInstance.transform.Find("MillingProgress").gameObject.GetComponent<Slider>();
            }

            generatingText.text = (miningSpeed * workers) + " wood per second";

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
                    Debug.LogError("[Mill] [OnHover] Unexpected occupant count for hover panel: " + occupants.Count);
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
                        Debug.LogError("[Mill] [OnHover] Unexpected iterator for setting details on hover panel: " + i);
                        break;
                }

                i++;
            }

            powerDrawText.text = "Power Draw: " + powerDraw;

            if (workers > 0)
                millingProgressSlider.value = (Time.time - timeSinceLastMine) / 1.0f;
            else
                millingProgressSlider.value = 0;
        }
        else
        {
            Debug.LogError("[Mill] [OnHover] Hover panel is null!");
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
        millingProgressSlider = null;

        //spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
    }
}
