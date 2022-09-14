using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	public string questDescription { get; private set; } // What the description of the quest is.
	public string faction; // Who's the faction that's giving it.
	public string questGiver; // Who's giving the quest
	public QuestState currentState; // The current phase the quest is in: WAITING, ADVENTURING, DONE
	public string currentLocation;

	private EventNode headConnection; // Tells the graph where the head is going to be.
	public EventNode currentConnection { get; private set; } // Used during the course of execution to update what the current event is.
	private EventNode.EventCase nextConnection; // What we use to tell us what to do before proceeding on the quest.
	public PartySheet adventuring_party { get; private set; }// Reference to the adventuring party attached to the quest.
	public ReadOnlyCollection<CharacterSheet> PartyMembers { get { return adventuring_party.Party_Members; } }

	public bool isActive = false; // isactive? for all to see and set i guess.
	public bool isComplete = false; // iscomplete? for all to see and set i guess.

	public int timeUntilProgression { get; private set; } // How much time this questsheet will wait until it progresses. Start at 0.
	public int eventTicksElapsed { get; private set; } // Tracks how many ticks has elapsed for an ADVENTURING quest and executes events appropriatly.
	public float timeToExpire { get; private set; } // How much time until a WAITING quest will auto-reject
	public float expirationTimer { get; private set; } // Tracks how many ticks have passed for the expiration timer
	public int totalTimeToComplete { get; private set; }
	public int restingPeriod { get; private set; } // How many hours adventurers will be resting for after this quest is completed

	public int accumulatedGold { get; private set; } // How much gold has been accumulated from the events.
	public int totalGold { get; private set; }

	private WorldStateManager worldStateManager;

	public List<EventNode> visitedNodes;

	public string questRecap { get; private set; }

	//Changers for when the quest is rejected
	public List<Storylet.IntChange> intChanges;
	public List<Storylet.ValueChange> floatChanges;
	public List<Storylet.StateChange> boolChanges;

	/// <summary>
	/// QuestSheet Constructor
	/// </summary>
	/// <param name="connection_input">Head of the event graph</param>
	/// <param name="name_Input">Name of the Quest</param>
	public QuestSheet(EventNode connection_input, string name_Input, WorldStateManager inputWorld, string inputQuestDescription = "")
	{
		Debug.Log($"Attempted to make a quest for {name_Input}");

		//Initialize our reference
		adventuring_party = null;

		visitedNodes = new List<EventNode>();

		// Set our connections right.
		headConnection = connection_input;
		currentConnection = headConnection;
		nextConnection = null;
		visitedNodes.Add(currentConnection);
		worldStateManager = inputWorld;
		Debug.Assert(worldStateManager != null);

		// Initialize our tracking variables.
		eventTicksElapsed = 0;
		timeUntilProgression = 0;
		totalTimeToComplete = EstimatedTimeToComplete(headConnection);
		accumulatedGold = 0;
		totalGold = 0;

		currentState = QuestState.WAITING;

		//placing a temp value for testing, will need to convert from hours to ticks later
		timeToExpire = 25;
		expirationTimer = 0;

		// Initialize out descriptor variables.
		questName = name_Input;
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
	/// <returns>0 if the quest is waiting or in progress. 1 if the quest is complete. 2 if the quest was rejected.</returns>
	public int advancebyTick()
	{
		int returnVal = 0;
        switch (currentState)
		{
			case QuestState.WAITING:
				//returnVal = AdvanceWaitingQuest();
				returnVal = 0;
				break;
			case QuestState.ADVENTURING:
				returnVal = AdvanceActiveQuest();
				break;
			default:
				returnVal = 1;
				break;
		}

		return returnVal;
	}

	private int AdvanceWaitingQuest()
    {
		if(expirationTimer >= timeToExpire) 
		{
			//code to reject quest
			foreach (Storylet.IntChange change in intChanges) { worldStateManager.ChangeWorldInt(change.name, change.value, change.set); }
			foreach (Storylet.StateChange change in boolChanges) { worldStateManager.ChangeWorldState(change.name, change.state); }
			foreach (Storylet.ValueChange change in floatChanges) { worldStateManager.ChangeWorldValue(change.name, change.value, change.set); }
			//test change values
			worldStateManager.ChangeWorldInt("Rejection test", 1, false);
			Debug.Log($"Rejection test now " + worldStateManager.GetWorldInt("Rejection test"));
			return 2;
		}

		expirationTimer++;
		return 0;
    }

	private int AdvanceActiveQuest()
    {
		// Base Case, check if we've reached the end. Does things if it has.
		if (eventTicksElapsed >= timeUntilProgression)
		{
			// Reset the event tick timer.
			eventTicksElapsed = 0;

			if (nextConnection != null)
			{
				// Add everything specified by the Event Case
				accumulatedGold += nextConnection.reward;
				adventuring_party.UpdateRelationshipStory(UpdatePartyRelationships(adventuring_party, nextConnection.bondupdate));
				visitedNodes.Add(currentConnection);
				questRecap += currentConnection.description + " " + nextConnection.progressionDescription + " ";

				// Update the world values according to the triggers.
				foreach (Storylet.IntChange change in nextConnection.intChanges) { worldStateManager.ChangeWorldInt(change.name, change.value, change.set); }
				foreach (Storylet.StateChange change in nextConnection.boolChanges) { worldStateManager.ChangeWorldState(change.name, change.state); }
				foreach (Storylet.ValueChange change in nextConnection.floatChanges) { worldStateManager.ChangeWorldValue(change.name, change.value, change.set); }

				// End the quest if we hit a null.
				if (nextConnection.nextNode != null)
				{
					currentConnection = nextConnection.nextNode;
					totalTimeToComplete = EstimatedTimeToComplete(currentConnection);
				}
				else
				{
					return 1;
				}
			}

			// Request the next event package.
			nextConnection = currentConnection.resolveEvent(adventuring_party);
			timeUntilProgression = nextConnection.time;
		}
		eventTicksElapsed++;

		return 0;
	}

	/// <summary>
	/// Adds the accumulated gold to the world manager, then clears the sheet's accumulated gold.
	/// </summary>
	public void AddGuildGold()
	{
        // TODO WorldInt Gold
        GameObject.Find("GuildManager").GetComponent<GuildManager>().Gold += accumulatedGold;
		//worldStateManager.ChangeWorldInt("PlayerGold",accumutatedGold);
		GameObject.Find("RecapDisplay").GetComponent<RecapManager>().AddDayGold(accumulatedGold);
		totalGold += accumulatedGold;
		accumulatedGold = 0;
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

	public int EstimatedTimeToComplete(EventNode node)
    {
		return MaxTime(node);
    }

	private int MaxTime(EventNode currentNode)
	{
		int totalTime = 0;

		foreach (EventNode.EventCase eCase in currentConnection.eventCases)
		{
			int nodeTime = eCase.time;
			if (eCase.nextNode != null) { nodeTime += MaxReward(eCase.nextNode); }
			if (nodeTime > totalTime) { totalTime = nodeTime; }
		}

		return totalTime;
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
			foreach (EventNode.StatCheck statCheck in eCase.statTriggers)
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

	public bool IsQuestActive()
    {
		return currentState == QuestState.ADVENTURING;	
    }
}