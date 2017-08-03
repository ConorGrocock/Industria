using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VillagerRole {
    None,
    Miner,
    Lumberjack
}

[RequireComponent(typeof(SpriteRenderer))]
public class Villager {

    private SpriteRenderer spriteRenderer;
    public Sprite sprite;

    public string Vname;

    public VillagerRole role;

    public Villager() {
        this.Vname = Villager.randomName();
        Debug.Log(Vname);
    }

	// Use this for initialization
	void Start () {
        //this.transform.Translate(new Vector3(this.transform.position.x, this.transform.position.y, -2));
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.sprite = sprite;
    }
	
	// Update is called once per frame
	void Update () {
        //otransform.Translate(new Vector3(Random.Range(-1, 2) * Time.deltaTime, Random.Range(-1, 2) * Time.deltaTime));
	}

    public static string randomName() {
        return names[Random.Range(0,names.Length-1)];
    }

    static string[] names = new string[]{
            "OLIVER ",
"JACK ",
"HARRY ",
"GEORGE ",
"JACOB ",
"CHARLIE ",
"NOAH ",
"WILLIAM ",
"THOMAS ",
"OSCAR ",
"JAMES ",
"MUHAMMAD ",
"HENRY ",
"ALFIE ",
"LEO ",
"JOSHUA ",
"FREDDIE ",
"ETHAN ",
"ARCHIE ",
"ISAAC ",
"JOSEPH ",
"ALEXANDER ",
"SAMUEL ",
"DANIEL ",
"LOGAN ",
"EDWARD ",
"LUCAS ",
"MAX ",
"MOHAMMED ",
"BENJAMIN ",
"MASON ",
"HARRISON ",
"THEO ",
"JAKE ",
"SEBASTIAN ",
"FINLEY ",
"ARTHUR ",
"ADAM ",
"DYLAN ",
"RILEY ",
"ZACHARY ",
"TEDDY ",
"DAVID ",
"TOBY ",
"THEODORE ",
"ELIJAH ",
"MATTHEW ",
"JENSON ",
"JAYDEN ",
"HARVEY ",

        };

}
