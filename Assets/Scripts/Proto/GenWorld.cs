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
        for (int i = -worldHeight / 2; i < worldHeight / 2; i++)
        {
            for (int j = -worldWidth / 2; j < worldWidth / 2; j++)
            {
                Sprite cSprite = sprites[Random.Range(0, sprites.Length)];

                GameObject cTile = Instantiate(Tile);
                Vector3 cTrans = cTile.transform.position;
                cTrans.x = i * cSprite.bounds.size.x;
                cTrans.y = j * cSprite.bounds.size.y;
                cTile.transform.position = cTrans;
                cTile.GetComponent<SpriteRenderer>().sprite = cSprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
