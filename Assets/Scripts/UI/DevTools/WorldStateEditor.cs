using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStateEditor : MonoBehaviour
{
    // Keeps track of how many stats we're displaying.
    private int statDisplayNums = 0;
    private int spacer = 10; // How much space we're giving them.
    private int topOfDisplay; // The current highest point we're putting a new element. 
    private int startingSpace = -10; // How much space the first item needs to generate below the top.
	private WorldStateManager worldStateManager;

	public int maxDisplay; // What the maximum amount of editable stats are.

    public GameObject intDisplayPrefab;
    public GameObject floatDisplayPrefab;
    public GameObject boolDisplayPrefab;

	Dictionary<string,IWorldStat> statDict = new Dictionary<string,IWorldStat>();

	private void Awake()
	{
		topOfDisplay = startingSpace;
		worldStateManager = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
		worldStateManager.NewStatEvent += addToPrefabList;
	}

	private void Start()
	{

	}

	private void OnEnable()
	{
	}

	private void addToPrefabList(object sender, IWorldStat worldStat)
	{
		if (worldStat is WorldInt) { }
	}

	private void generateFloatPrefab() { }
	private void generateBoolPrefab() { }
	private void generateIntPrefab() { }

}
