using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public WorldMap graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = new WorldMap();
        FindPath("Farm", "Village");
       
    }

    public void FindPath(string src, string dest)
    {
        MapLocation s = graph.getLocationObjRef(src);
        MapLocation d = graph.getLocationObjRef(dest);

        var temp = graph.getShortestPath(s, d);

        String str = "";
        if (temp.Item1 != null)
        {
            str += "Time: " + temp.Item2 + " Path: ";
            foreach (var i in temp.Item1)
            {
                str += i.locationName + " ";
            }
        }
        else str = "No Viable Path!";
        print(str);

    }
}
