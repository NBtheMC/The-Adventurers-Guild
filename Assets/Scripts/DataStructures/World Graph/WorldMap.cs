using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    private List<List<MapEdge>> edges;
    private Dictionary<string, MapLocation> nodes;
    private Dictionary<string, List<MapEdge>> edgesMap;
    private MapLocation guildNode;
    public TextAsset LocationsCSV;

    public IReadOnlyCollection<List<MapEdge>> Edges { get { return edges.AsReadOnly(); } }
    public IReadOnlyDictionary<string, MapLocation> Nodes { get { return (IReadOnlyDictionary<string, MapLocation>)nodes; } }

    public WorldMap()
    {
        edges = new List<List<MapEdge>>();
        nodes = new Dictionary<string, MapLocation>();
        edgesMap = new Dictionary<string, List<MapEdge>>();

        LocationsCSV = Resources.Load<TextAsset>("Locations");
        string locationsText = LocationsCSV.text;
        string[] lines = locationsText.Split('\n');
        foreach(string line in lines)
        {
            string[] parts = line.Split(',');
            MapLocation l1 = getLocationObjRef(parts[0]);
            if (l1 == null)
            {
                Console.WriteLine("Adding " + parts[0]);
                l1 = new MapLocation(parts[0]);
                nodes.Add(parts[0], l1);
                edgesMap.Add(parts[0], new List<MapEdge>());
            }  

            MapLocation l2 = getLocationObjRef(parts[1]);
            if (l2 == null)
            {
                Console.WriteLine("Adding " + parts[1]);
                l2 = new MapLocation(parts[1]);
                nodes.Add(parts[1], l2);
                edgesMap.Add(parts[1], new List<MapEdge>());
            }

            edgesMap[parts[0]].Add(new MapEdge(l1, l2, int.Parse(parts[2])));
            edgesMap[parts[1]].Add(new MapEdge(l2, l1, int.Parse(parts[2])));


        }
        guildNode = nodes["Guild"];
    }

    public MapLocation getLocationObjRef(string name)
    {
        MapLocation l;
        nodes.TryGetValue(name, out l);
        return l;
    }

    private MapEdge getEdge(MapLocation source, MapLocation dest)
    {
        string key = source.locationName;
        for (int i = 0; i < edgesMap[key].Count; i++)
        {
            if (edgesMap[key][i].dest == dest)
                return edgesMap[key][i];
        }
        return null;
    }

    public (List<MapLocation>, float) getShortestPathFromGuild(MapLocation d) 
    {
        return getShortestPath(guildNode, d);
    }

    public (List<MapLocation>, float) getShortestPath(MapLocation s, MapLocation d)
    {
        
        //initialize stuff for Dijkstra SSSP
        PriorityQueue queue = new PriorityQueue();
        var locations = nodes.Values;

        foreach (var location in locations)
        {
            if (location == s)
                location.d = 0;
            else
                location.d = double.PositiveInfinity;

            location.pred = null;
            location.visited = false;
            queue.Insert(location, location.d);
        }

        //Dijkstra hates UCSC
        while (!queue.IsEmpty())
        {
            MapLocation u = queue.ExtractMin();
            //stop search if we hit the destination
            if (u == d)
                break;

            foreach (var edge in edgesMap[u.locationName])
            {
                if (!edge.dest.visited)
                {
                    double tempDist = u.d + edge.timeToTravel;
                    if(tempDist < edge.dest.d)
                    {
                        edge.dest.d = tempDist;
                        edge.dest.pred = u;
                        queue.DecreaseKey(edge.dest, edge.dest.d);
                    }
                }
            }
        }

        //return null if there is no path to d
        if (d.pred == null)
            return (null, -1);

        //otherwise get the path from s to d
        List<MapLocation> path = new List<MapLocation>();
        for (MapLocation i = d; i != null; i = i.pred)
        {
            path.Insert(0, i);

        }

        //get total time taken for journey
        float totalTime = 0f;
        for (int i = 0; i < path.Count - 1; i++)
        {
            MapEdge e = getEdge(path[i], path[i + 1]);
            totalTime += e.timeToTravel;
        }

        return (path, totalTime);
    }
}
