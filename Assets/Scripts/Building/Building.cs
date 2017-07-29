using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour {

    public Sprite sprite;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	protected void Start () {
        this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -1));
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
