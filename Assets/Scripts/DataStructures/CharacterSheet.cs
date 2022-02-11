using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats
    private Dictionary<StatDescriptors, int> statlines = new Dictionary<StatDescriptors, int>();

    // The characters relationships
    public Adventurer adventurer;

    // All the basic stats.
    public enum StatDescriptors { Combat = 1, Exploration = 2, Diplomacy = 3, Stamina = 4 }

    /// <summary>
    /// Constructor with access to change dictionary at any time.
    /// </summary>
    /// <param name="nameInput">Name of Character</param>
    /// <param name="statslinesInput">Dictionary of stats to copy into character.</param>
    public CharacterSheet(string nameInput, Dictionary<StatDescriptors, int> statslinesInput)
	{
        name = nameInput;
        statlines = new Dictionary<StatDescriptors, int>(statslinesInput);
        adventurer = new Adventurer();
        adventurer.characterSheet = this;
    }

    /// <summary>
    /// Constructor that takes a parameter of type CharacterInitialStats in order to generate a character sheet
    /// </summary>
    public CharacterSheet(CharacterInitialStats characterStats)
    {
        name = characterStats.name;
        statlines.Add(StatDescriptors.Combat, characterStats.combat);
        statlines.Add(StatDescriptors.Diplomacy, characterStats.diplomacy);
        statlines.Add(StatDescriptors.Exploration, characterStats.exploration);
        statlines.Add(StatDescriptors.Stamina, characterStats.stamina);
        adventurer = new Adventurer();
        adventurer.characterSheet = this;
    }

    /// <summary>
    /// Returns a stat from this character
    /// </summary>
    /// <param name="stat">A string of what stat needs to exist.</param>
    /// <returns>If it exists, An int representing the stat if it exists.
    /// If not, 0.</returns>
    public int getStat(StatDescriptors stat) {
        if (!statlines.ContainsKey(stat)) { return 0; }
        return statlines[stat];
    }

    // Add the following stat into the character's statlines.
    public void addStat(StatDescriptors statname, int number)
    {
		if (statlines.ContainsKey(statname)) { statlines[statname] = number; }
        statlines.Add(statname, number);
    }

    /// <summary>
    /// Gets all the statnames and current stat information.
    /// </summary>
    /// <returns>A whole dictionary of the statname and the current stat number.</returns>
    public Dictionary<StatDescriptors,int> GetStatSheet()
	{
        return new Dictionary<StatDescriptors, int>(statlines);
	}

}