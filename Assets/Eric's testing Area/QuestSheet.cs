using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSheet
{
	private EventNode headConnection;
	private PartySheet adventuring_party;
	public QuestSheet(EventNode connection_input)
	{
		headConnection = connection_input;
	}

	public void assignParty(PartySheet party_input)
	{
		adventuring_party = party_input;
	}
	
	// Calculates the maximum total reward for the quest as given.
	// Done by doing exhaustive recursive search through the tree and calculating all rewards.
	public int rewardTotal()
	{
		int countingTotal = 0;

		return countingTotal;
	}
}

public class EventNode
{
	public string stat { get; protected set; }
	public int DC { get; protected set; }
	protected EventNode connection;
	public int time { get; protected set; }

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
}

/// <summary>
/// This is for Event nodes that have two connections, 1 for DC success, 1 for DC failure.
/// Currently unncessary for the linear quest progressions we're creating.
/// </summary>
public class FailsafeEventNode:EventNode {
	public FailsafeEventNode(string stat_Input, int DC_input, int time_input) : base(stat_Input, DC_input, time_input)
	{
	}
}
