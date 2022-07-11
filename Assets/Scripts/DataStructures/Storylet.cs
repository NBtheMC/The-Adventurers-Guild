using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A Storylet is what the world system will use to generate quests.
/// It needs trigger conditions from the world, and quest creation conditions once configured.
/// </summary>
//[CreateAssetMenu(fileName = "NewStorylet", menuName = "Storylet", order = 0)]
public class Storylet
{
	/// Storylets are triggered by some type of world condition, and should hold objects for constant triggering.
	/// Similar to Quests Structures, we'll create a dictionary of string ints to hold status

	//TriggerValue tells a Storylet what world values to trigger on.
	public enum NumberTriggerType
	{
		LessThan = -2,
		LessThanEqualTo = -1,
		EqualTo = 0,
		GreaterThan = 1,
		GreaterThanEqualTo = 2,
	}

	// TriggerInt tells a Storylet what world intergers to trigger on.
	[System.Serializable] public struct TriggerInt { public string name; public int value; public NumberTriggerType triggerType; }

	// TriggerValue tells a Storylet what world values to trigger on.
	[System.Serializable] public struct TriggerValue { public string name; public float value; public NumberTriggerType triggerType; }

	// TriggerState tells a Storylet what world states to trigger on
	[System.Serializable] public struct TriggerState { public string name; public bool state; }

	// IntChange is a set of instructions of how a storylet should change the world ints once triggered.
	[System.Serializable] public struct IntChange { public string name; public int value; public bool set; }

	// ValueChange is a set of instructions of how a storylet should change the world values.
	[System.Serializable] public struct ValueChange { public string name; public float value; public bool set; }

	// StateChange is a set of instructions of how a storylet should change the world state.
	[System.Serializable] public struct StateChange { public string name; public bool state; }

	public string questName; // The name of the quest they will embark on.
	public EventNode eventHead; // The head of the event tree that is associated with this event.
	[TextAreaAttribute(4, 10)]
	public string questDescription; // The description of the Quest, if there is to be one.

	// Set to true if this multiple versions of this quest could be triggered at the same time.
	public bool canBeInstanced = false;
	public bool canBeDuplicated = false;
	public bool endGame = false;

	// TriggerValues and TriggerStates keeps all the world conditions that we're looking to satisfy before triggering this storylet.
	public List<TriggerInt> triggerInts = new List<TriggerInt>();
	public List<TriggerValue> triggerValues = new List<TriggerValue>();
	public List<TriggerState> triggerStates = new List<TriggerState>();


	// A condition for this item has already been triggered.
	[HideInInspector] public int numInstances = 0;
	// Note from Eric, you should really get rid of this and have worldstate manager tracking the specfic uses of this storylet instead. This scripts isn't meant to hold variables.

	// triggerValueChanges and triggerStateChange are what the storylet will do to the world the moment this Storylet is triggered.
	// WorldState should handle this.
	public List<IntChange> triggerIntChanges = new List<IntChange>();
	public List<ValueChange> triggerValueChanges = new List<ValueChange>();
	public List<StateChange> triggerStateChanges = new List<StateChange>();

	// Adventurer that gets added to the roster if this storylet is triggered
	public string adventurer;
	public string name;
	public string factionName;
	public string issuerName;
	public string debriefMessage;

	/// <summary>
	/// Easy way to determine items using the NumberTriggerType.
	/// </summary>
	/// <param name="left">Item on the left of the sign</param>
	/// <param name="sign">The Sign. Use Storylet.number Trigger Type.</param>
	/// <param name="right">The number of the right.</param>
	/// <returns></returns>
	public static bool SignEvaluator(float left, NumberTriggerType sign, float right)
	{
		bool evaluation = false;

		switch (sign)
		{
			case NumberTriggerType.LessThanEqualTo:
				if (left <= right) { evaluation = true; }
				break;
			case NumberTriggerType.LessThan:
				if (left < right) { evaluation = true; }
				break;
			case NumberTriggerType.EqualTo:
				if (left == right) { evaluation = true; }
				break;
			case NumberTriggerType.GreaterThanEqualTo:
				if (left >= right) { evaluation = true; }
				break;
			case NumberTriggerType.GreaterThan:
				if (left > right) { evaluation = true; }
				break;
			default:
				break;
		}

		return evaluation;
	}
}