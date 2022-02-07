using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default event node. Inherited by all other event nodes.
/// Check against a DC.
/// If successful, sends first node.
/// If failed, sends second node.
/// </summary>
[CreateAssetMenu(fileName = "NewEvent",menuName = "EventNode", order = 1)]
public class EventNode: ScriptableObject
{
	public string description; //what the event is

	public string stat; // the stat to be checked against. Should correspond with PartySheet
	public int DC;
	public int time; // How many ticks before the DC check is triggered
	public int Reward;

	public EventNode successNode;
	public string successString;
	public EventNode failureNode;
	public string failureString;
	public string resultsString; //what actually happened

	public class EventPackage
	{
		public bool objectiveComplete = false;
		public int givenReward = 0;
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
	}

	public EventPackage resolveEvent(PartySheet adventurers)
	{
		EventPackage message = new EventPackage();
		message.objectiveComplete = adventurers.getStatSummed(stat) > DC;

		// switchcase for our checks.
		switch (message.objectiveComplete)
		{
			case true:
				message.nextEvent = successNode;
				message.givenReward = Reward;
				resultsString = successString;
				//change world state?
				break;
			case false:
				message.nextEvent = failureNode;
				resultsString = failureString;
				//change world state?
				break;
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
