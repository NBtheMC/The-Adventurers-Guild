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
    /// <returns>The stat if it exists, 0 if not.</returns>
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

public class PartySheet
{
    public string name;
    public List<CharacterSheet> party_members = new List<CharacterSheet>();

    public void addMember(CharacterSheet adventurer)
	{
        party_members.Add(adventurer);
	}

    /// <summary>
    /// Allows the removal of any adventurer that already in the party.
    /// </summary>
    /// <param name="adventurer">The charactersheet of the adventurer</param>
    /// <returns>
    /// A boolean depending on whether the adventurer was successfully removed.
    /// </returns>
    public bool removeMember(CharacterSheet adventurer)
	{
        if (!party_members.Contains(adventurer)) { return false; }
        party_members.Remove(adventurer);
        return true;
	}

    /// <summary>
    /// Returns to sum of the stats for each party member.
    /// </summary>
    /// <param name="stat">A string of what stat it is.</param>
    /// <returns>The Summed stat.</returns>
    public int getStatSummed(string stat)
	{
        int statTotal = 0;
        foreach(CharacterSheet adventurer in party_members)
		{
            statTotal += adventurer.getStat(stat);
		}
        return statTotal;
	}
}

public class EventNode
{
    public string name;
}

public class QuestSheet
{
    public string name;
}