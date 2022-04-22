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

	private void Start()
	{
		day = 0;
		this.gameObject.SetActive(false);
	}

	public void ToggleDisplay()
    {
		print("toggle display");
		this.gameObject.SetActive(!this.gameObject.activeSelf);
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
		if(day > 0)
			day--;
		PrintReport();
    }


}