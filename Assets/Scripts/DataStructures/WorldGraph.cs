using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGraph
{
    private List<(WorldNode, int)> edges;
    private List<WorldNode> nodes;

    public IReadOnlyCollection<(WorldNode, int)> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyCollection<WorldNode> Nodes { get { return nodes.AsReadOnly(); } }

    public WorldGraph() 
    {
        edges = new List<(WorldNode, int)>();
        nodes = new List<WorldNode>();
    }

    public void addEdge(WorldNode n1, WorldNode n2, int weight) 
    {
        edges.Insert(n1.id, (n2, weight));
    }

    public void addNode(string locationName) 
    {
        WorldNode n = new WorldNode(locationName);
        if(nodes.Contains(n))
            nodes.Insert(n.id, n);
        else nodes.Add(n);
    }
    public void addNode(WorldNode n) 
    {
        nodes.Insert(n.id, n);
    }
}
