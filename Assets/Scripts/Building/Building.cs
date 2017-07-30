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

    // Use this for initialization
    protected void Start()
    {
        buildings.Add(this);
        this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -1));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        int i = 0;

        foreach (Building building in buildings)
        {
            Vector3 pos = GenWorld._instance.getTileCoord(building.transform.position);

            if (buildings.Count < i + 1) break;

            Gizmos.color = Color.gray;

            Gizmos.DrawLine(buildings[i].transform.position, buildings[i + 1].transform.position);// getTileCenter(buildings[i].transform.position), getTileCenter(buildings[i + 1].transform.position));

            i += 1;
        }
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
}
