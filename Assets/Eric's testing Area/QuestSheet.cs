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

	/// <summary>
	/// Assigns a part to the quest.
	/// </summary>
	/// <param name="party_input">The party to assign to the quest.</param>
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
			else { return 1; }
		}
		eventTicksElapsed++;

		return 0;
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
	public string stat; // the stat to be checked against. Should correspond with PartySheet
	public int DC; 
	protected List<EventNode> connection; // A list of connections to get used.
	public int time; // How many ticks before the DC check is triggered
	public EventTypes eventType; // used to decide how EventNode will handle 

	/// <summary>
	/// Event Types describe what triggers them for use with the previous event.
	/// </summary>
	public enum EventTypes
	{
		head = 0, // Nodes without preconditions.
		successful = 1, // Nodes that need succesful DC check
		fail = 2, // Nodes that need failed DC check
	}

	public class EventPackage
	{
		public bool objectiveComplete = false;
		public EventNode nextEvent = null;
	}

	/// <summary>
	/// Default Constructor, sets everything to zero.
	/// </summary>
	public EventNode()
	{
		stat = "";
		DC = 0;
		time = 0;
		connection = new List<EventNode>();
		eventType = EventTypes.head;
	}

	/// <summary>
	/// Standard Constructor, used to initilize certain variables.
	/// </summary>
	/// <param name="statInput">Name of the stat to be checked against</param>
	/// <param name="DCInput">What the difficulty it.</param>
	/// <param name="timeInput">How much time to pass before checking.</param>
	public EventNode(string statInput, int DCInput, int timeInput)
	{
		stat = statInput;
		DC = DCInput;
		time = timeInput;
		connection = new List<EventNode>();
		eventType=EventTypes.head;
	}

	/// <summary>
	/// Used to add additional connections to the event node.
	/// </summary>
	/// <param name="connection_input">The connection to be added.</param>
	/// <param name="index">The index at which to be added. Added to the end by default.</param>
	public void addConnection(EventNode connection_input, int index = -1)
	{
		// If there is no desired input index.
		if(index == -1){ connection.Add(connection_input);}
		else { connection.Insert(index,connection_input); }
	}

	public EventPackage resolveEvent(PartySheet adventurers)
	{
		EventPackage message = new EventPackage();
		message.objectiveComplete = adventurers.getStatSummed(stat) > DC;

		// Goes through the entire list of connections, then checks them for whether any connection requirements are correct.
		foreach (EventNode nextNode in connection)
		{
			// switchcase for our checks.
			switch (nextNode.eventType)
			{
				case EventTypes.successful:
					if (message.objectiveComplete) { message.nextEvent = nextNode; }
					break;
				case EventTypes.fail:
					if (!message.objectiveComplete) { message.nextEvent = nextNode; }
					break;
				default: message.nextEvent = nextNode; break;
			}
		}


		return message;
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








/*
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
*/