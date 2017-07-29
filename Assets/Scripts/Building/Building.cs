using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Building : MonoBehaviour {

    public Sprite sprite;

    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	protected void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
