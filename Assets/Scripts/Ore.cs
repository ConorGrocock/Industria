using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum OreTypes {
    Coal,
    Iron,
    Copper,
    Wood
}

public enum MineType {
    None,
    Shaft,
    Mill
}

public class Ore {
    public OreTypes type;
    public MineType mine;
    public int amount;

    public Ore(OreTypes ore, int amount) {
        this.type = ore;
        this.amount = amount;

        switch (ore) {
            case OreTypes.Coal: {
                    mine = MineType.Shaft;
                    break;
                }
            case OreTypes.Iron: {
                    mine = MineType.Shaft;
                    break;
                }
            case OreTypes.Copper: {
                    mine = MineType.Shaft;
                    break;
                }
            case OreTypes.Wood: {
                    mine = MineType.Mill;
                    break;
                }
        }
    }
}