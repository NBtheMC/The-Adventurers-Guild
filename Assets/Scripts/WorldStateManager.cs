using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class WorldStateManager : MonoBehaviour
{
	public static WorldStateManager Instance;
    private Dictionary<string, WorldValue> worldValues;
    private Dictionary<string, WorldState> worldStates;
	private Dictionary<string, WorldInt> worldInts;

	// Keeps track of how many stats we're displaying.
	private int statDisplayNums = 0;
	private int spacer = 10; // How much space we're giving them.
	private int topOfDisplay; // How much space we're giving between items.
	private int startingSpace = -10; // How much space the first item needs to generate below the top.

	public GameObject intDisplayPrefab;
	public GameObject floatDisplayPrefab;
	public GameObject boolDisplayPrefab;

	// The list of all storylets we plan on creating.
	public List<Storylet> storylets;
	public Dictionary<Storylet, int> numberOfActivations = new Dictionary<Storylet, int>();
	public Dictionary<QuestSheet,Storylet> activeStorylets = new Dictionary<QuestSheet, Storylet>();

	// the reference to a QuestingManager.
	public QuestingManager questingManager;

	// the reference to the TimeSystem.
	public TimeSystem timeSystem;

	// the reference to the GameManager
	public GameManager gameManager;

	// A bunch of events for when the WorldStateManager updates itself.
	public event EventHandler<string> IntChangeEvent;
	public event EventHandler<string> StateChangeEvent;
	public event EventHandler<string> FloatChangeEvent;
	public event EventHandler<string> AdventurerHiredEvent;

	public event EventHandler<WorldStat> NewStat;

    void Awake()
    {
		storylets = new List<Storylet>(); // Clear all the storylets.

		worldValues = new Dictionary<string, WorldValue>();
        worldStates = new Dictionary<string, WorldState>();
		worldInts = new Dictionary<string, WorldInt>();

		//Set the top of Display to the spacer
		topOfDisplay = startingSpace;

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

	private void Start()
	{
		Debug.Log("THE WORLD STATE HAS STARTED.");
		storylets.Clear();
		CSVToQuests converter = this.GetComponent<CSVToQuests>();
		storylets = converter.allStorylets;

		Debug.Log($"STORYLETS NUMBER IN {storylets.Count}");

		foreach(Storylet storylet in storylets)
		{
			// Preload all the values into the dictionary.
			foreach(Storylet.TriggerInt intTrigger in storylet.triggerInts) { AddWorldInt(intTrigger.name, 0);}
			foreach(Storylet.TriggerValue floatTrigger in storylet.triggerValues) { AddWorldValue(floatTrigger.name, 0);}
			foreach(Storylet.TriggerState stateTrigger in storylet.triggerStates) { AddWorldState(stateTrigger.name, false); }

			// Add them into the dictionary and set it to zero.
			numberOfActivations.Add(storylet, 0);
			Debug.Log($"Added number of activations for {storylet.name}");
		}

		// Sets up initial trigger with Timesystem. If it doesn't exist, then *hopefully* nothing crashes.
		if (timeSystem != null) { timeSystem.TickAdded += TickTrigger; }

		// Adds a listener to questing manager to listen for finished quests.
		if (questingManager != null) { questingManager.QuestFinished += RemoveActiveStorylet; }
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
			WorldValue addedWorldValue = new WorldValue(name, value);

			worldValues.Add(name, addedWorldValue);
			// Instantiate the prefab.
			GameObject display = Instantiate(floatDisplayPrefab, this.transform);
			StoryletTesting.WorldValueChanger displayScript = display.GetComponent<StoryletTesting.WorldValueChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x, topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;
			
			// Invoke event call to add to whatever Ui element.
			NewStat?.Invoke(this, addedWorldValue);
		}
        else { worldValues[name].value = value; }
		FloatChangeEvent?.Invoke(this, name);

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
			WorldState addedWorldBool = new WorldState(name, state);

			// Add it to the dictionary.
            worldStates.Add(name, addedWorldBool);
			// Instantiate the prefab.
			GameObject display = Instantiate(boolDisplayPrefab, this.transform);
			StoryletTesting.WorldStateChanger displayScript = display.GetComponent<StoryletTesting.WorldStateChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x,topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;

			// Invoke event call to add to whatever Ui element.
			NewStat?.Invoke(this, addedWorldBool);
		}
        else
        {
            worldStates[name].state = state;
        }
		StateChangeEvent?.Invoke(this, name);
	}

	public void AddWorldInt(string name, int value)
	{
		if (!worldInts.ContainsKey(name))
		{
			WorldInt addedWorldInt = new WorldInt(name, value);

			// Add it to the dictionary
			worldInts.Add(name, addedWorldInt);
			// Instantiate the prefab.
			GameObject display = Instantiate(intDisplayPrefab, this.transform);
			StoryletTesting.WorldIntChanger displayScript = display.GetComponent<StoryletTesting.WorldIntChanger>();
			displayScript.theWorld = this;
			displayScript.worldStat = name;

			display.GetComponent<RectTransform>().anchoredPosition = new Vector2(display.GetComponent<RectTransform>().anchoredPosition.x, topOfDisplay);
			topOfDisplay -= Mathf.CeilToInt(display.GetComponent<RectTransform>().rect.height) + spacer;

			NewStat?.Invoke(this, addedWorldInt);
		}
		else
		{
			worldInts[name].value = value;
		}
		IntChangeEvent?.Invoke(this, name);
	}

	/// <summary>
	/// Changes the world value by a certain amount.
	/// </summary>
	/// <param name="name">Name of the value to change.</param>
	/// <param name="value">The change to be added to current value.</param>
	/// <param name="set">Whether this value should set the state or just alter it.</param>
	public void ChangeWorldValue(string name, float value, bool set = false)
	{
        if (!worldValues.ContainsKey(name)) { AddWorldValue(name, value); }
        else if (set) { worldValues[name].value = value; }
		else { worldValues[name].value += value;}
		FloatChangeEvent?.Invoke(this, name);
	}

	/// <summary>
	/// Changes the world value by a certain amount.
	/// </summary>
	/// <param name="name">Name of the value to change.</param>
	/// <param name="value">The change to be added to current value.</param>
	public void ChangeWorldState(string name, bool value)
	{
		if (!worldStates.ContainsKey(name)) { AddWorldState(name, value); Debug.Log($"Failed to find {name} in worldState, perhaps not already created"); }
		else { worldStates[name].state = value; }
		StateChangeEvent?.Invoke(this, name);
	}

	/// <summary>
	/// Changes the world int by a certain amount.
	/// </summary>
	/// <param name="name">Name of the value to change.</param>
	/// <param name="value">The change to be added to current value.</param>
	/// <param name="set">Whether this value should set the state or just alter it.</param>
	public void ChangeWorldInt(string name, int value, bool set = false)
	{
		if (!worldInts.ContainsKey(name)) { AddWorldInt(name, value); Debug.Log($"Failed to find {name} in worldState, perhaps not already created"); }
		else if (set) { worldInts[name].value = value; }
		else { worldInts[name].value += value; }
		IntChangeEvent?.Invoke(this, name);
	}

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
			if (numberOfActivations[storylet] > 0 && !storylet.canBeInstanced) { continue; }
			if (!storylet.canBeDuplicated && activeStorylets.ContainsValue(storylet)) { continue; }

			bool validStorylet = true;
			
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
				if (!validStorylet) { break;  }
			}

			// if this is not a valid storylet, keep searching
			if (!validStorylet) { continue; }

			// Now the trigger states.
			foreach (Storylet.TriggerState triggerState in storylet.triggerStates)
			{
				// Check if the trigger state matches the world state.
				if (GetWorldState(triggerState.name) != triggerState.state) { validStorylet = false; break; }
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
				if (!validStorylet) { /*Debug.Log($"Checking {storylet.name}. Failed on {triggerInt.name}");*/ break; }
			}

			// if this is not a valid storylet after checking through the trigger states, keep searching. otherwise, add to valid storylets.
			if (!validStorylet) { continue; }
			else { validStorylets.Add(storylet); }
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
			foreach (Storylet.IntChange change in storylet.triggerIntChanges)
			{
				// Checks to set it directly, or to change it by a value.
				if (change.set == true) { AddWorldInt(change.name, change.value); }
				else { ChangeWorldInt(change.name, change.value); }
			}

			if(!String.IsNullOrEmpty(storylet.adventurer))
            {
				Debug.Log("Adding adventurer " + storylet.adventurer);
				AdventurerHiredEvent(this, storylet.adventurer);
				//GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>().HireAdventurer(storylet.adventurer);
            }

			// Logs the number of times this quest has been actived.
			numberOfActivations[storylet]++;

			// Checks if there is an event head, to make, if so, makes a new quest
			if (storylet.eventHead != null)
			{
				QuestSheet newQuest = new QuestSheet(storylet.eventHead, storylet.questName, this, storylet.questDescription);
				newQuest.faction = storylet.factionName;
				newQuest.questGiver = storylet.issuerName;
				questingManager.AddQuest(newQuest);
				// Puts the new quest into activation
				if (!activeStorylets.ContainsValue(storylet)) { activeStorylets.Add(newQuest, storylet); }
			}
		}
	}

	public void RemoveActiveStorylet(object source, QuestSheet finishedQuest)
	{
		activeStorylets.Remove(finishedQuest);
	}
}

// Standard Structures for keeping our worldStates.
public abstract class WorldStat
{
	public string name;
}

public class WorldValue: WorldStat
{
    public float value;

	public event EventHandler<float> StateChange;

    public WorldValue(string inputName, float inputValue) { name = inputName; value = inputValue; }   

	public void Change(float input, bool set) { if (set) { value = input; } else { value += input; } }
}
public class WorldState: WorldStat
{
    public bool state;

	public event EventHandler<bool> StateChange;

    public WorldState(string inputName, bool inputState) { name = inputName; state = inputState; }

	public void Change(bool input) { state = input; }
}

public class WorldInt: WorldStat
{
	public int value;

	public event EventHandler<int> StateChange;
	public WorldInt(string inputName, int inputValue) { name = inputName; value = inputValue; }

	public void Change(int input, bool set) { if (set) { value = input; } else { value += input; } }
}