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
    private bool indicatorDisplayed = false;

    private TimeSystem timeSystem;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(delegate { debrief.ToggleDisplay(); });
        this.GetComponent<Button>().onClick.AddListener(delegate { ButtonPressed(); });
        reportIndicator.SetActive(false);

        timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();

        timeSystem.NewDay += SetReportIndicator;
    }

    public void ButtonPressed()
    {
        isDisplayed = !isDisplayed;
        if (indicatorDisplayed) 
        {
            reportIndicator.SetActive(false);
            indicatorDisplayed = false;
        }
        
    }

    public void SetReportIndicator(object o, GameTime e)
    {
        print("NEW LOG ADDED");
        if(!isDisplayed && timeSystem.GameTime.day > 0) 
        {
            indicatorDisplayed = true;
            reportIndicator.SetActive(true);
        }
            
    }
}
