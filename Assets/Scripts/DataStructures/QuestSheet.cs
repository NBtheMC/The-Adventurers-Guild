using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	public string questDescription { get; private set; } // What the description of the quest is.
	private EventNode headConnection; // Tells the graph where the head is going to be.
	private EventNode currentConnection; // Used during the course of execution to update what the current event is.
	private PartySheet adventuring_party; // Reference to the adventuring party attached to the quest.
	public int partySize = 4;
	public bool isActive = false;
	public bool isComplete = false;

	private int eventTicksElapsed; // Tracks how many ticks has elapsed and executes events appropriatly.
	public bool QuestComplete { get; private set; } // Indicator for QuestingManager to see if the quest is done.
	public int accumutatedGold { get; private set; } // How much gold has been accumulated from the events.

	private WorldStateManager worldStateManager;

	public List<EventNode> visitedNodes;

	public IReadOnlyCollection<CharacterSheet> PartyMembers { get { return adventuring_party.Party_Members; } }
	public string questRecap;

	/// <summary>
	/// QuestSheet Constructor
	/// </summary>
	/// <param name="connection_input">Head of the event graph</param>
	/// <param name="name_Input">Name of the Quest</param>
	public QuestSheet(EventNode connection_input, string name_Input, WorldStateManager inputWorld, string inputQuestDescription = "")
	{
		headConnection = connection_input;
		currentConnection = headConnection;
		eventTicksElapsed = 0;
		questName = name_Input;
		QuestComplete = false;
		accumutatedGold = 0;
		questDescription = inputQuestDescription;
		worldStateManager = inputWorld;
		Debug.Assert(worldStateManager != null);

        visitedNodes = new List<EventNode>();
		questRecap = "";
	}

	/// <summary>
	/// Assigns a part to the quest.
	/// </summary>
	/// <param name="party_input">The party to assign to the quest.</param>
	public void assignParty(PartySheet partyInput)
	{
		adventuring_party = partyInput;
	}

	/// <summary>
	/// Calls on the current event to see what's going on.
	/// </summary>
	/// <returns>A 0 if the quest is still ongoing. A 1 if the quest is complete.</returns>
	public int advancebyTick()
	{
		if (eventTicksElapsed >= currentConnection.time)
		{
			// Reset the event tick timer.
			eventTicksElapsed = 0;

			// Request the event package.
			EventNode.EventPackage returnMessage = currentConnection.resolveEvent(adventuring_party);
			accumutatedGold += returnMessage.givenReward;
			adventuring_party.UpdateRelationshipStory(returnMessage.relationshipsUpdate);
            visitedNodes.Add(currentConnection);
			questRecap += (returnMessage.resultsString + " ");

			// Changes the world based on Event Package
			switch (returnMessage.objectiveComplete)
			{
				case true:
					foreach (Storylet.IntChange change in currentConnection.successIntChange)
					{
						Debug.Log($"{change.name} changed {change.value}");
						worldStateManager.ChangeWorldInt(change.name, change.value, change.set);
					}
					foreach (Storylet.StateChange change in currentConnection.successStateChange)
					{
						Debug.Log($"{change.name} changed {change.state}");
						Debug.Log($"WorldStateManager Exists: {worldStateManager != null}");
						worldStateManager.ChangeWorldState(change.name, change.state);
					}
					foreach (Storylet.ValueChange change in currentConnection.successValueChange)
					{
						Debug.Log($"{change.name} changed {change.value}");
						worldStateManager.ChangeWorldValue(change.name, change.value, change.set);
					}
					break;
				case false:
					foreach (Storylet.IntChange change in currentConnection.failIntChange) { worldStateManager.ChangeWorldInt(change.name, change.value, change.set); }
					foreach (Storylet.StateChange change in currentConnection.failStateChange) { worldStateManager.ChangeWorldState(change.name, change.state); }
					foreach (Storylet.ValueChange change in currentConnection.failValueChange) { worldStateManager.ChangeWorldValue(change.name, change.value, change.set); }
					break;
			}

			// Progress to the next event.
			if (returnMessage.nextEvent != null)
			{
				currentConnection = returnMessage.nextEvent;
				//TODO: add onto corresponding quest UI object
				// return advancebyTick();
			}
			else
			{
				QuestComplete = true;
				//TODO: add finished node onto UI object
				return 1;
			}
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
		public string stat; //type of check that is done
		public int DC; //requirement
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
		currentEvent.stat = currentConnection.stat.ToString();
		currentEvent.DC = currentConnection.DC;
		return currentEvent;
	}

	public int EstimatedRewardTotal(){
		return MaxReward(headConnection, 0);
	}

	// Calculates the reward if party succeeds. change later
	private int MaxReward(EventNode currentNode, int previousTotal)
	{
		if(currentNode == null){
			return previousTotal;
		}
		int countingTotal = previousTotal + currentNode.Reward;
		return MaxReward(currentNode.successNode, countingTotal);
	}

	public string GetQuestRecap(){
		return questRecap;
	}


	/// <summary>
	/// Used to calculate what the maxes and mins of each event are through a recursive search of the graph.
	/// </summary>
	public int CalcualteNodeRanges(CharacterSheet.StatDescriptors inputType, EventNode topConnection = null)
	{
		int returnValue = 0;
		int successValue = 0;
		int failValue = 0;

		if (topConnection == null){topConnection = headConnection;} // Checks to see if there is a topConnection. Sets the head connection if there isn't.

		// Get the higest of the following nodes, if they exist
		if (topConnection.successNode != null) { successValue = CalcualteNodeRanges(inputType, topConnection.successNode); } 
		if (topConnection.failureNode != null) { failValue = CalcualteNodeRanges(inputType, topConnection.failureNode); }

		// Gets the current value as well. If it's of the current type, of course.
		if (topConnection.stat == inputType) { returnValue = topConnection.DC; }

		if (successValue > returnValue) { returnValue = successValue; }
		if (failValue > returnValue) { returnValue = failValue; }

		return returnValue;
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
}