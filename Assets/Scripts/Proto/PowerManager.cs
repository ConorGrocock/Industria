using UnityEngine;

public class PowerManager : MonoBehaviourSingleton<PowerManager>
{
    public float powerSupply;

    public float powerStored = 100.0f;

    public float powerDraw
    {
        get
        {
            float p = 0f;
            foreach (Building building in Building.buildings)
            {
                p += building.powerDraw;
            }
            return p;
        }
    }

	void Update()
    {
        if (Manager._instance.isMainMenu || Manager._instance.isPaused) return;

        if (powerStored < 0)
        {
            powerStored = 0;
            Time.timeScale = 0;
            Manager._instance.ShowGameOver();
        }
    }
}
