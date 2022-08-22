using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public WorldMap map { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        map = new WorldMap();
    }

    void Start()
    {
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

    //private void NewQuestStarted(object source, QuestSheet quest)
    public void NewQuestStarted(string quest)
    {
        //discover associated location
        LocationObject l = null;
        foreach (Transform child in transform.GetChild(1))
        {
            l = child.GetComponent<LocationObject>();
            //if (l.location.locationName == quest.location)
            if (l.location.locationName == quest)
            {
                l.ShowLocation();
                break;
            }
        }

        //discover edges that make path to the location
        if(l != null)
        {
            //IMPROVE BY HAVING NODES TRACK WHICH EDGES THEY ARE CONNECTED TO
            var paths = map.getAllPathsFromGuild(l.location);
            foreach(var path in paths)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    foreach (Transform child in transform.GetChild(0))
                    {
                        EdgeObject edge = child.GetComponent<EdgeObject>();
                        if ((edge.Node1.location == path[i] && edge.Node2.location == path[i + 1])
                            || (edge.Node2.location == path[i] && edge.Node1.location == path[i + 1]))
                        {
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
            }   
        }
        
    }

    private void ShowDiscoveredPath()
    {
        //display discovered nodes
        foreach (Transform child in transform.GetChild(1))
        {
            LocationObject l = child.GetComponent<LocationObject>();
            if (l.discovered)
            {
                l.ShowLocation();  
            }
        }

        //display edges between discovered nodes
        foreach (Transform child in transform.GetChild(0))
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
