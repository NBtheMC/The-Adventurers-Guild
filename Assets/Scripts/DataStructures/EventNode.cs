using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The default event node. Inherited by all other event nodes.
/// Check against a DC.
/// If successful, sends first node.
/// If failed, sends second node.
/// </summary>
[CreateAssetMenu(fileName = "NewEvent",menuName = "EventNode", order = 1)]
public class EventNode: ScriptableObject
{
	[TextAreaAttribute(2, 10)]
	public string description; //what the event is

	public CharacterSheet.StatDescriptors stat; // the stat to be checked against. Should correspond with PartySheet
	public int DC; //stat to be checked against also used for experience given eventually
	public int time; // How many ticks before the DC check is triggered
	public int Reward;

	// All the things that happen when we're successful.
	public EventNode successNode;
	public string successString;
	// List of all the items that should change if this is successful.
	public List<Storylet.IntChange> successIntChange;
	public List<Storylet.ValueChange> successValueChange;
	public List<Storylet.StateChange> successStateChange;


	// All the things that happen when we're not successful
	public EventNode failureNode;
	public string failureString;
	public List<Storylet.IntChange> failIntChange;
	public List<Storylet.ValueChange> failValueChange;
	public List<Storylet.StateChange> failStateChange;

	private WorldStateManager theWorld;

	private void Awake()
	{
		theWorld = GameObject.Find("WorldState").GetComponent<WorldStateManager>();
		Debug.Assert(theWorld != null);
	}

	private List<string> eventRelationships = new List<string>();

	public class EventPackage
	{
		public bool objectiveComplete = false;
		public int givenReward = 0;
		public EventNode nextEvent = null;
		public List<string> relationshipsUpdate = new List<string>(); //relationships update
		public string resultsString; //what actually happened
		//TODO Adventurer levelling
	}

	public EventPackage resolveEvent(PartySheet adventurers)
	{
		EventPackage message = new EventPackage();
		message.objectiveComplete = adventurers.getStatSummed(stat) > DC;
		Debug.Log($"Adventurer {adventurers.name}'s {stat} is {adventurers.getStatSummed(stat)}");

		// switchcase for our checks.
		switch (message.objectiveComplete)
		{
			case true:
				//update EventPackage
				message.nextEvent = successNode;
				message.givenReward = Reward;
				message.resultsString = description + " " + successString;
				message.relationshipsUpdate = UpdatePartyRelationships(adventurers, (int)Mathf.Ceil(DC/4)); //range from 1-5
				break;
			case false:
				//update EventPackage
				message.nextEvent = failureNode;
				message.resultsString = description + " " + failureString;
				message.relationshipsUpdate = UpdatePartyRelationships(adventurers, (int)Mathf.Floor(-DC/4)); //range from 1-5
				break;
		}


		return message;
	}

	//called first by quest when quest is done. updates friendships based on win or loss
    //done on current party, change is determined by quest
    private List<string> UpdatePartyRelationships(PartySheet party, int change){
		List<string> partyUpdates = new List<string>();

        //IReadOnlyCollection<CharacterSheet> partyMembersSheets = party.Party_Members;
        List<Adventurer> partyMembers = new List<Adventurer>();

        foreach(CharacterSheet a in party.Party_Members){
			//Debug.Log(a.name);
            partyMembers.Add(a.adventurer);
			//Debug.Log(a.adventurer.characterSheet.name);
        }

        //Actual updating
        for(int i  = 0; i < partyMembers.Count; i++){
            Adventurer a = partyMembers[i];
			Debug.Log(a.characterSheet.name);
            for(int j  = i+1; j < partyMembers.Count; j++){

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
