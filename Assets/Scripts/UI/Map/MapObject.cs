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

        gameObject.SetActive(false);
    }

    public void ToggleDisplay()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void TestLocationReveal(string locationName)
    {
        //discover associated location
        GameObject locGO;
        nodes.TryGetValue(locationName, out locGO);

        //discover edges that make path to the location
        if (locGO != null)
        {
            LocationObject l = locGO.GetComponent<LocationObject>();
            l.ShowLocation();

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

    //needs to be called when new eventNode begins
    //make new event listener inside QuestingManager, add this function to that listener
    public void NewQuestStarted(object source, QuestSheet quest)
    {
        //discover associated location
        GameObject locGO;
        nodes.TryGetValue(quest.location, out locGO);

        //discover edges that make path to the location
        if (locGO != null)
        {
            LocationObject l = locGO.GetComponent<LocationObject>();
            l.ShowLocation();

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
}
