using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building {

    public float babyTime = 4f;
    [SerializeField]
    private float timeToNextBaby = 0f;

    [Space(20)]
    public int occupancy;
    public int maxCapacity = 5;
    public bool full;

    public GameObject Villager;

    // Use this for initialization
    new void Start () {
        base.Start();
        timeToNextBaby = babyTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeToNextBaby <= 0) {
            timeToNextBaby = babyTime;
            babyTime *= 2;
            occupancy++;
            full = occupancy >= maxCapacity;

            if(full) {
                float xOffset = Random.Range(-1, 1);
                float yOffset = Random.Range(-1, 1);

                Vector3 cPos = GenWorld._instance.getTileCoord(transform.position);

                GenWorld._instance.tiles[(int)(cPos.x + xOffset)][(int)(cPos.y + yOffset)].GetComponent<Tile>().building = GenWorld._instance.house;

                //Spawn new house
                //Where?
                //How?
                //Spawn time of new house?
            }
        }
        timeToNextBaby -= Time.deltaTime;
	}
}
