using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebriefReport : MonoBehaviour
{
    public TextMeshProUGUI mainBriefingText;
	public DebriefTracker debriefTracker;
	private int day;

	private void Start()
	{
		day = 0;
	}

	private void OnEnable()
	{
		mainBriefingText.text = debriefTracker.getCompiledDayReport(day);
	}
}