using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The event node checks between a large number of conditions.
/// Sends back an package full of information for Quest Sheet to use, specifically what the next node is.
/// Also update bonds.
/// </summary>
[CreateAssetMenu(fileName = "NewEvent",menuName = "EventNode", order = 1)]
public class EventNode: ScriptableObject
{
	[TextAreaAttribute(2, 10)]
	public string description; //what the event is

	public CharacterSheet.StatDescriptors stat; // the stat to be checked against. Should correspond with PartySheet

	public List<EventCase> EventCases;

	public EventCase defaultCase; // The node to go to in case all eventNodes fail.

	public WorldStateManager theWorld;

	private void Start()
	{
		theWorld = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
		Debug.Assert(theWorld != null);
	}

	private List<string> eventRelationships = new List<string>();

	public class EventPackage
	{
		public int timeToCompletion;
		public int givenReward = 0;
		public EventNode nextEvent = null;
		public List<string> relationshipsUpdate = new List<string>(); //relationships update
		public string resultsString; //what actually happened
		//TODO Adventurer levelling
	}

	/// <summary>
	/// For use in EventCase, checking against the party's stats.
	/// </summary>
	public struct PartyCheck { public CharacterSheet.StatDescriptors stat; public Storylet.NumberTriggerType triggerType; public int value; }
	
	/// <summary>
	/// Used to specify a case for use in this event.
	/// </summary>
	public class EventCase
	{
		// All the specific details of going down this event.
		public EventNode nextNode = null;
		public int time = 0;
		public int reward = 0;
		public int bondupdate = 0;
		public string progressionDescription;

		// All our triggers.
		public List<PartyCheck> statTriggers;
		public List<Storylet.TriggerInt> IntTriggers;
		public List<Storylet.TriggerValue> FloatTriggers;
		public List<Storylet.TriggerState> BoolTriggers;

		// All the changes upon entering this event.
		public List<Storylet.IntChange> IntChanges;
		public List<Storylet.ValueChange> FloatChanges;
		public List<Storylet.StateChange> BoolChanges;
	}


	/// <summary>
	/// Resolves the current event with the specified Party Sheet
	/// </summary>
	/// <param name="adventurers">The party sheet that this event will resolve with.</param>
	/// <returns></returns>
	public EventPackage resolveEvent(PartySheet adventurers)
	{
		bool foundEvent = false;
		EventCase nextEvent = null;

		// Loops through every single item in EventCase to check if there's a valid value.
		foreach (EventCase eventToCheck in EventCases)
		{
			bool validEvent = true;
			
			// This chunk loops through the party checks.
			foreach(PartyCheck i in eventToCheck.statTriggers)
			{
				if (!Storylet.SignEvaluator(adventurers.getStatSummed(i.stat), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Next all the Trigger Ints.
			foreach(Storylet.TriggerInt i in eventToCheck.IntTriggers)
			{
				if (!Storylet.SignEvaluator(theWorld.GetWorldInt(i.name), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Then the Trigger Floats.
			foreach (Storylet.TriggerValue i in eventToCheck.FloatTriggers)
			{
				if (!Storylet.SignEvaluator(theWorld.GetWorldValue(i.name), i.triggerType, i.value)) { validEvent = false; break; }
			}
			if (!validEvent) { continue; }

			// Then finally the Trigger Bools.
			foreach (Storylet.TriggerState i in eventToCheck.BoolTriggers)
			{
				if (theWorld.GetWorldState(i.name) != i.state) { validEvent = false; break; }
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

		EventPackage message = new EventPackage();
		message.nextEvent = nextEvent.nextNode;
		message.givenReward = nextEvent.reward;
		message.resultsString = nextEvent.progressionDescription;
		message.timeToCompletion = nextEvent.time;
		message.relationshipsUpdate = UpdatePartyRelationships(adventurers, nextEvent.bondupdate);

		return message;
	}

	//called first by quest when quest is done. updates friendships based on win or loss
    //done on current party, change is determined by quest
    private List<string> UpdatePartyRelationships(PartySheet party, int change){
		List<string> partyUpdates = new List<string>();

        //IReadOnlyCollection<CharacterSheet> partyMembersSheets = party.Party_Members;
        List<Adventurer> partyMembers = new List<Adventurer>();

        foreach(CharacterSheet a in party.Party_Members){
			//Debug.Log(a.name);
            partyMembers.Add(a.adventurer);
			//Debug.Log(a.adventurer.characterSheet.name);
        }

        //Actual updating
        for(int i  = 0; i < partyMembers.Count; i++){
            Adventurer a = partyMembers[i];
			Debug.Log(a.characterSheet.name);
            for(int j  = i+1; j < partyMembers.Count; j++){

                Adventurer b = partyMembers[j];
                //update friendship between a and b
                a.ChangeFriendship(b, change);
                b.ChangeFriendship(a, change); //do if we want to handle relationships pretty much completely here
                //get string based on change
				partyUpdates.Add(a.characterSheet.name + " and " + b.characterSheet.name + " did thing");
            }
        }
		return partyUpdates;
    }

	
}
