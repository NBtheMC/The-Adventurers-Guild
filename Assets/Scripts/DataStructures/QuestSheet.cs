﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	private EventNode headConnection; // Tells the graph where the head is going to be.
	private EventNode currentConnection; // Used during the course of execution to update what the current event is.
	private PartySheet adventuring_party; // Reference to the adventuring party attached to the quest.

	private int eventTicksElapsed; // Tracks how many ticks has elapsed and executes events appropriatly.
	public bool QuestComplete { get; private set; } // Indicator for QuestingManager to see if the quest is done.
	public int accumutatedGold { get; private set; } // How much gold has been accumulated from the events.

	/// <summary>
	/// QuestSheet Constructor
	/// </summary>
	/// <param name="connection_input">Head of the event graph</param>
	/// <param name="name_Input">Name of the Quest</param>
	public QuestSheet(EventNode connection_input, string name_Input)
	{
		headConnection = connection_input;
		currentConnection = headConnection;
		eventTicksElapsed = 0;
		questName = name_Input;
		QuestComplete = false;
		accumutatedGold = 0;
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
	public int EstimatedRedwardTotal()
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

			// Progress to the next event.
			if (returnMessage.nextEvent != null)
			{
				currentConnection = returnMessage.nextEvent;
				return advancebyTick();
			}
			else
			{
				QuestComplete = true;
				return 1;
			}
		}
		eventTicksElapsed++;

		return 0;
	}
}

