using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MapEdge
{
    [HideInInspector] public MapLocation source;
    public MapLocation dest;
    public float timeToTravel;
}
