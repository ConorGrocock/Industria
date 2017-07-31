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

    [Space(20)]
    public int occupancy = 2;
    public int maxCapacity = 5;
    public bool full;

    public Villager[] occupants;

    // Use this for initialization
    protected void Start()
    {
        base.Start();
        GenWorld._instance.houses.Add(this);
        timeToNextBaby = babyTime;
        occupants = new Villager[5];

        occupants[0] = new Villager();
        occupants[0].role = VillagerRole.Default;
        occupants[1] = new Villager();
        occupants[1].role = VillagerRole.Default;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToNextBaby <= 0)
        {
            timeToNextBaby = babyTime;
            babyTime *= 1.5f;
            full = occupancy >= maxCapacity;

            if (full) {
                float xOffset = Random.Range(-2, 2);
                float yOffset = Random.Range(-2, 2);

                Vector3 cPos = GenWorld._instance.getTileCoord(transform.position);

                try {
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)] != null) {
                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null) {
                            xOffset = Random.Range(-2, 2);
                            yOffset = Random.Range(-2, 2);
                        }
                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null) {
                            cPos = GenWorld._instance.getTileCoord(new Vector3((cPos.x + xOffset), (cPos.y + yOffset)));
                            xOffset = Random.Range(-2, 2);
                            yOffset = Random.Range(-2, 2);
                        }

                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().ore != null) {
                            return;
                        }
                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building == null)
                            GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building = GenWorld._instance.buildings["House"];
                    }
                }
                catch (System.IndexOutOfRangeException e) {
                    return;
                }
                //Spawn new house
                //Where?
                //How?
                //Spawn time of new house?
            }
            else {
                occupants[occupants.Length - 1] = new Villager();
                occupants[occupants.Length - 1].role = VillagerRole.Default;
                occupancy++;
            }
        }
        timeToNextBaby -= Time.deltaTime;
    }

    public void register()
    {
        GenWorld._instance.buildings.Add("House", new BuildingType("House", this, Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite));
    }

    public override void clickMenu(GameObject top, GameObject panel) {
        House h = top.GetComponent<House>();
        GameObject[] profiles = new GameObject[h.occupancy];
        for (int i = 0; i < h.occupancy; i++) {
            profiles[i] = Instantiate(Resources.Load<GameObject>("Prefabs/UI/UIPerson"));
            profiles[i].transform.SetParent(panel.transform);
            profiles[i].transform.localPosition = new Vector3(120 + (i * 210), -100, 0);
            profiles[i].GetComponentInChildren<Text>().text = h.occupants[i].Vname;
            profiles[i].GetComponentInChildren<Dropdown>().value = (int)h.occupants[i].role;
        }
    }
}
