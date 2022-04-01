using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGraph
{
    private List<List<GraphEdge>> edges;
    private List<WorldLocation> nodes;

    public IReadOnlyCollection<List<GraphEdge>> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyCollection<WorldLocation> Nodes { get { return nodes.AsReadOnly(); } }

    public WorldGraph() 
    {
        edges = new List<List<GraphEdge>>();
        nodes = new List<WorldLocation>();

        WorldLocation[] locations;
        locations = Resources.LoadAll<WorldLocation>("Locations");

        foreach(var location in locations)
        {
            foreach (var edge in location.connections)
            {
                edge.source = location;
            }
            edges.Add(location.connections);
            nodes.Add(location);
        }
    }

    private GraphEdge getEdge(WorldLocation source, WorldLocation dest)
    {
        for(int i = 0; i < edges.Count; i++)
        {
            if (edges[nodes.IndexOf(source)][i].dest == dest)
                return edges[nodes.IndexOf(source)][i];
        }
        return null;
    }

    private void Relax(GraphEdge e)
    {
        if(e.dest.d > e.source.d + e.timeToTravel)
        {
            e.dest.d = e.source.d + e.timeToTravel;
            e.dest.pred = e.source;
        }
    }

    public (List<WorldLocation>, float) getShortestPath(WorldLocation s, WorldLocation d, float DC)
    {
        //initialize stuff for Dijkstra SSSP
        foreach(var location in nodes)
        {
            if (location == s)
                location.d = 0;
            else
                location.d = double.PositiveInfinity;

            location.pred = null;
        }

        PriorityQueue queue = new PriorityQueue();
        queue.Insert(s, s.d);

        //Dijkstra hates UCSC
        while (!queue.IsEmpty())
        {
            WorldLocation u = queue.ExtractMin();
            //stop search if we hit the destination
            if (u == d)
                break;

            foreach(var edge in edges[nodes.IndexOf(u)])
            {
                //ignore edges that party cannot traverse
                if(edge.difficulty <= DC)
                {
                    Relax(edge);
                    if (queue.Contains(edge.dest))
                    {
                        queue.DecreaseKey(edge.dest, edge.dest.d);
                    }
                    else
                        queue.Insert(edge.dest, edge.dest.d);
                }   
            }     
        }
        
        //return null if there is no path to d
        if (d.pred == null)
            return (null, -1);

        //otherwise get the path from s to d
        List<WorldLocation> path = new List<WorldLocation>();
        for (WorldLocation i = d; i != null; i = i.pred)
        {
            path.Insert(0, i);

        }
            
        //get total time taken for journey
        float totalTime = 0f;
        for(int i = 0; i < path.Count - 1; i++)
        {
            GraphEdge e = getEdge(path[i], path[i+ 1]);
            totalTime += e.timeToTravel;
        }

        return (path, totalTime);
    }
}
