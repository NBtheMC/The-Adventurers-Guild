using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapObject : MonoBehaviour
{
    public WorldMap map { get; private set; }
    private Transform transform;
    // Start is called before the first frame update
    void Awake()
    {
        transform = GetComponent<Transform>();
        map = new WorldMap();
    }

    void Start()
    {
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
        //display discovered nodes
        foreach (Transform child in transform.GetChild(1))
        {
            if (child.GetComponent<LocationObject>().discovered)
            {
                child.gameObject.SetActive(true);
            }
        }

        //display edges between discovered nodes
        foreach (Transform child in transform.GetChild(0))
        {
            EdgeObject edge = child.GetComponent<EdgeObject>();
            if (edge.Node1 != null && edge.Node2 != null)
            {
                if (edge.Node1.discovered && edge.Node2.discovered)
                {
                    child.gameObject.SetActive(true);
                } 
            }

        }
    }

    public void ToggleDisplay()
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
