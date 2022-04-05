using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphObject : MonoBehaviour
{
    public WorldGraph graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = new WorldGraph();

        WorldLocation A = graph.getLocationObjRef("A");
        WorldLocation E = graph.getLocationObjRef("G");
        var temp = graph.getShortestPath(A, E, 3);

        String s = "";
        if (temp.Item1 != null)
        {
            s += "Time: " + temp.Item2 + " Path: ";
            foreach (var i in temp.Item1)
            {
                s += i.locationName + " ";
            }
        }
        else s = "No Viable Path!";

        print(s);
    }
}
