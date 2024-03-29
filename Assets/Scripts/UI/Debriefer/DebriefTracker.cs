using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebriefTracker : MonoBehaviour
{
	private List<List<DebriefReport>> cumulatedDebriefReport; //Used to keep all of our debrief reports.

	public TimeSystem timeSystem;

	private void Awake()
	{
		cumulatedDebriefReport = new List<List<DebriefReport>>();
		//add day 0 report list
		cumulatedDebriefReport.Add(new List<DebriefReport>());
		timeSystem.NewDay += AddNewDay;
	}

	/// <summary>
	/// Submits a log for the current entry.
	/// </summary>
	/// <param name="itemToBeLogged">the item you need logged.</param>
	public void submitLog(string itemToBeLogged)
	{
		GameTime timeLogged = timeSystem.GameTime;
		print(timeLogged.day);
		cumulatedDebriefReport[timeLogged.day - 1].Add(new DebriefReport(timeLogged,itemToBeLogged));

		//mark that page as unread
	}

	public void AddNewDay(object source, GameTime gameTime)
    {
		GameTime timeLogged = timeSystem.GameTime;
		if (timeLogged.day >= cumulatedDebriefReport.Count)
		{
			cumulatedDebriefReport.Add(new List<DebriefReport>());
		}
	}

	private struct DebriefReport
	{
		public GameTime time; public string log;
		public DebriefReport(GameTime timeInput, string logInput) { time = timeInput; log = logInput; }
	}
	
	public string getCompiledDayReport(int selectedTime = -1)
	{
		string dayReport = "";
		if (selectedTime < 0) { selectedTime = timeSystem.GameTime.day; }
		if (selectedTime == 0) { return "Welcome to the Adventurer's Guild!\n"; }
		if (selectedTime > cumulatedDebriefReport.Count) { return dayReport; }

		//dayReport += $"Day {selectedTime} \n\n";

		foreach(DebriefReport item in cumulatedDebriefReport[selectedTime - 1])
		{
			dayReport += $"{item.time.hour}: {item.log}\n";
		}

		dayReport += "\n End of Report";

		return dayReport;
	}

}
