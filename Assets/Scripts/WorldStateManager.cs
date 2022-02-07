using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    private Dictionary<string, WorldValue> worldValues;
    private Dictionary<string, WorldState> worldStates;
	private Dictionary<string, WorldInt> worldInts;

	// Keeps track of how many stats we're displaying.
	private int statDisplayNums = 0;
	private int spacer = 10; // How much space we're giving them.
	private int topOfDisplay; // How much space we're giving between items.
	private int startingSpace = -60; // How much space the first item needs to generate below the top.

	public GameObject intDisplayPrefab;
	public GameObject floatDisplayPrefab;
	public GameObject boolDisplayPrefab;

	// The list of all storylets we plan on creating.
    public List<Storylet> storylets = new List<Storylet>();

	// the reference to a QuestingManager.
	public QuestingManager questingManager;

	// the reference to the TimeSystem.
	public TimeSystem timeSystem;

    void Awake()
    {
        worldValues = new Dictionary<string, WorldValue>();
        worldStates = new Dictionary<string, WorldState>();
		worldInts = new Dictionary<string, WorldInt>();

		//Set the top of Display to the spacer
		topOfDisplay = startingSpace;
    }

	private void Start()
	{
		foreach(Storylet storylet in storylets)
		{
			// resets each storylet's active quest value to zero. will come up with better solution in future.
			storylet.numInstances = 0;

			// Preload all the values into the dictionary.
			foreach(Storylet.TriggerInt intTrigger in storylet.triggerInts) { AddWorldInt(intTrigger.name, 0);}
			foreach(Storylet.TriggerValue floatTrigger in storylet.triggerValues) { AddWorldValue(floatTrigger.name, 0);}
			foreach(Storylet.TriggerState stateTrigger in storylet.triggerStates) { AddWorldState(stateTrigger.name, false); }
		}

		// Sets up initial trigger with Timesystem. If it doesn't exist, then *hopefully* nothing crashes.
		if (timeSystem != null) { timeSystem.TickAdded += TickTrigger; }
	}

	/// <summary>
	/// Adds a new World Value to the World State Manager.
	/// If the value already exists, it replaces the existed value with the new value.
	/// Therefore, this can also be used to set values.
	/// </summary>
	/// <param name="name">Name of the value</param>
	/// <param name="value">The value.</param>
	public void AddWorldValue(string name, float value)
	{
		if (!worldValues.ContainsKey(name))
        {
            worldValues.Add(name, new WorldValue(name, value));
			// Instantiate the prefab.
			GameObject display = Instantiate(floatDisplayPrefab, this.transform);
			StoryletTesting.WorldValueChanger displayScript = display.GetComponent<StoryletTesting.WorldValueChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x, topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;
		}
        else { worldValues[name].value = value; }
    }

    /// <summary>
    /// Adds a new World State to the World State Manager
    /// If the state already exists, it replaces the existing state with the new state.
    /// Therefore, this can also be used to set states.
    /// </summary>
    /// <param name="name">Name of the State</param>
    /// <param name="value">The State.</param>
    public void AddWorldState(string name, bool state)
	{
        if (!worldStates.ContainsKey(name))
        {
			// Add it to the dictionary.
            worldStates.Add(name, new WorldState(name, state));
			// Instantiate the prefab.
			GameObject display = Instantiate(boolDisplayPrefab, this.transform);
			StoryletTesting.WorldStateChanger displayScript = display.GetComponent<StoryletTesting.WorldStateChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x,topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;

			Debug.Log($"Instantiated something: {name}");
		}
        else
        {
            worldStates[name].state = state;
        }
    }

	public void AddWorldInt(string name, int value)
	{
		if (!worldInts.ContainsKey(name))
		{
			// Add it to the dictionary
			worldInts.Add(name, new WorldInt(name, value));
			// Instantiate the prefab.
			GameObject display = Instantiate(intDisplayPrefab, this.transform);
			StoryletTesting.WorldIntChanger displayScript = display.GetComponent<StoryletTesting.WorldIntChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x, topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;

			Debug.Log($"Instantiated something: {name}");
		}
		else
		{
			worldInts[name].value = value;
		}
	}


	/// <summary>
	/// Changes the world value by a certain amount.
	/// </summary>
	/// <param name="name">Name of the value to change.</param>
	/// <param name="value">The change to be added to current value.</param>
	public void ChangeWorldValue(string name, float value)
	{
        if (!worldValues.ContainsKey(name)) { AddWorldValue(name, value); }
        else { worldValues[name].value += value; }
	}

    // You don't need a changeWorldState. Just set it using addWorldState.

    public float GetWorldValue(string key)
	{
        WorldValue specifiedWorldValue;
        if (worldValues.TryGetValue(key, out specifiedWorldValue))
		{
            return specifiedWorldValue.value;
		}
		else { AddWorldValue(key, 0); return 0; }
	}

    public bool GetWorldState(string key)
	{
        WorldState specifiedWorldState;
        if (worldStates.TryGetValue(key, out specifiedWorldState))
		{
            return specifiedWorldState.state;
		}
        else { AddWorldState(key, false); return false; }
	}

	public int GetWorldInt(string key)
	{
		WorldInt specifiedWorldInt;
		if (worldInts.TryGetValue(key, out specifiedWorldInt))
		{
			return specifiedWorldInt.value;
		}
		else { AddWorldInt(key, 0); return 0; }
	}


	/// <summary>
	/// Literally just here to curcumvent the overload from timesystem.
	/// </summary>
	/// <param name=""></param>
	public void TickTrigger(object source, GameTime gameTime){
		TriggerStorylets();
	}


    /// <summary>
    /// Checks the Storylet List for any activatable storylet, then activates them.
    /// </summary>
    public void TriggerStorylets()
	{

		List<Storylet> validStorylets = new List<Storylet>();

		// checks all our storylets to see if there are any valid storylets to trigger.
		foreach (Storylet storylet in storylets)
		{

			// Checks to see if it can be instanced, and if it can't, whether we've instanced it already.
			if (storylet.numInstances > 0 && !storylet.canBeInstanced) { continue; }

			bool validStorylet = true;

			//Debug.Log($"Begining Trigger Checks. Total of {storylet.triggerValues.Count} trigger values.");
			
			// Goes through the list of trigger values.
			foreach (Storylet.TriggerValue triggerValue in storylet.triggerValues)
			{

				// create a copy of the world's current value to check against.
				float worldValue = GetWorldValue(triggerValue.name);
				switch (triggerValue.triggerType)
				{
					case Storylet.NumberTriggerType.LessThanEqualTo:
						if (worldValue > triggerValue.value) { validStorylet = false;}
						break;
					case Storylet.NumberTriggerType.LessThan: // Fail check if world value is more.
						if (worldValue >= triggerValue.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.EqualTo: // checks for exact equal value. fail check if world value is not exact.
						if (worldValue != triggerValue.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.GreaterThanEqualTo:
						if (worldValue < triggerValue.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.GreaterThan: // Fail check if world value is less.
						if (worldValue <= triggerValue.value) { validStorylet = false; }
						break;
					default:
						break;
				}
				// If the value ended up false, stops checking other values. 
				if (!validStorylet) { Debug.Log($"Checking {storylet.name}. Failed on {triggerValue.name}"); break;  }
			}

			// if this is not a valid storylet, keep searching
			if (!validStorylet) { continue; }

			// Now the trigger states.
			foreach (Storylet.TriggerState triggerState in storylet.triggerStates)
			{
				// Check if the trigger state matches the world state.
				if (GetWorldState(triggerState.name) != triggerState.state) { validStorylet = false; Debug.Log($"Checking {storylet.name}. Failed on {triggerState.name}"); break; }
			}

			if (!validStorylet) { continue;}

			foreach (Storylet.TriggerInt triggerInt in storylet.triggerInts)
			{
				// create a copy of the world's current value to check against.
				int worldInt = GetWorldInt(triggerInt.name);
				switch (triggerInt.triggerType)
				{
					case Storylet.NumberTriggerType.LessThanEqualTo:
						if (worldInt > triggerInt.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.LessThan: // Fail check if world value is more.
						if (worldInt >= triggerInt.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.EqualTo: // checks for exact equal value. fail check if world value is not exact.
						if (worldInt != triggerInt.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.GreaterThanEqualTo:
						if (worldInt < triggerInt.value) { validStorylet = false; }
						break;
					case Storylet.NumberTriggerType.GreaterThan: // Fail check if world value is less.
						if (worldInt <= triggerInt.value) { validStorylet = false; }
						break;
					default:
						break;
				}
				// If the value ended up false, stops checking other values. 
				if (!validStorylet) { break; Debug.Log($"Checking {storylet.name}. Failed on {triggerInt.name}"); }
			}

			// if this is not a valid storylet after checking through the trigger states, keep searching. otherwise, add to valid storylets.
			if (!validStorylet) { continue; }
			else { validStorylets.Add(storylet); Debug.Log($"Storylet {storylet.questName} works."); }
		}

		// goes through the list of valid storylets and triggers them.
		foreach (Storylet storylet in validStorylets)
		{
			// Goes through the storylets and applies worldchanges.
			foreach (Storylet.ValueChange valueChange in storylet.triggerValueChanges)
			{
				// Checks to set it directly, or to change it by a value.
				if (valueChange.set == true) { AddWorldValue(valueChange.name, valueChange.value); }
				else { ChangeWorldValue(valueChange.name, valueChange.value); }
			}
			foreach (Storylet.StateChange change in storylet.triggerStateChanges)
			{
				AddWorldState(change.name, change.state);
			}

			storylet.numInstances++;

			Debug.Log($"New Quest {storylet.name} created.");

			// This will need to change depending on  how Parm codes the new questing manager.
			QuestSheet newQuest = new QuestSheet(storylet.eventHead,storylet.questName);


			//questingManager.bankedQuests.Add(newQuest);
			questingManager.AddQuest(newQuest);
			// Added a quest.
		}
	}
}

// Standard Structures for keeping our worldStates.
public class WorldValue
{
    public string name; public float value;

    public WorldValue(string inputName, float inputValue) { inputName = name; inputValue = value; }   
}
public class WorldState
{
    public string name; public bool state;

    public WorldState(string inputName, bool inputState) { inputName = name; inputState = state;}
}

public class WorldInt
{
	public string name; public int value;
	public WorldInt(string inputName, int inputValue) { inputName = name;inputValue = value; }
}