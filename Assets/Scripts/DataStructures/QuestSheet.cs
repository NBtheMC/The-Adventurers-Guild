using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	public string questDescription { get; private set; } // What the description of the quest is.
	private EventNode headConnection; // Tells the graph where the head is going to be.
	private EventNode currentConnection; // Used during the course of execution to update what the current event is.
	private EventNode.EventCase nextConnection; // What we use to tell us what to do before proceeding on the quest.
	private PartySheet adventuring_party; // Reference to the adventuring party attached to the quest.

	public int partySize = 4;

	private int timeUntilProgression; // How much time this questsheet will wait until it progresses. Start at 0.
	private int eventTicksElapsed; // Tracks how many ticks has elapsed and executes events appropriatly.
	public bool QuestComplete { get; private set; } // Indicator for QuestingManager to see if the quest is done.
	public int accumutatedGold { get; private set; } // How much gold has been accumulated from the events.

	private WorldStateManager worldStateManager;

	public List<EventNode> visitedNodes;

	public IReadOnlyCollection<CharacterSheet> PartyMembers { get { return adventuring_party.Party_Members; } }
	public string questRecap { get; private set; }

	/// <summary>
	/// QuestSheet Constructor
	/// </summary>
	/// <param name="connection_input">Head of the event graph</param>
	/// <param name="name_Input">Name of the Quest</param>
	public QuestSheet(EventNode connection_input, string name_Input, WorldStateManager inputWorld, string inputQuestDescription = "")
	{
		// Set our connections right.
		headConnection = connection_input;
		currentConnection = headConnection;
		nextConnection = currentConnection.resolveEvent(adventuring_party);
		visitedNodes.Add(currentConnection);
		worldStateManager = inputWorld;
		Debug.Assert(worldStateManager != null);

		// Initialize our tracking variables.
		eventTicksElapsed = 0;
		timeUntilProgression = nextConnection.time;
		accumutatedGold = 0;

		// Initialize out descriptor variables.
		questName = name_Input;
		QuestComplete = false;
		questDescription = inputQuestDescription;

        visitedNodes = new List<EventNode>();
		questRecap = "";
	}

	/// <summary>
	/// Assigns a party to the quest.
	/// </summary>
	/// <param name="party_input">The party to assign to the quest.</param>
	public void assignParty(PartySheet partyInput)
	{
		adventuring_party = partyInput;
	}

	/// <summary>
	/// This function is used by Questing Manager to control the flow of a quest as acording to ticks.
	/// 
	/// </summary>
	/// <returns>A 0 if the quest is still ongoing. A 1 if the quest is complete.</returns>
	public int advancebyTick()
	{
		// Base Case, check if we've reached the end. Does things if it has.
		if (eventTicksElapsed >= timeUntilProgression)
		{
			// Reset the event tick timer.
			eventTicksElapsed = 0;

			// Add everything specified by the Event Case
			accumutatedGold += nextConnection.reward;
			adventuring_party.UpdateRelationshipStory(UpdatePartyRelationships(adventuring_party, nextConnection.bondupdate));
			visitedNodes.Add(currentConnection);
			questRecap += nextConnection.progressionDescription + "";

			// Update the world values according to the triggers.
			foreach (Storylet.IntChange change in nextConnection.IntChanges) { worldStateManager.ChangeWorldInt(change.name, change.value, change.set); }
			foreach (Storylet.StateChange change in nextConnection.BoolChanges) { worldStateManager.ChangeWorldState(change.name, change.state); }
			foreach (Storylet.ValueChange change in nextConnection.FloatChanges) { worldStateManager.ChangeWorldValue(change.name, change.value, change.set); }

			// End the quest if we hit a null.
			if (nextConnection.nextNode != null)
			{
				currentConnection = nextConnection.nextNode;
			}
			else
			{
				QuestComplete = true;
				return 1;
			}

			// Request the next event package.
			nextConnection = currentConnection.resolveEvent(adventuring_party);
		}
		eventTicksElapsed++;

		return 0;
	}

	/// <summary>
	/// Adds the accumulated gold to the world manager, then clears the sheet's accumulated gold.
	/// </summary>
	public void AddGuildGold()
	{
		worldStateManager.ChangeWorldInt("PlayerGold",accumutatedGold);
		accumutatedGold = 0;
	}

	public struct EventInfo
	{
		public string description; //not currently implemented yet, but events will need to have this soon
	}

	/// <summary>
	/// Sends list of relevant event data to QuestUI object
	/// Right now, that's just the description (needs to be added onto EventNode somehow), stat, and difficulty
	/// Will want to call this whenever the next one gets revealed
	/// </summary>
	public EventInfo getNewEventInfo()
	{
		EventInfo currentEvent;
		currentEvent.description = currentConnection.description; //placeholder text
		return currentEvent;
	}

	public int EstimatedRewardTotal(){
		return MaxReward(headConnection);
	}

	// Calculates through a recursive tree through the entire damn thing.
	private int MaxReward(EventNode currentNode)
	{
		int totalRewards = 0;

		foreach(EventNode.EventCase eCase in currentConnection.eventCases)
		{
			int nodeGold = eCase.reward;
			if (eCase.nextNode != null) { nodeGold += MaxReward(eCase.nextNode); }
			if (nodeGold > totalRewards) { totalRewards = nodeGold; }
		}

		return totalRewards;
	}

	public string GetQuestRecap(){
		return questRecap;
	}


	/// <summary>
	/// Used to calculate what the maxs are of each event are through a recursive search of the graph.
	/// </summary>
	public int CalcualteNodeRanges(CharacterSheet.StatDescriptors inputType, EventNode topConnection = null)
	{
		int currentHighest = 0;

		if (topConnection == null){topConnection = headConnection;} // Checks to see if there is a topConnection. Sets the head connection if there isn't.

		// Make a list of all the cases.

		// Search through all the items in default node cases
		foreach (EventNode.EventCase eCase in topConnection.eventCases)
		{
			int tempHigh = 0;
			if (eCase.nextNode != null) { tempHigh = CalcualteNodeRanges(inputType, eCase.nextNode); }
			if (tempHigh > currentHighest) { currentHighest = tempHigh; }
			foreach (EventNode.PartyCheck statCheck in eCase.statTriggers)
			{
				if(statCheck.stat == inputType && statCheck.value > currentHighest)
				{
					currentHighest = statCheck.value;
				}
			}
		}

		return currentHighest;
	}

	// public string GenerateEventText(){
	// 	string eventResults = "";
	// 	foreach(EventNode e in visitedNodes){
	// 		eventResults += (" " + e.resultsString);
	// 	}
	// 	return eventResults;
	// }

	// all the stuff a quest needs to show
	// used at start and end of quest
	// public struct GivenQuestInfo
	// {
	// 	string name; //stays constant, name of storylet probably
	// 	List<EventInfo> events; //list of predicted storylets
	// 	int predictedHighReward;		
	// 	string description;
	// }

	// public struct FinishedQuestInfo{
	// 	string name; //stays constant, name of storylet probably
	// 	List<EventInfo> events; //List of completed events
	// 	// consequences (positive or negative)
	// 		//based off difference between stat of party and stat of quest 
	// 		//includes adventurers dying, being op, being weak, etc
	// 		//concatenates based on each event
	// 	RelationshipManager.RelationshipsInfo partyRelationships;
	// 	int reward;
	// }

	// quest start info
	// public GivenQuestInfo QuestStartInfo(){
	// 	GivenQuestInfo questStart  = new GivenQuestInfo();

	// 	return questStart;
	// }

	// quest done info
	// public FinishedQuestInfo QuestDoneInfo(){
	// 	FinishedQuestInfo questDone  = new FinishedQuestInfo();
	// 	questDone.events = visitedNodes;
	// 	return questDone;
	// }

	/// <summary>
	/// called first by quest when quest is done. updates friendships based on win or loss
	/// done on current party, change is determined by quest
	/// </summary>
	/// <param name="party">The PartySheet used to calculate and assign bond updates to</param>
	/// <param name="change">The amount of change specified by Event Case</param>
	/// <returns>A list of strings specifiying what happened.</returns>
	private List<string> UpdatePartyRelationships(PartySheet party, int change)
	{
		List<string> partyUpdates = new List<string>();

		//IReadOnlyCollection<CharacterSheet> partyMembersSheets = party.Party_Members;
		List<Adventurer> partyMembers = new List<Adventurer>();

		foreach (CharacterSheet a in party.Party_Members)
		{
			//Debug.Log(a.name);
			partyMembers.Add(a.adventurer);
			//Debug.Log(a.adventurer.characterSheet.name);
		}

		//Actual updating
		for (int i = 0; i < partyMembers.Count; i++)
		{
			Adventurer a = partyMembers[i];
			Debug.Log(a.characterSheet.name);
			for (int j = i + 1; j < partyMembers.Count; j++)
			{

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