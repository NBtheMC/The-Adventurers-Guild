using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapEdge
{
    [HideInInspector] public MapLocation source;
    public MapLocation dest;
    public float timeToTravel;

    public MapEdge(MapLocation s, MapLocation d, float t)
    {
        source = s;
        dest = d;
        timeToTravel = t;
    }
}
