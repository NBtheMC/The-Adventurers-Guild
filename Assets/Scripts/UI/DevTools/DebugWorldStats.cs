using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWorldStats : MonoBehaviour
{
	private WorldStateManager worldStateManager; // our reference to the world manager.
	private List<(string, WorldStat)> statList;

	private GameObject intPrefab;
	private GameObject floatPrefab;
	public GameObject boolPrefab;

	private void Awake()
	{
		statList = new List<(string, WorldStat)>();
		worldStateManager = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
		worldStateManager.NewStat += AddStat;

		intPrefab = Resources.Load<GameObject>("WorldStateDebug/SampleIntChanger");
		floatPrefab = Resources.Load<GameObject>("WorldStateDebug/SampleFloatChanger");
		boolPrefab = Resources.Load<GameObject>("WorldStateDebug/SampleStateChanger");
	}

	private void AddStat(object sender, WorldStat input)
	{
		statList.Add((input.name,input));
	}

	private void OnEnable()
	{
		
	}

	/// <summary>
	/// Display all the stats that are found.
	/// </summary>
	private void displayFoundStats()
	{
		foreach (WorldStat worldStat in findStats())
		{
			GameObject display;
			if (worldStat is WorldInt)
			{
				display = Instantiate(intPrefab, this.transform);
				StoryletTesting.WorldValueChanger displayScript = display.GetComponent<StoryletTesting.WorldValueChanger>();
				displayScript.theWorld = worldStateManager;
				displayScript.worldStat = name;
			}
			else if (worldStat is WorldValue)
			{
				display = Instantiate(floatPrefab, this.transform);
				StoryletTesting.WorldValueChanger displayScript = display.GetComponent<StoryletTesting.WorldValueChanger>();
				displayScript.theWorld = worldStateManager;
				displayScript.worldStat = name;
			}
			else if (worldStat is WorldState) { display = Instantiate(boolPrefab, this.transform) ; }


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
