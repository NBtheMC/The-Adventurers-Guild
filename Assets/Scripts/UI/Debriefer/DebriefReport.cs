using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebriefReport : MonoBehaviour
{
	public TextMeshProUGUI mainBriefingText;
	public DebriefTracker debriefTracker;
	public Text PageNumber;

	private TimeSystem timeSystem;

	private int day;
	private ItemDisplayManager displayManager;
	[HideInInspector]  public bool isDisplayed = false;

	private void Awake()
	{
		this.gameObject.SetActive(false);
		timeSystem = GameObject.Find("TimeSystem").GetComponent<TimeSystem>();
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
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
	}

	private void OnEnable()
	{
		day = timeSystem.getTime().day - 1;
		mainBriefingText.text = debriefTracker.getCompiledDayReport(day);
		PrintReport();
	}

	public void PrintReport()
    {
		PageNumber.text = day + "";
		mainBriefingText.text = debriefTracker.getCompiledDayReport(day);
	}

	public void DisplayNextPage()
    {
		GameTime gameTime = debriefTracker.timeSystem.getTime();
		if (day < gameTime.day - 1)
			day++;
		PrintReport();
    }

	public void DisplayPrevPage()
    {
		if(day > 0)
			day--;
		PrintReport();
    }


}