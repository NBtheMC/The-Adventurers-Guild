using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap
{
    private Dictionary<string, MapLocation> nodes;
    private Dictionary<string, List<MapEdge>> edges;
    private MapLocation guildNode;
    public TextAsset LocationsCSV;

    private List<List<MapLocation>> allPaths;

    public IReadOnlyDictionary<string, List<MapEdge>> Edges { get { return Edges; } }
    public IReadOnlyDictionary<string, MapLocation> Nodes { get { return nodes; } }

    public WorldMap()
    {
        nodes = new Dictionary<string, MapLocation>();
        edges = new Dictionary<string, List<MapEdge>>();

        LocationsCSV = Resources.Load<TextAsset>("Locations");
        string locationsText = LocationsCSV.text;
        string[] lines = locationsText.Split('\n');
        foreach(string line in lines)
        {
            string[] parts = line.Split(',');
            MapLocation l1 = getLocationObjRef(parts[0]);
            if (l1 == null)
            {
                Console.WriteLine($"Adding {parts[0]}");
                l1 = new MapLocation(parts[0]);
                nodes.Add(parts[0], l1);
                edges.Add(parts[0], new List<MapEdge>());
            }  

            MapLocation l2 = getLocationObjRef(parts[1]);
            if (l2 == null)
            {
                Console.WriteLine($"Adding {parts[1]}");
                l2 = new MapLocation(parts[1]);
                nodes.Add(parts[1], l2);
                edges.Add(parts[1], new List<MapEdge>());
            }

            edges[parts[0]].Add(new MapEdge(l1, l2, int.Parse(parts[2])));
            edges[parts[1]].Add(new MapEdge(l2, l1, int.Parse(parts[2])));


        }
        guildNode = nodes["Guild"];
    }

    public MapLocation getLocationObjRef(string name)
    {
        MapLocation l;
        nodes.TryGetValue(name, out l);
        return l;
    }

    public MapEdge getEdge(MapLocation source, MapLocation dest)
    {
        string key = source.locationName;
        for (int i = 0; i < edges[key].Count; i++)
        {
            if (edges[key][i].dest == dest)
                return edges[key][i];
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

            foreach (var edge in edges[u.locationName])
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

    public List<List<MapLocation>> getAllPathsFromGuild(MapLocation d)
    {
        var locations = nodes.Values;

        foreach (var location in locations)
        {
            location.visited = false;
        }

        List<MapLocation> pathList = new List<MapLocation>();
        allPaths = new List<List<MapLocation>>();

        pathList.Add(guildNode);

        getAllPathsUtil(guildNode, d, pathList);

        return allPaths;
    }

    private void getAllPathsUtil(MapLocation s, MapLocation d, List<MapLocation> localPathList)
    {
        if(s == d)
        {
            allPaths.Add(new List<MapLocation>());
            foreach(var item in localPathList)
            {
                allPaths[allPaths.Count - 1].Add(item);
            }

            return;
        }

        nodes[s.locationName].visited = true;

        foreach(var l in edges[s.locationName])
        {
            if (!l.dest.visited)
            {
                localPathList.Add(l.dest);
                getAllPathsUtil(l.dest, d, localPathList);
                localPathList.Remove(l.dest);
            }
        }

        nodes[s.locationName].visited = false;
    }
}
