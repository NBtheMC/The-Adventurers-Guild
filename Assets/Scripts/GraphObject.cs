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
        WorldNode a = new WorldNode("A");
        WorldNode b = new WorldNode("B");
        WorldNode c = new WorldNode("C");
        WorldNode d = new WorldNode("D");
        WorldNode e = new WorldNode("E");
        WorldNode f = new WorldNode("F");
        WorldNode g = new WorldNode("G");
        WorldNode h = new WorldNode("H");

        graph.addEdge(a, b, 3, 1);
        graph.addEdge(a, c, 2, 3);
        graph.addEdge(b, c, 2, 1);
        graph.addEdge(c, d, 3, 1);
        graph.addEdge(c, e, 1, 8);
        graph.addEdge(d, f, 5, 1);
        graph.addEdge(f, g, 3, 2);
        graph.addEdge(g, h, 1, 1);
        graph.addEdge(d, e, 3, 1);

        Text t = this.GetComponent<Text>();
        var temp = graph.getShortestPath(a, e, 10);
        String s = "";
        if (temp.Item1 != null)
        {
            for (int i = 0; i < temp.Item1.Count; i++)
            {
                s += temp.Item1[i].location + " ";
            }
            s += temp.Item2;
        }
        else s = "No Viable Path!"; 

        print(s);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
