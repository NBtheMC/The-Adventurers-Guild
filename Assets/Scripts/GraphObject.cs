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
        WorldLocation[] locations;
        locations = Resources.LoadAll<WorldLocation>("Locations");

        graph = new WorldGraph();

        Text t = this.GetComponent<Text>();
        print(locations[0].locationName + "  " + locations[4].locationName);
        var temp = graph.getShortestPath(locations[0], locations[4], 8);
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

    // Update is called once per frame
    void Update()
    {

    }
}
