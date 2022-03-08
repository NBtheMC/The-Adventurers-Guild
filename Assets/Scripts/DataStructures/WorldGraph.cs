using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGraph
{
    private List<List<(WorldNode, ushort)>> edges;
    private List<WorldNode> nodes;

    public IReadOnlyCollection<List<(WorldNode, ushort)>> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyCollection<WorldNode> Nodes { get { return nodes.AsReadOnly(); } }

    public WorldGraph() 
    {
        edges = new List<List<(WorldNode, ushort)>>();
        nodes = new List<WorldNode>();
    }

    private int getIndexFromLocation(string location)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].location == location)
            {
                return i;
            }
        }

        return -1;
    }

    public void addEdge(WorldNode n1, WorldNode n2, ushort weight) 
    {
        if (!nodes.Contains(n1))
            addNode(n1);
        if (!nodes.Contains(n2))
            addNode(n2);

        edges[nodes.IndexOf(n1)].Add((n2, weight));
    }

    public void addEdge(string location1, string location2, ushort weight)
    {
        int n1Index = getIndexFromLocation(location1);
        int n2Index = getIndexFromLocation(location2);

        if (n1Index == -1)
        {
            addNode(location1);
            n1Index = nodes.Count - 1;
        }
        if (n2Index == -1)
        {
            addNode(location2);
            n1Index = nodes.Count - 1;
        }

        edges[n1Index].Add((nodes[n2Index], weight));
    }

    public void addNode(string locationName) 
    {
        WorldNode n = new WorldNode(locationName);
        addNode(n);
    }
    public void addNode(WorldNode n) 
    {
        if (!nodes.Contains(n)) 
        {
            nodes.Add(n);
            edges.Add(new List<(WorldNode, ushort)>());
        }
    }

    public void deleteEdge(WorldNode n1, WorldNode n2)
    {
        if(nodes.Contains(n1) && nodes.Contains(n2))
        {
            foreach (var edge in edges[nodes.IndexOf(n1)])
            {
                if (edge.Item1 == n2)
                    edges[nodes.IndexOf(n1)].Remove(edge);
            }
        } 
    }

    public void deleteEdge(string location1, string location2)
    {
        int n1Index = getIndexFromLocation(location1);
        int n2Index = getIndexFromLocation(location2);

        if (n1Index != -1 && n1Index != -1)
        {
            foreach (var edge in edges[n1Index])
            {
                if (edge.Item1 == nodes[n2Index])
                    edges[n1Index].Remove(edge);
            }
        }
    }

    public void deleteNode(WorldNode n)
    {
        if (nodes.Contains(n))
        {
            nodes.Remove(n);
            edges.RemoveAt(nodes.IndexOf(n));

            for(int i = 0; i < edges.Count; i++)
            {
                deleteEdge(nodes[i], n);
            }
        }
    }
    public void deleteNode(string locationName)
    {
        int index = getIndexFromLocation(locationName);
        if(index != -1)
        {
            deleteNode(nodes[index]);
        }
            
    }
}
