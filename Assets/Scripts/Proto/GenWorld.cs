using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenWorld : MonoBehaviour
{

    public int worldHeight = 10;
    public int worldWidth = 10;
    public GameObject Tile;

    public Sprite[] sprites;

    // Use this for initialization
    void Start()
    {
        for (float y = (-(float)worldHeight / 2f); y < ((float)worldHeight / 2f) + 1; y++)
        {
            for (float x = (-(float)worldWidth / 2f); x < (float)worldWidth / 2f; x++)
            {
                Sprite cSprite = sprites[Random.Range(0, sprites.Length)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;
                cTrans.x = x * cSprite.bounds.size.x;
                cTrans.y = y * cSprite.bounds.size.y;
                cTile.transform.position = cTrans;
                cTile.AddComponent<Tile>();
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
