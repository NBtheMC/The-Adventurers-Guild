using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationObject : MonoBehaviour
{
    public MapLocation location { get; private set; }
    public WorldMap map;
    public bool discovered { get; private set; }
    public PartyDisplay partyDisplay { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Main UI/World Map").GetComponent<MapObject>().map;
        location = map.getLocationObjRef(name);

        if (name == "Guild")
            ShowLocation();
        else
            gameObject.SetActive(false);

        partyDisplay = transform.GetChild(1).GetComponent <PartyDisplay>();
    }

    public void ShowLocation()
    {
        gameObject.SetActive(true);
        discovered = true;
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void HideLocation()
    {
        gameObject.SetActive(true);
        discovered = false;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
