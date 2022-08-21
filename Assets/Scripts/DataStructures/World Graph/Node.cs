using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string location;
    public double d;
    public Node pred;

    public Node(string location)
    {
        this.location = location;
        d = double.PositiveInfinity;
        pred = null;
    }
}
