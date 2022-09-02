using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public WorldMap map { get; private set; }
    private Dictionary<string, GameObject> nodes;
    private Dictionary<string, GameObject> edges;

    // Start is called before the first frame update
    void Awake()
    {
        map = new WorldMap();
        nodes = new Dictionary<string, GameObject>();
        edges = new Dictionary<string, GameObject>();
    }

    void Start()
    {
        foreach (Transform child in transform.GetChild(2))
        {
            nodes.Add(child.name, child.gameObject);
        }

        foreach (Transform child in transform.GetChild(1))
        {
            edges.Add(child.name, child.gameObject);
        }

        //GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += NewQuestStarted;
        MapLocation s = map.getLocationObjRef("Farm");
        map.getAllPathsFromGuild(s);
        gameObject.SetActive(false);
    }

    public void FindPath(string src, string dest)
    {
        MapLocation s = map.getLocationObjRef(src);
        MapLocation d = map.getLocationObjRef(dest);

        var temp = map.getShortestPath(s, d);

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

    private void OnEnable()
    {
        ShowDiscoveredPath();
    }

    public void ToggleDisplay()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void TestLocationReveal(string locationName)
    {
        //discover associated location
        LocationObject l = null;
        foreach (Transform child in transform.GetChild(2))
        {
            l = child.GetComponent<LocationObject>();
            if (l.location.locationName == locationName)
            {
                l.ShowLocation();
                break;
            }
        }

        //discover edges that make path to the location
        if (l != null)
        {
            var path = map.getShortestPathFromGuild(l.location).Item1;
            for (int i = 0; i < path.Count - 1; i++)
            {
                //lookup current edge game object
                GameObject edgeGO;
                edges.TryGetValue(path[i].locationName + "-" + path[i + 1].locationName, out edgeGO);
                if (edgeGO == null)
                {
                    edges.TryGetValue(path[i + 1].locationName + "-" + path[i].locationName, out edgeGO);
                }
                    

                EdgeObject edge = edgeGO.GetComponent<EdgeObject>();
                if (edge.Node1.discovered)
                    edge.Node1.ShowLocation();
                else
                    edge.Node1.HideLocation();

                if (edge.Node2.discovered)
                    edge.Node2.ShowLocation();
                else
                    edge.Node2.HideLocation();

                edge.ShowEdge();
            }

        }
    }

    public void NewQuestStarted(object source, QuestSheet quest)
    {
        //discover associated location
        LocationObject l = null;
        foreach (Transform child in transform.GetChild(2))
        {
            l = child.GetComponent<LocationObject>();
            if (l.location.locationName == quest.location)
            {
                l.ShowLocation();
                break;
            }
        }

        //discover edges that make path to the location
        if (l != null)
        {
            var path = map.getShortestPathFromGuild(l.location).Item1;
            for (int i = 0; i < path.Count - 1; i++)
            {
                //lookup current edge game object
                GameObject edgeGO;
                edges.TryGetValue(path[i].locationName + "-" + path[i + 1].locationName, out edgeGO);
                if (edgeGO == null)
                {
                    edges.TryGetValue(path[i + 1].locationName + "-" + path[i].locationName, out edgeGO);
                }


                EdgeObject edge = edgeGO.GetComponent<EdgeObject>();
                if (edge.Node1.discovered)
                    edge.Node1.ShowLocation();
                else
                    edge.Node1.HideLocation();

                if (edge.Node2.discovered)
                    edge.Node2.ShowLocation();
                else
                    edge.Node2.HideLocation();

                edge.ShowEdge();
            }

        }

    }

    private void ShowDiscoveredPath()
    {
        //display discovered nodes
        foreach (Transform child in transform.GetChild(2))
        {
            LocationObject l = child.GetComponent<LocationObject>();
            if (l.discovered)
            {
                l.ShowLocation();  
            }
        }

        //display edges between discovered nodes
        foreach (Transform child in transform.GetChild(1))
        {
            EdgeObject edge = child.GetComponent<EdgeObject>();
            if (edge.Node1 != null && edge.Node2 != null)
            {
                //if (edge.Node1.discovered && edge.Node2.discovered)
                if(edge.discovered)
                {
                    edge.ShowEdge();
                }
            }

        }
    }
}
