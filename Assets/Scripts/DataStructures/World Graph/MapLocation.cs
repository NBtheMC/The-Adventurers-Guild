using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLocation
{
    public string locationName;
    public double d;
    public MapLocation pred;
    public bool visited;

    public MapLocation(string name)
    {
        locationName = name;
        d = double.PositiveInfinity;
        pred = null;
        visited = false;
    }
}
