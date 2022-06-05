using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebriefReport : MonoBehaviour
{
	public Text mainBriefingText;
	public DebriefTracker debriefTracker;
	public Text PageNumber;
	public GameObject nextPage;
	public GameObject previousPage;
	public GameObject endRecap;
	public GameObject exitButton;
	public GameObject goldDisplay;
	public GameObject displayButton;
	public Text titleText;

	private TimeSystem timeSystem;

	private int day;
	private ItemDisplayManager displayManager;
	[HideInInspector]  public bool isDisplayed = false;

	private void Awake()
	{
		//this.gameObject.SetActive(false);
		timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
		TriggerEndOfDay();
		SetGoldDisplayState(false);
	}

	private void Start()
	{
		day = 0;
		//this.gameObject.SetActive(false);
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
	}

	public void ToggleDisplay()
    {
		isDisplayed = !isDisplayed;
		if(isDisplayed)
			EnableDisplay();
		else
			DisableDisplay();
	}

	public void EnableDisplay() 
	{
		isDisplayed = true;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		endRecap.SetActive(false);
		exitButton.SetActive(true);
		nextPage.SetActive(true);
		previousPage.SetActive(true);
		PageNumber.gameObject.SetActive(true);
		titleText.text = "Day " + day + " Report";
	}

	public void DisableDisplay() 
	{
		isDisplayed = false;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		SetGoldDisplayState(false);
		displayButton.SetActive(true);
	}

	public void TriggerEndOfDay()
    {
		isDisplayed = true;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
		endRecap.SetActive(true);
		exitButton.SetActive(false);
		nextPage.SetActive(false);
		previousPage.SetActive(false);
		SetGoldDisplayState(true);
		PageNumber.gameObject.SetActive(false);
		titleText.text = "End of Day " + timeSystem.getTime().day;

		day += 1;
		print(day);
		PrintReport(day);
	}

	private void OnEnable()
	{
		day = timeSystem.getTime().day - 1;
		if(day < 0)
			day = 0;
		PrintReport(day);
		displayButton.SetActive(false);
	}

	public void PrintReport(int d)
    {
		PageNumber.text = d + "";
		mainBriefingText.text = debriefTracker.getCompiledDayReport(d);
	}

	public void DisplayNextPage()
    {
		GameTime gameTime = debriefTracker.timeSystem.getTime();
		if (day < gameTime.day - 1)
			day++;
		titleText.text = "Day " + day + " Report";
		PrintReport(day);
    }

	public void DisplayPrevPage()
    {
		if(day > 0)
			day--;
		titleText.text = "Day " + day + " Report";
		PrintReport(day);
    }

	public void SetGoldDisplayState(bool display)
    {
		goldDisplay.SetActive(display);
    }


}