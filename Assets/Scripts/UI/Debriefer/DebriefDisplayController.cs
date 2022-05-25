using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebriefDisplayController : MonoBehaviour
{
    public DebriefReport debrief;
    public GameObject reportIndicator;
    public GameObject itemDisplay;
    private bool isDisplayed = false;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate { debrief.ToggleDisplay(); });
        this.GetComponent<Button>().onClick.AddListener(delegate { ButtonPressed(); });
        reportIndicator.SetActive(false);
    }

    public void ButtonPressed()
    {
        isDisplayed = !isDisplayed;
        reportIndicator.SetActive(false);
    }

    public void SetReportIndicator(object o, EventArgs e)
    {
        if(!isDisplayed)
            reportIndicator.SetActive(true);
    }
}
