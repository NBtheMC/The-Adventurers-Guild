using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNode
{
    public string location;
    public double d;
    public WorldNode pred;

    public WorldNode(string location)
    {
        this.location = location;
        d = double.PositiveInfinity;
        pred = null;
    }
}
