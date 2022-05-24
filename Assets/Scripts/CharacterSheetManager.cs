using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheetManager : MonoBehaviour
{
    private List<CharacterSheet> unhiredAdventurers; //Adventurers not working in the guild
    private List<CharacterSheet> hiredAdventurers; //Adventurers working in the guild
    private List<CharacterSheet> freeAdventurers; //Adventurers in the guild that are not on a quest
    private List<CharacterSheet> questingAdventurers; //Adventurers that are currently on a quest
    private List<CharacterSheet> deadAdventurers; //Adventurers that fucking died
    private List<CharacterSheet> allAdventurers; //All the adventurers

    public IReadOnlyCollection<CharacterSheet> UnhiredAdventurers { get { return unhiredAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> HiredAdventurers { get { return hiredAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> FreeAdventurers { get { return freeAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> QuestingAdventurers { get { return questingAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> DeadAdventurers { get { return deadAdventurers.AsReadOnly(); } }

    public event EventHandler<EventArgs> RosterChange;

    private void Awake()
    {
        unhiredAdventurers = new List<CharacterSheet>();
        hiredAdventurers = new List<CharacterSheet>();
        freeAdventurers = new List<CharacterSheet>();
        questingAdventurers = new List<CharacterSheet>(); 
        deadAdventurers = new List<CharacterSheet>();
        allAdventurers = new List<CharacterSheet>();

        CharacterInitialStats[] characters;
        characters = Resources.LoadAll<CharacterInitialStats>("Characters");

        foreach (CharacterInitialStats character in characters)
        {
            CharacterSheet charSheet = new CharacterSheet(character);
            allAdventurers.Add(charSheet);
            Debug.Log($"Added adventurer {charSheet.name}");
            if (character.hiredAtStart){
                freeAdventurers.Add(charSheet);
                hiredAdventurers.Add(charSheet);}
            else
                unhiredAdventurers.Add(charSheet);
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
        {
            freeAdventurers.Remove(character);
            questingAdventurers.Add(character);
        }
        RosterChange(this, EventArgs.Empty);
    }

    public void PartyBackFromQuest(object src, QuestSheet quest)
    {
        foreach(CharacterSheet character in quest.PartyMembers)
        {
            questingAdventurers.Remove(character);
            freeAdventurers.Add(character);
        }
        RosterChange(this, EventArgs.Empty);
    }

    public void HireAdventurer(object src, string name)
    {
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
    }

    public bool TryFindSheetByName(string name, out CharacterSheet adventurer)
	{
        foreach(CharacterSheet character in allAdventurers)
		{
            if (character.name.Equals(name)) { adventurer = character; return true; }
		}

        adventurer = null;
        return false;
	}
}
