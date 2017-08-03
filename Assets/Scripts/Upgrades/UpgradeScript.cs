using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeScript : MonoBehaviour
{
    //[HideInInspector]
    public bool unlocked = false;

    private bool appliedEffect = false;

    public abstract void ApplyUpgradeEffect();
    public abstract void OnUpgradeUpdate();

    void Start()
    {
        Upgrades._instance.RegisterUpgrade(this);
    }

	public void OnUpdate()
    {
        if (unlocked && !appliedEffect)
        {
            ApplyUpgradeEffect();
            appliedEffect = true;
        }
        else if (unlocked && appliedEffect)
        {
            OnUpgradeUpdate();
        }
	}
}
