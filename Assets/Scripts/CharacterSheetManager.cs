using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CharacterSheetManager : MonoBehaviour
{
    protected Dictionary<CharacterSheet, AdventurerState> adventurerStates; //represents state of advenutrers

    // These properties are used to access a read only list of adventurers
    public ReadOnlyCollection<CharacterSheet> UnhiredAdventurers { get { return GetAdventurersWithState(AdventurerState.UNHIRED); } }
    public ReadOnlyCollection<CharacterSheet> FreeAdventurers { get { return GetAdventurersWithState(AdventurerState.FREE); } }
    public ReadOnlyCollection<CharacterSheet> AssignedAdventurers { get { return GetAdventurersWithState(AdventurerState.ASSIGNED); } }
    public ReadOnlyCollection<CharacterSheet> QuestingAdventurers { get { return GetAdventurersWithState(AdventurerState.QUESTING); } }
    public ReadOnlyCollection<CharacterSheet> DeadAdventurers { get { return GetAdventurersWithState(AdventurerState.DEAD); } }
    public ReadOnlyCollection<CharacterSheet> AllAdventurers { get { return new List<CharacterSheet>(adventurerStates.Keys).AsReadOnly(); } }
    /// <summary>
    /// Returns read only list of all hired advneuters. In other words, adventures with the FREE, ASSIGNED, or QUESTING state
    /// </summary>
    /// <returns></returns>
    public ReadOnlyCollection<CharacterSheet> HiredAdventurers { get { return GetHiredAdventurers(); } }

    public event EventHandler<EventArgs> RosterChange;

    CharacterInitialStats[] characters;

    public virtual void Awake()
    {
        characters = Resources.LoadAll<CharacterInitialStats>("Characters");
        adventurerStates = new Dictionary<CharacterSheet, AdventurerState>();

        foreach (CharacterInitialStats character in characters)
        {
            CharacterSheet charSheet = new CharacterSheet(character);
            Debug.Log($"Added adventurer {charSheet.name}");
            if (character.hiredAtStart)
            {
                Debug.Log("Hired " + charSheet.name);
                adventurerStates.Add(charSheet, AdventurerState.FREE);
            }
            else
                adventurerStates.Add(charSheet, AdventurerState.UNHIRED);
        }
    }

    private void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += PartyBackFromQuest;
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += SendPartyOnQuest;
        GameObject.Find("QuestDisplayManager").transform.Find("QuestDisplay").Find("WorldState")
            .GetComponent<WorldStateManager>().AdventurerHiredEvent += HireAdventurer;
    }

    public void SendPartyOnQuest(object src, QuestSheet quest)
    {
        foreach(CharacterSheet character in quest.PartyMembers)
            adventurerStates[character] = AdventurerState.QUESTING;

        RosterChange(this, EventArgs.Empty);
    }

    public void PartyBackFromQuest(object src, QuestSheet quest)
    {
        foreach(CharacterSheet character in quest.PartyMembers)
        {
            adventurerStates[character] = AdventurerState.FREE;
            //questingAdventurers.Remove(character);
            //freeAdventurers.Add(character);
        }

        RosterChange(this, EventArgs.Empty);
    }

    public void HireAdventurer(object src, string name)
    {
        CharacterSheet adventurerToHire = null;
        foreach(var item in adventurerStates)
            if (item.Key.name.Equals(name))
                adventurerToHire = item.Key;

        if (adventurerToHire == null)
            Debug.LogError("Error: There is no adventurer by the name of " + name + " in the unhired adventurerers list");
        else
        {
            adventurerStates[adventurerToHire] = AdventurerState.FREE;
            Debug.Log("Hired " + name);
        }

        /*
        foreach (CharacterSheet character in hiredAdventurers)
        {
            if (character.name.Equals(name)) { return; }
        }

        Debug.Log("Hired " + name);
        CharacterSheet characterToHire = null;
        foreach(CharacterSheet character in unhiredAdventurers)
        {
            if (character.name.Equals(name))
                characterToHire = character;
        }
        unhiredAdventurers.Remove(characterToHire);
        hiredAdventurers.Add(characterToHire);
        freeAdventurers.Add(characterToHire);

        if (characterToHire == null)
            Debug.LogError("Error: There is no adventurer by the name of " + name + " in the unhired adventurerers list");
        else
            RosterChange(this, EventArgs.Empty);
        */
    }

    //this is never used so i commented it out

    /*
    public bool TryFindSheetByName(string name, out CharacterSheet adventurer)
	{
        foreach(CharacterSheet character in allAdventurers)
		{
            if (character.name.Equals(name)) { adventurer = character; return true; }
		}

        adventurer = null;
        return false;
	}
    */

    /// <summary>
    /// Returns read only collection of all adventurers under the given state
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private ReadOnlyCollection<CharacterSheet> GetAdventurersWithState(AdventurerState state)
    {
        List<CharacterSheet> list = new List<CharacterSheet>();
        foreach(var item in adventurerStates)
            if (item.Value == state)
                list.Add(item.Key);

        return list.AsReadOnly();
    }

    private ReadOnlyCollection<CharacterSheet> GetHiredAdventurers()
    {
        List<CharacterSheet> list = new List<CharacterSheet>();
        foreach (var item in adventurerStates)
            if (item.Value == AdventurerState.ASSIGNED || item.Value == AdventurerState.FREE || item.Value == AdventurerState.QUESTING)
                list.Add(item.Key);

        return list.AsReadOnly();

    }
}
