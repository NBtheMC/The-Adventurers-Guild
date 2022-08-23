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
        mapObj = GameObject.Find("Canvas/World Map").GetComponent<MapObject>();
        map = mapObj.map;

        location = map.getLocationObjRef(name);
        locationNameDisplay = transform.GetChild(0).GetComponent<Text>();
        locationNameDisplay.text = name;

        if (name == "Guild")
            ShowLocation();
        else
            gameObject.SetActive(false);
    }

    public void ShowLocation()
    {
        gameObject.SetActive(true);
        discovered = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetComponent<Image>().color = Color.white;
    }
    public void HideLocation()
    {
        gameObject.SetActive(true);
        discovered = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetComponent<Image>().color = Color.black;
    }
}
