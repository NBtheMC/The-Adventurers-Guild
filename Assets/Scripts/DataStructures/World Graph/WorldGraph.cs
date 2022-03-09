using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGraph
{
    private List<List<Edge>> edges;
    private List<WorldNode> nodes;
    private PriorityQueue<(List<WorldNode>, uint)> paths;

    public IReadOnlyCollection<List<Edge>> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyCollection<WorldNode> Nodes { get { return nodes.AsReadOnly(); } }

    public WorldGraph() 
    {
        edges = new List<List<Edge>>();
        nodes = new List<WorldNode>();
        paths = new PriorityQueue<(List<WorldNode>, uint)>();
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

    public (List<WorldNode>, float) getShortestPath(WorldNode s, WorldNode d, float DC)
    {
        paths.Clear();
        bool[] isVisited = new bool[nodes.Count];
        List<WorldNode> pathList = new List<WorldNode>();

        pathList.Add(s);

        findAllPaths(s, d, isVisited, pathList, 0, 0);

        while (!paths.IsEmpty())
        {
            if (paths.Peak().Item2 < DC)
                return (paths.Peak().Item1, paths.contents[0].Item2);
            else
                paths.ExtractMin();
        }

        return (null, 0);
    }

    private void findAllPaths(WorldNode u, WorldNode d, bool[] isVisited, List<WorldNode> localPathList, float totalTime, uint maxDifficulty)
    {
        if (u.Equals(d))
        {
            List<WorldNode> temp = new List<WorldNode>();
            foreach(WorldNode node in localPathList)
            {
                temp.Add(node);
            }
            paths.Insert((temp, maxDifficulty), totalTime);
            return;
        }

        int sourceIndex = nodes.IndexOf(u);
        isVisited[sourceIndex] = true;

        foreach (Edge e in edges[nodes.IndexOf(u)])
        {
            int destIndex = nodes.IndexOf(e.dest);
            if (!isVisited[destIndex])
            {
                localPathList.Add(e.dest);
                findAllPaths(e.dest, d, isVisited, localPathList, totalTime + e.timeToTravel, Math.Max(maxDifficulty, e.difficulty));

                localPathList.Remove(e.dest);
            }
        }

        isVisited[sourceIndex] = false;
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

    public void addEdge(WorldNode n1, WorldNode n2, float timeToTravel, uint difficulty) 
    {
        if (!nodes.Contains(n1))
            addNode(n1);
        if (!nodes.Contains(n2))
            addNode(n2);

        edges[nodes.IndexOf(n1)].Add(new Edge(n1, n2, timeToTravel, difficulty));
        // edges[nodes.IndexOf(n1)].Add(new Edge(n2, n1, timeToTravel, difficulty));
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
        //edges.Add(new Edge(nodes[n2Index], nodes[n1Index], timeToTravel, difficulty));
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
                    edges.Remove(edges[i]);
            }
            //for (int i = 0; i < edges[nodes.IndexOf(n2)].Count; i++)
            //{
            //    if (edges[nodes.IndexOf(n2)][i].dest == n1)
            //        edges.Remove(edges[i]);
            //}
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
