using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats
    private Dictionary<string, int> statlines = new Dictionary<string, int>();

    // The characters relationships
    public Adventurer adventurer;

    /// <summary>
    /// Normal Character Sheet Constructor
    /// </summary>
    /// <param name="nameInput">Name of the character</param>
    /// <param name="defaultStats">A List of all the statsnames.</param>
    public CharacterSheet(string nameInput,List<string> defaultStats)
	{
        name = nameInput;
        foreach(string statline in defaultStats) { statlines[statline] = 0;}
        adventurer = new Adventurer();
        //relationships.transform.SetParent(this.gameObject); //charactersheet and adventurer have same parent
	}

    /// <summary>
    /// Constructor with access to change dictionary at any time.
    /// </summary>
    /// <param name="nameInput">Name of Character</param>
    /// <param name="statslinesInput">Dictionary of stats to copy into character.</param>
    public CharacterSheet(string nameInput, Dictionary<string, int> statslinesInput)
	{
        name = nameInput;
        statlines = new Dictionary<string, int>(statslinesInput);
	}

    /// <summary>
    /// Constructor that takes a parameter of type CharacterInitialStats in order to generate a character sheet
    /// </summary>
    public CharacterSheet(CharacterInitialStats characterStats)
    {
        name = characterStats.name;
        statlines.Add("combat", characterStats.combat);
        statlines.Add("diplomacy", characterStats.diplomacy);
        statlines.Add("exploration", characterStats.exploration);
        statlines.Add("stamina", characterStats.stamina);
    }

    /// <summary>
    /// Returns a stat from this character
    /// </summary>
    /// <param name="stat">A string of what stat needs to exist.</param>
    /// <returns>If it exists, An int representing the stat if it exists.
    /// If not, 0.</returns>
    public int getStat(string stat) {
        if (!statlines.ContainsKey(stat)) { return 0; }
        return statlines[stat];
    }

    // Add the following stat into the character's statlines.
    public void addStat(string statname, int number)
    {
		if (statlines.ContainsKey(statname)) { statlines[statname] = number; }
        statlines.Add(statname, number);
    }

    /// <summary>
    /// Gets all the statnames and current stat information.
    /// </summary>
    /// <returns>A whole dictionary of the statname and the current stat number.</returns>
    public Dictionary<string,int> GetStatSheet()
	{
        return new Dictionary<string, int>(statlines);
	}

}