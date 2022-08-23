using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EdgeObject : MonoBehaviour
{
    public LocationObject Node1;
    public LocationObject Node2;
    public bool discovered;

    // Start is called before the first frame update
    void Start()
    {
        string[] locations = name.Split('-');
        if(Node1 == null)
            Node1 = GameObject.Find("World Map/Nodes/" + locations[0]).GetComponent<LocationObject>();
        if(Node2 == null)
            Node2 = GameObject.Find("World Map/Nodes/" + locations[1]).GetComponent<LocationObject>();

        HideEdge();
    }

    public void ShowEdge()
    {
        discovered = true;
        transform.GetComponent<Image>().enabled = true;
    }
    public void HideEdge()
    {
        discovered = false;
        transform.GetComponent<Image>().enabled = false;
    }
}
