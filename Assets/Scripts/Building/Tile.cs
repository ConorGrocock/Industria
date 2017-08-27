﻿using System;
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
    BuildingType BuildingType;
    public BuildingType buildingType
    {
        get
        {
            return BuildingType;
        }

        set
        {
            top = Instantiate(tile);
            top.GetComponent<SpriteRenderer>().sprite = value.sprite;
            topBuilding = ((Building)top.AddComponent(value.script.GetType()));
            topBuilding.tile = this;
            if (value.name == "House") if (BuildingManager._instance.houses.Count <= 1) topBuilding.powerDraw = 1;
            Destroy(top.GetComponent<BoxCollider>());
            top.transform.parent = transform;
            top.transform.localPosition = new Vector3(0, 0, -10);
            BuildingType = value;
        }
    }

    public GameObject top;
    public Building topBuilding;

    public GameObject tile;
    public bool hover = false;

    GameObject housePanel;

    private SpriteRenderer spriteRenderer;
    protected Transform canvasTransform;

    // Use this for initialization
    void Start()
    {
        TileManager._instance.tiles.Add(this);

        canvasTransform = GameObject.Find("Canvas").transform;

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
        if (menuClose) { menuClose = false; return; }
        if (IsPointerOverUIObject() || Manager._instance.isGameOver) { mouseOver = false; return; }
        if (buildingType != null) return;
        if (BuildingManager._instance.buildTile != null) { BuildingManager._instance.buildTile = null; return; }
        if (mouseOver && GenWorld._instance.menu == null)
            BuildingManager._instance.buildTile = this;
        else if (BuildingManager._instance.buildTile == this) BuildingManager._instance.buildTile = null;
    }

    private bool menuOpen = false;
    GameObject panel;
    bool mouseOver = false;
    bool menuClose = false;

    public void OnMouseOver()
    {
        BuildingManager._instance.hoverTile = this;

        if (IsPointerOverUIObject() || Manager._instance.isGameOver || Manager._instance.isMainMenu || Manager._instance.isPaused) { OnMouseExit(); return; }
        mouseOver = true;

        if (buildingType != null)
        {
            top.GetComponent<Building>().OnHover();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (GenWorld._instance.menu != null)
            {
                menuClose = true;
                GenWorld._instance.closeMenu();
                return;
            }

            //building = GenWorld._instance.buildings["House"];
            if (buildingType != null)
            {
                top.GetComponent<Building>().clickMenu(top, createMenu()); 
            }
        }

        hover = true;
    }

    public GameObject createMenu()
    {
        BuildingManager._instance.buildTile = null;
        GameObject sprite = Resources.Load<GameObject>("Prefabs/UI/" + buildingType.name + "Panel");

        if (sprite != null)
        {
            panel = Instantiate(sprite);
            panel.transform.SetParent(canvasTransform);
            //panel.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
            panel.transform.localPosition = new Vector3(0, 0, 0);

            menuOpen = true;

            Text[] t = panel.GetComponentsInChildren<Text>();
            foreach (Text text in t) if (text.gameObject.name == "Power") text.text = "POWER";
            return panel;
        }

        return null;
    }

    public void OnMouseExit()
    {
        mouseOver = false;
        hover = false;

        if (buildingType != null)
        {
            top.GetComponent<Building>().OnHoverEnd();
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void UpdateTile()
    {
        if (Manager._instance.isMainMenu) return;

        try
        {
            if ((hover && !IsPointerOverUIObject() || BuildingManager._instance.buildTile == this)) spriteRenderer.color = new Color(0, 0.5f, 0.5f, 0.7f);
            else spriteRenderer.color = Color.white;
        }
        catch (Exception e)
        {
            Debug.LogError("TILE ERROR! (" + e.Message + ") hover: " + hover + ", pointerOverUI: " + IsPointerOverUIObject() + ", GenWorld instance: " + GenWorld._instance);
        }
        if (Input.GetKeyDown(KeyCode.Escape) && menuOpen && panel != null)
        {
            Destroy(panel);
            menuOpen = false;
        }
    }

    public void generateOre() {

        if (!TileManager._instance.oreCounts.ContainsKey(OreTypes.Coal)) {
            ore = new Ore(OreTypes.Coal, 1000);
            buildingType = BuildingManager._instance.buildings["Mine"];
            TileManager._instance.oreCounts.Add(OreTypes.Coal,1);
        }
        else if (!TileManager._instance.oreCounts.ContainsKey(OreTypes.Copper)) {
            ore = new Ore(OreTypes.Copper, 1000);
            TileManager._instance.oreCounts.Add(OreTypes.Copper, 1);
        }
        else if (!TileManager._instance.oreCounts.ContainsKey(OreTypes.Coal)) {
            ore = new Ore(OreTypes.Wood, 1000);
            TileManager._instance.oreCounts.Add(OreTypes.Wood, 1);
        }

        if (ore == null) {
            int orePercent = UnityEngine.Random.Range(0, 100);
            if (orePercent > 50) {
                ore = new Ore(OreTypes.Coal, 1000);
            }
            else if (orePercent > 30) {
                ore = new Ore(OreTypes.Copper, 1000);
            }
            else if (orePercent > 00) {
                ore = new Ore(OreTypes.Wood, 1000);
            }
        }
        return;
    }
}
