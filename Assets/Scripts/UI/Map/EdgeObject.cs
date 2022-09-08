using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EdgeObject : MonoBehaviour
{
    public LocationObject Node1;
    public LocationObject Node2;
    public bool discovered { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
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
