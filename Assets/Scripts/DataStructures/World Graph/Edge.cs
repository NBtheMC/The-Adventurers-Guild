using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public WorldNode source;
    public WorldNode dest;
    public float timeToTravel;
    public uint difficulty;

    public Edge(WorldNode n1, WorldNode n2, float time, uint diff)
    {
        source = n1;
        dest = n2;
        timeToTravel = time;
        difficulty = diff;
    }
}
