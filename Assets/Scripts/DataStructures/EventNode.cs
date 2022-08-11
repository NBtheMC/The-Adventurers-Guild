using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The event node checks between a large number of conditions.
/// Sends back an package full of information for Quest Sheet to use, specifically what the next node is.
/// Also update bonds.
/// </summary>
[CreateAssetMenu(fileName = "NewEventNode", menuName = "Event Node", order = 0)]
public class EventNode : ScriptableObject
{
	public string name;

	[TextAreaAttribute(2, 10)]
	public string description; //what the event is

	public CharacterSheet.StatDescriptors stat; // the stat to be checked against. Should correspond with PartySheet

	public List<EventCase> eventCases;

	public EventCase defaultCase; // The node to go to in case all eventNodes fail.

	public WorldStateManager theWorld;

	/// <summary>
	/// For use in EventCase, checking against the party's stats.
	/// </summary>
	[System.Serializable]
	public struct StatCheck { public CharacterSheet.StatDescriptors stat; public NumberTriggerType triggerType; public int value; }

	public struct PartyCheck { public string character; public bool present; }
	
	/// <summary>
	/// Used to specify a case for use in this event.
	/// </summary>
	[System.Serializable]
	public class EventCase
	{
		// All the specific details of going down this event.
		public EventNode nextNode = null; // What event node to progress to when it hits this case.
		public int time = 0; // How much time to wait after triggering EventCase
		public int reward = 0; // How much gold to provide after waiting/
		public int bondupdate = 0; // How much bond to increse after event.
		public string progressionDescription; // The string given to follow up after the 

		// All our triggers for how this Event Case sets off.
		public List<StatCheck> statTriggers;
		public List<Storylet.TriggerInt> intTriggers;
		public List<Storylet.TriggerValue> floatTriggers;
		public List<Storylet.TriggerState> boolTriggers;
		public List<PartyCheck> partyTriggers;

		// All the changes upon entering this Event Case.
		public List<Storylet.IntChange> intChanges;
		public List<Storylet.ValueChange> floatChanges;
		public List<Storylet.StateChange> boolChanges;

		public EventCase()
		{
			statTriggers = new List<StatCheck>();
			intTriggers = new List<Storylet.TriggerInt>();
			floatTriggers = new List<Storylet.TriggerValue>();
			boolTriggers = new List<Storylet.TriggerState>();
			partyTriggers = new List<PartyCheck>();
			intChanges = new List<Storylet.IntChange>();
			floatChanges = new List<Storylet.ValueChange>();
			boolChanges = new List<Storylet.StateChange>();
		}
	}


	/// <summary>
	/// Resolves the current event with the specified Party Sheet
	/// </summary>
	/// <param name="adventurers">The party sheet that this event will resolve with.</param>
	/// <returns></returns>
	public EventCase resolveEvent(PartySheet adventurers)
	{
		bool foundEvent = false;
		EventCase nextEvent = null;

		// Loops through every single item in EventCase to check if there's a valid value.
		foreach (EventCase eventToCheck in eventCases)
		{
			bool validEvent = true;

			// This chunk loops through the party checks.
			foreach(StatCheck i in eventToCheck.statTriggers)
			{
				if (!Storylet.SignEvaluator(adventurers.getStatSummed(i.stat), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Next all the Trigger Ints.
			foreach(Storylet.TriggerInt i in eventToCheck.intTriggers)
			{
				if (!Storylet.SignEvaluator(theWorld.GetWorldInt(i.name), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Then the Trigger Floats.
			foreach (Storylet.TriggerValue i in eventToCheck.floatTriggers)
			{
				if (!Storylet.SignEvaluator(theWorld.GetWorldValue(i.name), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Then finally the Trigger Bools.
			foreach (Storylet.TriggerState i in eventToCheck.boolTriggers)
			{
				if (theWorld.GetWorldState(i.name) != i.state) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			foreach (PartyCheck i in eventToCheck.partyTriggers)
			{
				bool foundcharacter = false;
				foreach(CharacterSheet adventurer in adventurers.Party_Members)
				{
					// Check to see if they're here.
					if(adventurer.name == i.character)
					{
						foundcharacter = true; break;
					}
				}
				if(foundcharacter != i.present) { validEvent = false; }
			}
			if (!validEvent) { continue; }

			nextEvent = eventToCheck;
			foundEvent = true;
			break;
		}

		// If the previous loop doesn't find anything.
		if (!foundEvent)
		{
			nextEvent = defaultCase;
		}

		return nextEvent;
	}
}
