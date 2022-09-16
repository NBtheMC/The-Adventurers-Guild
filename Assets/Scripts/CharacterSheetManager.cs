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

    public event EventHandler<CharacterSheet> RosterUpdate;

    CharacterInitialStats[] characters;
    public bool isCredits;

    private GuildManager guildManager;
    [SerializeField] private CharacterPoolController characterPoolController;

    public virtual void Awake()
    {
        if(!isCredits){
            characters = Resources.LoadAll<CharacterInitialStats>("Characters");
        }
        else{
            characters = Resources.LoadAll<CharacterInitialStats>("Credits");
        }

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
        guildManager = GameObject.Find("GuildManager").GetComponent<GuildManager>();
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += SendPartyOnQuest;
        GameObject.Find("QuestDisplayManager").transform.Find("QuestDisplay").Find("WorldState")
            .GetComponent<WorldStateManager>().AdventurerHiredEvent += HireAdventurer;
        if(characterPoolController == null)
            characterPoolController = GameObject.Find("Main UI/QuestDisplayManager/QuestDisplay/CharacterPool").GetComponent<CharacterPoolController>();
    }

    public void SendPartyOnQuest(object src, QuestSheet quest)
    {
        foreach(CharacterSheet character in quest.PartyMembers)
            adventurerStates[character] = AdventurerState.QUESTING;

        //RosterChange(this, EventArgs.Empty);
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

        RosterUpdate(this, adventurerToHire);
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

    /// <summary>
    /// Returns the state of the given adventurer, or null if adventurer does not exist
    /// </summary>
    /// <param name="adventurer"></param>
    /// <returns></returns>
    public AdventurerState? GetAdventurerState(CharacterSheet adventurer)
    {
        if(adventurerStates.ContainsKey(adventurer))
            return adventurerStates[adventurer];

        Debug.LogError("CharacterSheet does not exist in dictionary");
        return null;
    }

    public void SetAdventurerState(CharacterSheet adventurer, AdventurerState state)
    {
        if (adventurerStates.ContainsKey(adventurer))
            adventurerStates[adventurer] = state;
        else
            Debug.LogError("CharacterSheet does not exist in dictionary");
    }

    public ReadOnlyCollection<CharacterSheet> GetHiredAdventurers()
    {
        List<CharacterSheet> list = new List<CharacterSheet>();
        foreach (var item in adventurerStates)
            if (item.Value != AdventurerState.UNHIRED && item.Value != AdventurerState.DEAD)
                list.Add(item.Key);

        return list.AsReadOnly();

    }

    public void PayAdventurer(CharacterSheet character, int daysToPay)
    { 
        guildManager.Gold -= character.salary * daysToPay;
        character.daysUnpaid -= daysToPay;
        if (character.daysUnpaid < 0)
            character.daysUnpaid = 0;

        if (character.daysUnpaid == 0)
        {
            SetAdventurerState(character, AdventurerState.FREE);
            characterPoolController.ResetPortraitColor(character);
        }
             
    }

    public void IncrementAdventurerDebt(CharacterSheet character)
    {
        character.daysUnpaid += 1;
        //mark character as unavailable
        SetAdventurerState(character, AdventurerState.QUESTING);
        characterPoolController.BlackOutPortrait(character);

        if (character.daysUnpaid == 3)
        {
            //adventurer quits
            SetAdventurerState(character, AdventurerState.UNHIRED);
            character.daysUnpaid = 0;
            RosterUpdate(this, character);
        }
    }
}
