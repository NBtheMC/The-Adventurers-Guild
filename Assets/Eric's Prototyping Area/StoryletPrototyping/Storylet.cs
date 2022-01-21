using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Storylet is what the world system will use to generate quests.
/// It needs trigger conditions from the world, and quest creation conditions once configured.
/// </summary>
[CreateAssetMenu(fileName = "NewStorylet",menuName = "Storylet",order = 0)]
public class Storylet : ScriptableObject
{
	/// Storylets are triggered by some type of world condition, and should hold objects for constant triggering.
	/// Similar to Quests Structures, we'll create a dictionary of string ints to hold status

	//TriggerValue tells a Storylet what world values to trigger on.
	[System.Serializable] public struct triggerValue { public string name; public float value; public int triggerType; }

	//TriggerState tells a Storylet what world states to trigger on
	[System.Serializable] public struct triggerState { public string name; public bool state; }
	
	// ValueChange is a set of instructions of how a storylet should change the world values.
	[System.Serializable] public struct ValueChange { public string name; public float value; public bool set; }

	//StateChange is a set of instructions of how a storylet should change the world state.
	[System.Serializable] public struct StateChange { public string name; public bool state; }


	public string questName; // The name of the quest they will embark on.
	public EventNode eventHead; // The head of the event tree that is associated with this event.

	// TriggerValues and TriggerStates keeps all the world conditions that we're looking to satisfy before triggering this storylet.
	public List<triggerValue> triggerValues = new List<triggerValue>();
	public List<triggerState> triggerStates = new List<triggerState>();

	// Set to true if this multiple versions of this quest could be triggered at the same time.
	public bool canBeInstanced = false;

	// A condition for this item has already been triggered.
	[HideInInspector] public int numInstances = 0;

	// triggerValueChanges and triggerStateChange are what the storylet will do to the world the moment this Storylet is triggered.
	// WorldState should handle this.
	public List<ValueChange> triggerValueChanges = new List<ValueChange>();
	public List<StateChange> triggerStateChanges = new List<StateChange>();
	
}