using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    public static List<Building> buildings = new List<Building>();
    public Sprite sprite;
    protected SpriteRenderer spriteRenderer;
    public Tile tile;

    public float powerLimit = 100f;
    public float powerDraw = 5f;

    protected Transform canvasTransform;

    private Sprite warningSprite;

    private Vector3 warningSpriteScale = new Vector3(0.1f, 0.1f);
    private Vector3 warningSpritePosition = new Vector3(0.55f, 0.55f, -1.0f);

    protected float w_SecondsPerFlash = 0.5f;
    private float w_TimeSinceLastFlash;
    private bool w_Flash;

    private bool warningDoFlash;
    private bool showingWarning;

    private GameObject warningSpriteInstance;

    // if (housePanel != null) Destroy(housePanel);
    // Use this for initialization
    protected virtual void Start()
    {
        buildings.Add(this);

        warningSprite = Resources.Load<Sprite>("Sprites/UI/warning");

        canvasTransform = GameObject.Find("Canvas").transform;

        //this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -1));
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
        int i = 0;

        bool plantExists = false;

        foreach (Building building in buildings)
            if (building.tile.building.name == "PowerPlant")
                plantExists = true;

        if (plantExists)
        {
            foreach (Building building in buildings)
            {
                if (drawnLines > i)
                {
                    i++;
                    continue;
                }

                Vector3 pos = UtilityManager._instance.getTileCoord(building.transform.position);

                if (buildings.Count <= i + 1) break;

                DrawLine(buildings[i].transform.position, buildings[i + 1].transform.position, Color.black);
                drawnLines = i;
                i += 1;
            }
        }
        displayedHeads = new List<GameObject>();
        drawnLines = 0;
    }

    protected virtual void Update()
    {
        if (warningDoFlash && showingWarning)
        {
            if (Time.time - w_TimeSinceLastFlash >= w_SecondsPerFlash)
            {
                w_TimeSinceLastFlash = Time.time;
                w_Flash = !w_Flash;

                if (w_Flash)
                {
                    warningSpriteInstance.SetActive(true);
                }
                else
                {
                    warningSpriteInstance.SetActive(false);
                }
            }
        }
    }

    private int drawnLines;//What lines have been drawn already?

    private List<GameObject> displayedHeads;

    private void setAnchor(Image image)
    {
        Vector2 pos = gameObject.transform.position;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);
        image.GetComponent<RectTransform>().anchorMin = viewportPoint;
        image.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }

    public void displayHeads(List<Villager> villagers)
    {
        foreach (GameObject head in displayedHeads) Destroy(head);
        displayedHeads.Clear();
        float x = 0;
        float y = 0;
        foreach (Villager villager in villagers)
        {
            GameObject go = new GameObject();
            SpriteRenderer image = go.AddComponent<SpriteRenderer>();
            //setAnchor(image);
            go.transform.SetParent(transform);

            //RectTransform rt = go.GetComponent<RectTransform>();

            go.transform.localScale = new Vector3(1.5f, 1.5f);

            image.sprite = villager.getHead();
            image.transform.localPosition = new Vector3(-0.5f + x, -0.5f + y, -1);
            x += image.sprite.bounds.size.x + 0.15f;

            if (x > 0.9)
            {
                x = 0;
                y += image.sprite.bounds.size.y + 0.15f;
            }

            displayedHeads.Add(go);
        }
    }

    public void showWarningSign(bool flash)
    {
        if (showingWarning) return;

        warningDoFlash = flash;

        if (warningSpriteInstance == null)
        {
            warningSpriteInstance = new GameObject();
            SpriteRenderer image = warningSpriteInstance.AddComponent<SpriteRenderer>();
            warningSpriteInstance.transform.SetParent(transform);

            warningSpriteInstance.transform.localScale = warningSpriteScale;

            image.sprite = warningSprite;
            image.transform.localPosition = warningSpritePosition;
        }
        else
        {
            warningSpriteInstance.SetActive(true);   
        }

        showingWarning = true;
    }

    public void hideWarningSign()
    {
        if (!showingWarning) return;

        if (warningSpriteInstance != null)
        {
            warningSpriteInstance.SetActive(false);
        }
        else
        {
            Debug.LogError("[Building] [hideWarningSign] Warning sprite instance is null!");
        }

        showingWarning = false;
        warningDoFlash = false;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        return;
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        myLine.transform.SetParent(transform);
        //lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.025f;
        lr.endWidth = 0.025f;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    Vector3 getTileCenter(Vector3 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        float centerX = x + (1.28f / 2f);
        float centerY = y + (1.28f / 2f);

        return new Vector3(centerX, centerY) * 1.28f;
    }

    public virtual void register()
    {
    }

    public virtual void clickMenu(GameObject top, GameObject panel)
    {
    }

    public virtual void closeMenu()
    {
    }

    public virtual void OnHover()
    {
    }

    public virtual void OnHoverEnd()
    {
    }
}