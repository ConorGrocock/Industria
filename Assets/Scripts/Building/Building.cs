using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    public static List<Building> buildings = new List<Building>();
    public Sprite sprite;
    private SpriteRenderer spriteRenderer;
    public Tile tile;

    public float powerLimit = 100f;
    public float powerDraw = 5f;


    // if (housePanel != null) Destroy(housePanel);
    // Use this for initialization
    protected virtual void Start()
    {
        buildings.Add(this);
        //this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -1));
        if (sprite != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
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

                Vector3 pos = GenWorld._instance.getTileCoord(building.transform.position);

                if (buildings.Count <= i + 1) break;

                DrawLine(buildings[i].transform.position, buildings[i + 1].transform.position, Color.black);
                drawnLines = i;
                i += 1;
            }
        }


    }

    private int drawnLines = 0;//What lines have been drawn already?

    private List<Image> displayedHeads = new List<Image>();

    private void setAnchor(Image image)
    {
        Vector2 pos = gameObject.transform.position;
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pos);
        image.GetComponent<RectTransform>().anchorMin = viewportPoint;
        image.GetComponent<RectTransform>().anchorMax = viewportPoint;
    }

    public void displayHeads(List<VillagerRole> roles)
    {
        foreach (Image head in displayedHeads) Destroy(head);
        displayedHeads.Clear();

        foreach (VillagerRole role in roles)
        {
            GameObject go = new GameObject();
            Image image = go.AddComponent<Image>();
            setAnchor(image);
            go.transform.SetParent(GameObject.Find("Canvas").transform);

            displayedHeads.Add(image);
        }
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

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void clickMenu(GameObject top, GameObject panel)
    {

    }

    public virtual void closeMenu()
    {

    }
}
