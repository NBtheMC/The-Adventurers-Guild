using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	public string questName { get; private set; }
	private EventNode headConnection; // Tells the graph where the head is going to be.
	private EventNode currentConnection; // Used during the course of execution to update what the current event is.
	private PartySheet adventuring_party; // Reference to the adventuring party attached to the quest.
	public QuestingManager questingManager; // reference to the master questing manager.

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

	public void assignParty(PartySheet party_input)
	{
		adventuring_party = party_input;
	}

	// Calculates the maximum total reward for the quest as given.
	// Done by doing brute-force search through the tree and calculating all possible rewards.
	public int EstimatedRedwardTotal()
	{
		int countingTotal = 0;
		// Put code here.
		return countingTotal;
	}

	// Calls on the current event to see what's going on. May update things as neccessary.
	public void advancebyTick()
	{
		int returnCode = currentConnection.timeCheck(eventTicksElapsed, adventuring_party.getStatSummed(currentConnection.stat), ref currentConnection);
		// Now we decipher those return codes.
		switch (returnCode)
		{
			case 1:
				//insert event code here.
				break;
			case 2:
				// insert success code here.
				break;
			case 3:
				// insert reward collection code here.
				break;
			case 0:
			default:
				eventTicksElapsed++;
				break;
		}
	}
}

/// <summary>
/// The default event node. Inherited by all other event nodes.
/// Check against a DC.
/// If successful, sends first node.
/// If failed, sends second node.
/// </summary>
public class EventNode
{
	public string stat { get; protected set; } // the stat to be checked against. Should correspond with PartySheet
	public int DC { get; protected set; }
	protected List<EventNode> connection; // A list of connections to get used.
	public int time { get; protected set; } // How many ticks before the DC check is triggered
	public int goldReward; //How much gold if event is completed.


	//Constructors
	public EventNode()
	{
		stat = "";
		DC = 0;
		time = 0;
		connection = null;
	}
	public EventNode(string stat_Input, int DC_input, int time_input)
	{
		stat = stat_Input;
		DC = DC_input;
		time = time_input;
		connection = null;
	}

	public void addConnection(EventNode connection_input, int index = -1)
	{
		// If there is no desired input index.
		if(index == -1){ connection.Add(connection_input);}
		else { connection.Insert(index,connection_input); }
	}

	/// <summary>
	/// Checks if it's time to execute an event. If it is, executes event.
	/// </summary>
	/// <param name="currentTick">How much time elapsed since this event started.</param>
	/// <param name="input_DC">The calculated DC of the specified party.</param>
	/// <param name="currentNode">A reference to the current node to change if neccessary</param>
	/// <returns>
	/// 0 for no trigger, event still ongoing.
	/// 1 for quest failed.
	/// 2 for quest success.
	/// 3 for event succes, new currentNode.
	/// </returns>
	public int timeCheck(int currentTick, int input_DC, ref EventNode currentNode)
	{
		if (currentTick >= time)
		{
			currentNode = nextConnection(input_DC);
			return 3;
		}
		else
		{
			return 0;
		}
	}

	/// <summary>
	/// Returns the next connection according to a DC check.
	/// </summary>
	/// <param name="input_DC">The value to be checked against</param>
	/// <returns>The next connection, depending on success or failure.</returns>
	protected EventNode nextConnection(int input_DC)
	{
		if (connection.Count < 2)
		{
			Debug.Log("Not enough connections in default event node");
		}

		// If there are enough connections
		if (input_DC > DC) { return connection[0]; }
		else { return connection[1]; }
	}
}

/// <summary>
/// Returns 1 on all timeChecks, no matter what.
/// </summary>
public class FailEventNode: EventNode
{
	public new int timeCheck(int currentTick, int input_DC, ref EventNode currentNode)
	{
		return 1;
	}
}

/// <summary>
/// Returns 2 on all timeChecks, no matter what.
/// </summary>
public class SuccessEventNode: EventNode
{
	public new int timeCheck(int currentTick, int input_DC, ref EventNode eventNode)
	{
		return 2;
	}
}

/// <summary>
/// The most common event node, used for events that must succeed in order to progress.
/// </summary>
public class LinearEventNode : EventNode
{
	new protected EventNode nextConnection(int input_DC)
	{
		// If there are enough connections
		if (input_DC > DC) { return connection[0]; }
		else { return new FailEventNode(); }
	}
}