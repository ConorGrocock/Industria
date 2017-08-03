using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeScript : MonoBehaviour
{
    public bool unlocked = false;

    public abstract void ApplyUpgradeEffect();

    void Start()
    {
        Upgrades._instance.RegisterUpgrade(this);
    }

	public void OnUpdate()
    {
        if (unlocked)
            ApplyUpgradeEffect();
	}
}
