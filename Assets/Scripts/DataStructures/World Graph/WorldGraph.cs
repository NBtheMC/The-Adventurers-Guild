using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGraph
{
    private List<List<Edge>> edges;
    private List<WorldNode> nodes;
    private PriorityQueue<(List<WorldNode>, uint)> paths;
    private bool isDirected;

    public IReadOnlyCollection<List<Edge>> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyCollection<WorldNode> Nodes { get { return nodes.AsReadOnly(); } }

    public WorldGraph(bool isDirected = true) 
    {
        edges = new List<List<Edge>>();
        nodes = new List<WorldNode>();
        paths = new PriorityQueue<(List<WorldNode>, uint)>();
        this.isDirected = isDirected;
    }

    private Edge getEdge(WorldNode source, WorldNode dest)
    {
        for(int i = 0; i < edges.Count; i++)
        {
            if (edges[nodes.IndexOf(source)][i].dest == dest)
                return edges[nodes.IndexOf(source)][i];
        }
        return null;
    }

    private void Relax(Edge e)
    {
        if(e.dest.d > e.source.d + e.timeToTravel)
        {
            e.dest.d = e.source.d + e.timeToTravel;
            e.dest.pred = e.source;
        }
    }

    public (List<WorldNode>, float) getShortestPath(WorldNode s, WorldNode d, float DC)
    {
        //copy edges into a temp data structure then remove any edges that the
        //party cannot traverse
        List<List<Edge>> tempEdges = new List<List<Edge>>(edges);
        for(int i = 0; i < tempEdges.Count; i++)
        {
            for(int j = 0; j < tempEdges[i].Count; j++)
            {
                if(tempEdges[i][j].difficulty > DC)
                {
                    tempEdges[i].Remove(tempEdges[i][j]);
                    j--;
                }      
            }
        }

        //initialize stuff for Dijkstra SSSP
        s.d = 0;
        PriorityQueue<WorldNode> queue = new PriorityQueue<WorldNode>();
        foreach(var node in nodes)
            queue.Insert(node, node.d);

        //Dijkstra hates UCSC
        while (!queue.IsEmpty())
        {
            WorldNode u = queue.ExtractMin();
            foreach(var edge in tempEdges[nodes.IndexOf(u)])
                Relax(edge);
        }
        
        //return null if there is no path to d
        if (d.pred == null)
            return (null, -1);

        //otherwise get the path from s to d
        List<WorldNode> path = new List<WorldNode>();
        for (WorldNode i = d; i != s && i != null; i = i.pred)
            path.Insert(0, i);

        path.Insert(0, s);

        //get total time taken for journey
        float totalTime = 0f;
        for(int i = 0; i < path.Count - 1; i++)
        {
            Edge e = getEdge(path[i], path[i+ 1]);
            totalTime += e.timeToTravel;
        }

        return (path, totalTime);
    }

    private int getIndexFromLocation(string location)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].location == location)
                return i;
        }
        return -1;
    }

    public void addEdge(WorldNode n1, WorldNode n2, float timeToTravel, uint difficulty) 
    {
        if (!nodes.Contains(n1))
            addNode(n1);
        if (!nodes.Contains(n2))
            addNode(n2);

        edges[nodes.IndexOf(n1)].Add(new Edge(n1, n2, timeToTravel, difficulty));
        if(!isDirected)
            edges[nodes.IndexOf(n2)].Add(new Edge(n2, n1, timeToTravel, difficulty));
    }

    public void addEdge(string location1, string location2, float timeToTravel, uint difficulty)
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
            n2Index = nodes.Count - 1;
        }

        edges[n1Index].Add(new Edge(nodes[n1Index], nodes[n2Index], timeToTravel, difficulty));
        if (!isDirected)
            edges[n2Index].Add(new Edge(nodes[n2Index], nodes[n1Index], timeToTravel, difficulty));
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
            edges.Add(new List<Edge>());
        }
    }

    public void deleteEdge(WorldNode n1, WorldNode n2)
    {
        if(nodes.Contains(n1) && nodes.Contains(n2))
        {
            for(int i = 0; i < edges[nodes.IndexOf(n1)].Count; i++)
            {
                if (edges[nodes.IndexOf(n1)][i].dest == n2)
                {
                    edges.Remove(edges[i]);
                    i--;
                }
                    
            }
            if (!isDirected)
            {
                for (int i = 0; i < edges[nodes.IndexOf(n2)].Count; i++)
                {
                    if (edges[nodes.IndexOf(n2)][i].dest == n1)
                    {
                        edges.Remove(edges[i]);
                        i--;
                    }
                }
            }
        } 
    }

    public void deleteEdge(string location1, string location2)
    {
        int n1Index = getIndexFromLocation(location1);
        int n2Index = getIndexFromLocation(location2);

        if (n1Index != -1 && n1Index != -1)
        {
            WorldNode n1 = nodes[n1Index];
            WorldNode n2 = nodes[n2Index];
            deleteEdge(n1, n2);
        }
    }

    public void deleteNode(WorldNode n)
    {
        if (nodes.Contains(n))
        {
            edges.RemoveAt(nodes.IndexOf(n));
            for (int i = 0; i < edges.Count; i++)
            {
                for(int j = 0; j < edges[i].Count; j++)
                {
                    if (edges[i][j].dest == n)
                    {
                        deleteEdge(edges[i][j].source, edges[i][j].dest);
                        i--;
                    }
                }    
            }
            nodes.Remove(n);
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
