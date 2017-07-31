using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour
{
    public static List<Building> buildings = new List<Building>();
    public Sprite sprite;
    private SpriteRenderer spriteRenderer;
    public Tile tile;
    
    // if (housePanel != null) Destroy(housePanel);
    // Use this for initialization
    protected void Start()
    {
        buildings.Add(this);
        //this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -1));
        if (sprite != null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
        int i = 0;

        foreach (Building building in buildings)
        {
            Vector3 pos = GenWorld._instance.getTileCoord(building.transform.position);

            if (buildings.Count <= i + 1) break;

            Debug.Log(buildings[i].transform.position);
            Debug.Log(buildings[i + 1].transform.position);


            DrawLine(buildings[i].transform.position, buildings[i + 1].transform.position, Color.black);// getTileCenter(buildings[i].transform.position), getTileCenter(buildings[i + 1].transform.position));

            i += 1;
        }
    }
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        myLine.transform.SetParent(transform);
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
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

    public virtual void clickMenu(GameObject top, GameObject panel) {

    }
}
