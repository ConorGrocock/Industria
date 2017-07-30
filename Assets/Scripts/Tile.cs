using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
            GameObject init = Instantiate(tile);
            init.GetComponent<SpriteRenderer>().sprite = value.sprite;
            init.AddComponent(value.script.GetType());
            Destroy(init.GetComponent<BoxCollider>());
            init.transform.parent = transform;
            init.transform.localPosition = new Vector3(0, 0, -10);
            Building = value;
        }
    }

    public GameObject tile;
    public bool hover = false;

    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ore != null)
        {
            Debug.Log(Resources.Load<Sprite>("Sprites/Ore/" + ore.type.ToString()));
            spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Ore/" + ore.type.ToString());
        }
    }

    public void OnMouseOver()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        hover = true;
        if (Input.GetMouseButtonDown(0))
        {
            //building = GenWorld._instance.buildings["House"];
            GenWorld._instance.buildTile = this;
        }
    }

    public void OnMouseExit()
    {
        hover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hover || GenWorld._instance.buildTile == this) gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0.5f, 0.7f);
        else gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
