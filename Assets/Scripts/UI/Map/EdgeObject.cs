using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeObject : MonoBehaviour
{
    public LocationObject Node1;
    public LocationObject Node2;

    // Start is called before the first frame update
    void Start()
    {
        string[] locations = name.Split('-');
        if(Node1 == null)
            Node1 = GameObject.Find(locations[0]).GetComponent<LocationObject>();
        if(Node2 == null)
            Node2 = GameObject.Find(locations[1]).GetComponent<LocationObject>();

        gameObject.SetActive(false);
    }
}
