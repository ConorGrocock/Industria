using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Ore ore;
    /// <summary>
    /// Building on this tile
    /// </summary>
    BuildingType Building;
    public BuildingType building
    {
        get
        {
            return Building;
        }

        set
        {
            top = Instantiate(tile);
            top.GetComponent<SpriteRenderer>().sprite = value.sprite;
            ((Building)top.AddComponent(value.script.GetType())).tile = this;
            Destroy(top.GetComponent<BoxCollider>());
            top.transform.parent = transform;
            top.transform.localPosition = new Vector3(0, 0, -10);
            Building = value;
        }
    }
    public GameObject top;

    public GameObject tile;
    public bool hover = false;

    GameObject housePanel;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ore != null)
        {
            Sprite oreResource = Resources.Load<Sprite>("Sprites/Ore/" + ore.type.ToString());
            if (ore.type != OreTypes.Wood)
            {
                spriteRenderer.sprite = oreResource;
            }
            else
            {
                //GameObject fB = Instantiate(new GameObject("Forest_Background"));
                //fB.transform.position = this.transform.position;
                //fB.transform.Translate(new Vector3(0, 0, 1));
                //fB.AddComponent<SpriteRenderer>().sprite = spriteRenderer.sprite;
                //spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Ore/" + ore.type.ToString());

                GameObject fB = new GameObject("Tree");
                fB.transform.position = this.transform.position;
                fB.transform.Translate(new Vector3(0, 0, -1));
                fB.AddComponent<SpriteRenderer>().sprite = oreResource;
                fB.transform.SetParent(transform);
            }
        }
    }

    public void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject() || GenWorld._instance.gameOver) { mouseOver = false; return; }
        if (GenWorld._instance.buildTile != null) { GenWorld._instance.buildTile = null; return; }
        if (mouseOver)
            GenWorld._instance.buildTile = this;
        else if (GenWorld._instance.buildTile == this) GenWorld._instance.buildTile = null;
    }

    private bool menuOpen = false;
    GameObject panel;
    bool mouseOver = false;
    public void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject() || GenWorld._instance.gameOver) { mouseOver = false; return; }
        mouseOver = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (GenWorld.menu != null)
            {
                GenWorld._instance.closeMenu();
                return;
            }

            //building = GenWorld._instance.buildings["House"];
            if (building != null)
            {
                GameObject sprite = Resources.Load<GameObject>("Prefabs/UI/" + building.name + "Panel");
                if (sprite != null)
                {
                    panel = Instantiate(sprite);
                    panel.transform.SetParent(GameObject.Find("Canvas").transform);
                    //panel.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
                    panel.transform.localPosition = new Vector3(0, 0, 0);

                    menuOpen = true;

                    Text[] t = panel.GetComponentsInChildren<Text>();
                    foreach (Text text in t)
                    {
                        if (text.gameObject.name == "Power") text.text = "POWER";
                        top.GetComponent<Building>().clickMenu(top, panel);
                    }
                }
            }
        }
        hover = true;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
        hover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if ((hover && !EventSystem.current.IsPointerOverGameObject() || GenWorld._instance.buildTile == this)) gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0.5f, 0.7f);
        else gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen && panel != null)
        {
            Destroy(panel);
            menuOpen = false;
        }
    }
}
