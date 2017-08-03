using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviourSingleton<Upgrades> {
    [HideInInspector]
    public List<UpgradeScript> upgrades;

    void Awake()
    {
        upgrades = new List<UpgradeScript>();
    }
	
	void Update()
    {
		if (upgrades != null)
        {
            foreach (UpgradeScript uScript in upgrades)
            {
                uScript.OnUpdate();
            }
        }
	}

    public void RegisterUpgrade(UpgradeScript uScript)
    {
        upgrades.Add(uScript);
    }
}
