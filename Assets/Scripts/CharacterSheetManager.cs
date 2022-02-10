using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheetManager : MonoBehaviour
{
    private List<CharacterSheet> unhiredAdventurers; //Adventurers not working in the guild
    private List<CharacterSheet> freeAdventurers; //Adventurers in the guild that are not on a quest
    private List<CharacterSheet> questingAdventurers; //Adventurers that are currently on a quest
    private List<CharacterSheet> deadAdventurers; //Adventurers that fucking died

    public IReadOnlyCollection<CharacterSheet> UnhiredAdventurers { get { return unhiredAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> FreeAdventurers { get { return freeAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> QuestingAdventurers { get { return questingAdventurers.AsReadOnly(); } }
    public IReadOnlyCollection<CharacterSheet> DeadAdventurers { get { return deadAdventurers.AsReadOnly(); } }

    public event EventHandler<EventArgs> RosterChange;

    private void Awake()
    {
        unhiredAdventurers = new List<CharacterSheet>();
        freeAdventurers = new List<CharacterSheet>();
        questingAdventurers = new List<CharacterSheet>(); 
        deadAdventurers = new List<CharacterSheet>();

        CharacterInitialStats[] characters;
        characters = Resources.LoadAll<CharacterInitialStats>("Characters");

        foreach (CharacterInitialStats character in characters)
        {
            CharacterSheet charSheet = new CharacterSheet(character);

            if (character.hiredAtStart)
                freeAdventurers.Add(charSheet);
            else
                unhiredAdventurers.Add(charSheet);
        }
    }

    private void Start()
    {
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestFinished += PartyBackFromQuest;
        GameObject.Find("QuestingManager").GetComponent<QuestingManager>().QuestStarted += SendPartyOnQuest;
    }

    public void SendPartyOnQuest(object src, QuestSheet quest)
    {
        foreach(CharacterSheet character in quest.PartyMembers)
        {
            freeAdventurers.Remove(character);
            questingAdventurers.Add(character);
        }
        //RosterChange(this, EventArgs.Empty);
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
}
