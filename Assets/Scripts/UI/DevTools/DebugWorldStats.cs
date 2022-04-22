using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWorldStats : MonoBehaviour
{
	private WorldStateManager worldStateManager; // our reference to the world manager.
	private List<(string, WorldStat)> statList;

	public GameObject intPrefab;
	public GameObject floatPrefab;
	public GameObject boolPrefab;

	public List<GameObject> activeDisplays;

	private void Awake()
	{
		statList = new List<(string, WorldStat)>();
		activeDisplays = new List<GameObject>();
	}

	private void Start()
	{
		worldStateManager = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
		worldStateManager.NewStat += AddStat;
	}

	private void AddStat(object sender, WorldStat input)
	{
		statList.Add((input.name,input));
	}

	private void OnEnable()
	{
		List<WorldStat> foundStats = findStats();
		displayFoundStats(foundStats);
	}

	private void OnDisable()
	{
		foreach (GameObject display in activeDisplays)
		{
			GameObject.Destroy(display.gameObject);
		}
	}

	/// <summary>
	/// Display all the stats that are found.
	/// </summary>
	private void displayFoundStats(List<WorldStat> input)
	{
		int topOfDisplay = -10;
		int spacer = 10;
		foreach (WorldStat worldStat in input)
		{
			GameObject display;

			// goes in and changes a bunch of things for the prefabricated display.
			if (worldStat is WorldValue)
			{
				display = Instantiate(floatPrefab, this.transform);
				StoryletTesting.WorldValueChanger displayScript = display.GetComponent<StoryletTesting.WorldValueChanger>();
				displayScript.theWorld = worldStateManager;
				displayScript.worldStat = worldStat.name;
			}
			else if (worldStat is WorldInt)
			{
				display = Instantiate(intPrefab, this.transform);
				StoryletTesting.WorldIntChanger displayScript = display.GetComponent<StoryletTesting.WorldIntChanger>();
				displayScript.theWorld = worldStateManager;
				displayScript.worldStat = worldStat.name;
			}
			else
			{
				display = Instantiate(boolPrefab, this.transform) ;
				StoryletTesting.WorldStateChanger displayScript = display.GetComponent<StoryletTesting.WorldStateChanger>();
				displayScript.theWorld = worldStateManager;
				displayScript.worldStat = worldStat.name;
			}

			// Goes in and displays all of them 
			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x, topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;

			// Track the new displays.
			activeDisplays.Add(display);
		}
	}

	// Gets the first twenty of active results.
	private List<WorldStat> findStats()
	{
		List<WorldStat> activeStats = new List<WorldStat> ();

		for (int i = 0; i < 20; i++)
		{
			if (i < statList.Count) { activeStats.Add(statList[i].Item2); }
			else { break; }
		}
		
		return activeStats;
	}
}
