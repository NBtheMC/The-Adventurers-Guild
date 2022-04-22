using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebriefDisplayController : MonoBehaviour
{
    public GameObject reportIndicator;
    public GameObject itemDisplay;
    private bool isDisplayed = false;
    // Start is called before the first frame update
    void Start()
    {
        DebriefReport debrief = GameObject.Find("DebriefReport").GetComponent<DebriefReport>();
        this.GetComponent<Button>().onClick.AddListener(delegate { debrief.ToggleDisplay(); });
        this.GetComponent<Button>().onClick.AddListener(delegate { ButtonPressed(); });
        //reportIndicator.SetActive(false);
    }

    public void ButtonPressed()
    {
        isDisplayed = !isDisplayed;
        itemDisplay.SetActive(!itemDisplay.activeSelf);
        reportIndicator.SetActive(false);
    }

    public void SetReportIndicator(object o, EventArgs e)
    {
        if(!isDisplayed)
            reportIndicator.SetActive(true);
    }
}
