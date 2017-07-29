using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building {

    public float babyTime = 60f;
    [SerializeField]
    private float timeToNextBaby = 0f;

    [Space(20)]
    public int capacity;
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
            capacity++;
            Instantiate(Villager, position: transform.position, rotation: Quaternion.identity);
        }
        timeToNextBaby -= Time.deltaTime;
	}
}
