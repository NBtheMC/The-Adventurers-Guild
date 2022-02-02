using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheetManager : MonoBehaviour
{
    public List<CharacterSheet> unhiredAdventurers; //Adventurers not working in the guild
    public List<CharacterSheet> freeAdventurers; //Adventurers in the guild that are not on a quest
    public List<CharacterSheet> questingAdventurers; //Adventurers that are currently on a quest
    public List<CharacterSheet> deadAdventurers; //Adventurers that fucking died

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
}
