using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	private EventNode headConnection; // Tells the graph where the head is going to be.
	private EventNode currentConnection; // Used during the course of execution to update what the current event is.
	private PartySheet adventuring_party; // Reference to the adventuring party attached to the quest.
	private QuestUI questUI; //association to corresponding UI element
	public int partySize { get; private set; } //how many adventurers can be assigned to a quest

	[HideInInspector] public bool isDisplayed = false; //has this quest sheet been displayed to the user

	private int eventTicksElapsed; // Tracks how many ticks has elapsed and executes events appropriatly.
	public bool QuestComplete { get; private set; } // Indicator for QuestingManager to see if the quest is done.
	public int accumutatedGold { get; private set; } // How much gold has been accumulated from the events.


    public List<EventNode> visitedNodes;


	/// <summary>
	/// QuestSheet Constructor
	/// </summary>
	/// <param name="connection_input">Head of the event graph</param>
	/// <param name="name_Input">Name of the Quest</param>
	public QuestSheet(EventNode connection_input, string name_Input, int partySize_input = 4)
	{
		headConnection = connection_input;
		currentConnection = headConnection;
		eventTicksElapsed = 0;
		questName = name_Input;
		QuestComplete = false;
		accumutatedGold = 0;
		partySize = partySize_input;

        visitedNodes = new List<EventNode>();
	}

	/// <summary>
	/// Assigns a part to the quest.
	/// </summary>
	/// <param name="party_input">The party to assign to the quest.</param>
	public void assignParty(PartySheet partyInput)
	{
		adventuring_party = partyInput;
	}

	// Calculates the maximum total reward for the quest as given.
	// Done by doing brute-force search through the tree and calculating all possible rewards.
	public int EstimatedRewardTotal()
	{
		int countingTotal = 0;
		// Put code here.
		return countingTotal;
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
            visitedNodes.Add(currentConnection);

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
		currentEvent.description = "Event description"; //placeholder text
		currentEvent.stat = currentConnection.stat;
		currentEvent.DC = currentConnection.DC;
		return currentEvent;
	}

	//all the stuff a quest needs to show
	//used at start and end of quest
	public struct GivenQuestInfo
	{
		string name; //stays constant, name of storylet probably
		List<EventInfo> events; //either list of predicted storylets or completed ones
		// consequences (positive or negative)
			//based off difference between stat of party and stat of quest 
			//includes adventurers dying, being op, being weak, etc
		// insert relationships
		string description;

	}

	public struct FinishedQuestInfo{
		string name; //stays constant, name of storylet probably
		List<EventInfo> events; //either list of predicted storylets or completed ones
	}

	//quest start info
	public QuestInfo QuestStartInfo(){
		return new QuestInfo();
	}

	//quest done info
	public QuestInfo QuestDoneInfo(){
		return new QuestInfo();
	}
}