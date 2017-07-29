using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Villager : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public Sprite sprite;

	// Use this for initialization
	void Start () {
        this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -2));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
