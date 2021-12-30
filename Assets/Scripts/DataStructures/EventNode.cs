using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		head = 1, // Nodes without preconditions.
		successful = 2, // Nodes that need succesful DC check
		fail = 3, // Nodes that need failed DC check
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
		eventType = EventTypes.head;
	}

	/// <summary>
	/// Used to add additional connections to the event node.
	/// </summary>
	/// <param name="connection_input">The connection to be added.</param>
	/// <param name="index">The index at which to be added. Added to the end by default.</param>
	public void addConnection(EventNode connection_input, int index = -1)
	{
		// If there is no desired input index.
		if (index == -1) { connection.Add(connection_input); }
		else { connection.Insert(index, connection_input); }
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

	/*
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
	}*/
}
