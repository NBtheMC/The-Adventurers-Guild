using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats
    private Dictionary<string, int> statlines = new Dictionary<string, int>();

    public CharacterSheet(string name)
    {
        this.name = name;
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
}