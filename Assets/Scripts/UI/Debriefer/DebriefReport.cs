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

	private int day;
	private ItemDisplayManager displayManager;
	[HideInInspector]  public bool isDisplayed = false;

	private void Awake()
	{
		this.gameObject.SetActive(false);
	}

	private void Start()
	{
		day = 1;
		//this.gameObject.SetActive(false);
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		displayManager.DisplayDebrief(isDisplayed);
	}

	public void ToggleDisplay()
    {
		isDisplayed = !isDisplayed;
		displayManager = transform.parent.GetComponent<ItemDisplayManager>();
		print(isDisplayed);
		print(displayManager.name);
		displayManager.DisplayDebrief(isDisplayed);
	}

	private void OnEnable()
	{
		day = debriefTracker.timeSystem.getTime().day;
		mainBriefingText.text = debriefTracker.getCompiledDayReport(day);
	}

	public void PrintReport()
    {
		PageNumber.text = day + "";
		mainBriefingText.text = debriefTracker.getCompiledDayReport(day);
	}

	public void DisplayNextPage()
    {
		GameTime gameTime = debriefTracker.timeSystem.getTime();
		if (day < gameTime.day)
			day++;
		PrintReport();
    }

	public void DisplayPrevPage()
    {
		if(day > 1)
			day--;
		PrintReport();
    }


}