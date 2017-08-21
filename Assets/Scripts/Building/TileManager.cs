using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviourSingleton<TileManager>
{
    [HideInInspector]
    public List<Tile> tiles = new List<Tile>();
	
	void Update()
    {
		foreach (Tile tile in tiles)
        {
            tile.UpdateTile();
        }
	}
}
