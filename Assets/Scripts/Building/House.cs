using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building {

    public float babyTime = 0f;
    [SerializeField]
    private float timeToNextBaby = 0f;

    public new Sprite sprite {
        get {
            return Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite;
        }
    }

    [Space(20)]
    public int occupancy;
    public int maxCapacity = 5;
    public bool full;

    public GameObject Villager;
    
    // Use this for initialization
    protected void Start (){
        timeToNextBaby = babyTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeToNextBaby <= 0) {
            timeToNextBaby = babyTime;
            babyTime *= 0f;
            full = occupancy >= maxCapacity;

            if(full) {
                float xOffset = Random.Range(-2, 2);
                float yOffset = Random.Range(-2, 2);

                Vector3 cPos = GenWorld._instance.getTileCoord(transform.position);

                try {
                    if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)] != null) {
                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building != null) {
                            xOffset = Random.Range(-2, 2);
                            yOffset = Random.Range(-2, 2);
                        }

                        if (GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building == null)
                            GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building = GenWorld._instance.buildings["House"];
                    }
                } catch (System.IndexOutOfRangeException e) {
                    return;
                }
                //Spawn new house
                //Where?
                //How?
                //Spawn time of new house?
            } else occupancy++;
        }
        timeToNextBaby -= Time.deltaTime;
	}

    public void register() {
        Debug.Log((Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite).bounds);
        GenWorld._instance.buildings.Add("House", new BuildingType(this, Resources.Load("Sprites/Building/House/1", typeof(Sprite)) as Sprite));
    }
}
