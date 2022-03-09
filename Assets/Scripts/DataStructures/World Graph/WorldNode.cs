using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode
{
    public string location;
    public PartySheet party;
    public double d;
    public WorldNode predecessor;

    public WorldNode(string location)
    {
        this.location = location;
        this.party = null;
        d = double.PositiveInfinity;
        predecessor = null;
    }
}
