using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationObject : MonoBehaviour
{
    public MapLocation location { get; private set; }
    private Text locationNameDisplay;
    private WorldMap map;
    [SerializeField] private MapObject mapObj;
    public bool discovered;

    // Start is called before the first frame update
    void Start()
    {
        map = mapObj.map;
        location = map.getLocationObjRef(name);
        locationNameDisplay = transform.GetChild(0).GetComponent<Text>();
        locationNameDisplay.text = name;

        if (name == "Guild")
            discovered = true;
        else
            discovered = false;

        gameObject.SetActive(false);
    }
}
