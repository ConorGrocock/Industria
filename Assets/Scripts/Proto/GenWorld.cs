using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenWorld : MonoBehaviour {
    public static GenWorld _instance;

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;
    public GameObject Parent;

    public Dictionary<string, BuildingType> buildings;
    public Dictionary<OreTypes, int> Resources;

    public List<House> houses = new List<House>();
    public GameObject buildingPanel;
    Tile BuildTile;
    public Tile buildTile {
        get {
            return BuildTile;
        }
        set {
            BuildTile = value;
            if (value == null) return;
            buildingPanel.SetActive(true);
            Button[] buttons = buildingPanel.GetComponentsInChildren<Button>();
            
            foreach (Button b in buttons) {
                if (value.ore == null) {
                    if (b.name == "Lab") b.interactable = true;
                    else b.interactable = false; 
                    continue;
                }
                if (b.name == "Lumber Mill" && value.ore.mine == MineType.Mill) b.interactable = true;
                else if (b.name == "Mine" && value.ore.mine == MineType.Shaft) b.interactable = true;
                else b.interactable = false;
            }

        }
    }

    public Sprite[] sprites;

    public GameObject[][] tiles;

    private int iron, copper, wood, coal;

    // Use this for initialization
    void Start() {
        if (_instance == null) _instance = this;
        else Debug.LogError("YOU HAVE FUCKED UP. You have more than one World gen class");
        buildingPanel.SetActive(false);

        buildings = new Dictionary<string, BuildingType>();
        registerBuildings();

        Resources = new Dictionary<OreTypes, int>();
        Resources.Add(OreTypes.Coal, 0);
        Resources.Add(OreTypes.Copper, 0);
        Resources.Add(OreTypes.Iron, 0);
        Resources.Add(OreTypes.Wood, 0);
        

        Camera.main.transform.Translate(new Vector3((worldWidth / 2) * 1.28f, ((worldHeight / 2) * 1.28f) - 0.5f));

        tiles = new GameObject[worldWidth][];
        for (int x = 0; x < worldWidth; x++) {
            tiles[x] = new GameObject[worldHeight];
            for (int y = 0; y < worldHeight; y++) {
                Sprite cSprite = sprites[Random.Range(0, sprites.Length - 1)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;

                cTrans.x = x * 1.28f;
                cTrans.y = y * 1.28f;
                cTrans.z = 100;

                cTile.transform.position = cTrans;
                cTile.transform.parent = Parent.transform;

                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;


                cTile.GetComponent<Tile>().tile = Tile;
                tiles[x][y] = cTile;
            }
        }

        for (int ores = 0; ores < Random.Range(4, 10); ores++) {
            GameObject cTile = null;

            while (cTile == null || cTile.GetComponent<Tile>().building != null) {
                cTile = tiles[Random.Range(2, worldWidth - 2)][Random.Range(2, worldHeight - 2)];
            }
            if (coal == 0) {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                coal++;
            }
            else if (copper == 0) {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                copper++;
            }
            else if (iron == 0) {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
                iron++;
            }
            else if (wood == 0) {
                cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                wood++;
            }
            else {
                switch (Random.Range(0, 4)) {
                    case (0): {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Coal, 1000);
                            coal++;
                            break;
                        }
                    case (1): {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Copper, 1000);
                            copper++;
                            break;
                        }
                    case (2): {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Iron, 1000);
                            iron++;
                            break;
                        }
                    case (3): {
                            cTile.GetComponent<Tile>().ore = new Ore(OreTypes.Wood, 1000);
                            wood++;
                            break;
                        }
                }
            }
        }

        tiles[worldWidth / 2][worldHeight / 2].GetComponent<Tile>().building = buildings["House"];
    }

    void Update() {
        int count = 0;
        foreach (House house in houses) {
            count += house.occupancy;
        }
        GameObject.Find("PeopleCount").GetComponent<UnityEngine.UI.Text>().text = "Normal: " + count;

        string resource = "";
        foreach (KeyValuePair<OreTypes, int> entry in Resources) {
            resource += entry.Key.ToString() + ": " + entry.Value + " ";
        }
        GameObject.Find("ResourceCount").GetComponent<UnityEngine.UI.Text>().text = resource;

    }

    void registerBuildings() {
        new House().register();
        new Mine().register();
        new Mill().register();
        new Lab().register();
    }

    public Vector3 getTileCoord(Vector3 vector)
    {
        return vector / 1.28f;
    }

    public void buildOnTile(string building) {
        buildTile.building = buildings[building];
        buildingPanel.SetActive(false);
        BuildTile = null;
    }
}
