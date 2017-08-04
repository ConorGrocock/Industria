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

    public string Vname;

    public VillagerRole role;

    public Villager(VillagerRole role = VillagerRole.None) {
        this.Vname = Villager.randomName();
        if (GenWorld._instance.maxMineWorkers > GenWorld._instance.totalMiners)
            this.role = VillagerRole.Miner;
        else if (GenWorld._instance.maxMillWorkers > GenWorld._instance.totalJacks)
            this.role = VillagerRole.Lumberjack;
        else this.role = (VillagerRole)Random.Range(0, 3);
    }

    public Sprite getSprite()
    {
        switch (role)
        {
            case VillagerRole.None:
                return Resources.Load<Sprite>("Sprites/Villager/3");
            case VillagerRole.Miner:
                return Resources.Load<Sprite>("Sprites/Villager/1");
            case VillagerRole.Lumberjack:
                return Resources.Load<Sprite>("Sprites/Villager/4");
            default:
                return null;
        }
    }

    public Sprite getHead()
    {
        switch (role)
        {
            case VillagerRole.None:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/3");
            case VillagerRole.Miner:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/1");
            case VillagerRole.Lumberjack:
                return Resources.Load<Sprite>("Sprites/Villager/Heads/4");
            default:
                return null;
        }
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
