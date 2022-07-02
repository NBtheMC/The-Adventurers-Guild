using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class CreditsSheetManager : CharacterSheetManager
{
    public override void Awake()
    {
        base.Awake();
        CharacterInitialStats[] characters;
        characters = Resources.LoadAll<CharacterInitialStats>("Credits");
        //replace characters

        //TODO Commented this out because changes from CharacterSheetManager broke it, fix later
        /*
        allAdventurers.Clear();
        freeAdventurers.Clear();
        hiredAdventurers.Clear();
        unhiredAdventurers.Clear();
        foreach (CharacterInitialStats character in characters)
        {
            CharacterSheet charSheet = new CharacterSheet(character);
            allAdventurers.Add(charSheet);
            freeAdventurers.Add(charSheet);
            hiredAdventurers.Add(charSheet);
            Debug.Log($"Added credits {charSheet.name}");
        }
        */
    }
}
