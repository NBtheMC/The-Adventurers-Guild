using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public Node source;
    public Node dest;
    public float timeToTravel;
    public uint difficulty;

    public Edge(Node n1, Node n2, float time, uint diff)
    {
        source = n1;
        dest = n2;
        timeToTravel = time;
        difficulty = diff;
    }
}
