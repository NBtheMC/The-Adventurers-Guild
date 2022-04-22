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
		GameTime timeLogged = timeSystem.getTime();
		cumulatedDebriefReport[timeLogged.day].Add(new DebriefReport(timeLogged,itemToBeLogged));

		//mark that page as unread
	}

	public void AddNewDay(object source, EventArgs e)
    {
		GameTime timeLogged = timeSystem.getTime();
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
	
	public string getCompiledDayReport(int seletedTime = -1)
	{
		string dayReport = "";
		if (seletedTime < 0) { seletedTime = timeSystem.getTime().day; }
		if (seletedTime >= cumulatedDebriefReport.Count) { return dayReport; }

		dayReport += $"Day {seletedTime} \n\n";

		foreach(DebriefReport item in cumulatedDebriefReport[seletedTime])
		{
			dayReport += $"{item.time.hour}: {item.log}\n";
		}

		dayReport += "\n End of Report";

		return dayReport;
	}

}
