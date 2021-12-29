using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats
    private Dictionary<string, int> statlines = new Dictionary<string, int>();

    
    /// <summary>
    /// Default Constructor
    /// </summary>
    public CharacterSheet() { name = "Cock"; } // I'm sorry, my boyfriend told me to do this
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="name">Name of Character</param>
    public CharacterSheet(string nameInput)
    {
        this.name = nameInput;
    }
    public CharacterSheet(string nameInput,List<string> defaultStats)
	{
        name = nameInput;
        foreach(string statline in defaultStats) { statlines[statline] = 0;}
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

    /// <summary>
    /// Generates a random character with random stats. For use in testing purposes only.
    /// Use secondary constructor to prime a character full of stats.
    /// </summary>
    public void GenerateRandomExample()
	{
        foreach (string name in statlines.Keys)
		{
            statlines[name] = Mathf.FloorToInt(Random.Range(0,15));
		}
	}
}