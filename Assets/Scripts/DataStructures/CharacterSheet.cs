using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats and experience
    private Dictionary<StatDescriptors, int[]> statlines; //current level, current experience, and experience required to level up 
    private Dictionary<StatDescriptors, int[]> visibleStatlines; //what actually gets displayed to the player

    // The characters relationships
    public Adventurer adventurer;

    // All the basic stats.
    public enum StatDescriptors { Combat = 1, Exploration = 2, Charisma = 3}

    //Portrait associated with character
    public Sprite portrait { get; private set; }

    // Text associated with the character.
    public string biography { get; private set; }

    /// <summary>
    /// Constructor with access to change dictionary at any time.
    /// </summary>
    /// <param name="nameInput">Name of Character</param>
    /// <param name="statslinesInput">Dictionary of stats to copy into character.</param>
    public CharacterSheet(string nameInput, Dictionary<StatDescriptors, int> statslinesInput)
	{
        name = nameInput;
        statlines = new Dictionary<StatDescriptors, int[]>();
        foreach( KeyValuePair<StatDescriptors, int> kvp in statslinesInput )
        {
            statlines.Add(kvp.Key, new int[] {kvp.Value,0,(int)Mathf.Pow(kvp.Value,2)*200});
        }
        adventurer = new Adventurer();
        adventurer.characterSheet = this;
    }

    /// <summary>
    /// Constructor that takes a parameter of type CharacterInitialStats in order to generate a character sheet
    /// </summary>
    public CharacterSheet(CharacterInitialStats characterStats)
    {
        name = characterStats.name;
        statlines = new Dictionary<StatDescriptors, int[]>();
        statlines.Add(StatDescriptors.Combat, new int[] {characterStats.combat, 0,(int)Mathf.Pow(characterStats.combat+1,2)*200});
        statlines.Add(StatDescriptors.Charisma, new int[] {characterStats.charisma, 0,(int)Mathf.Pow(characterStats.charisma+1,2)*200});
        statlines.Add(StatDescriptors.Exploration, new int[] {characterStats.exploration, 0,(int)Mathf.Pow(characterStats.exploration+1,2)*200});
        adventurer = new Adventurer();
        adventurer.characterSheet = this;
        portrait = characterStats.portrait;
        biography = characterStats.Biography;
    }

    /// <summary>
    /// Returns a stat from this character
    /// </summary>
    /// <param name="stat">A string of what stat needs to exist.</param>
    /// <returns>If it exists, An int representing the stat if it exists.
    /// If not, 0.</returns>
    public int getStat(StatDescriptors stat) {
        if (!statlines.ContainsKey(stat)) { return 0; }
        return statlines[stat][0];
    }

    public int getVisibleStat(StatDescriptors stat) {
        if (!visibleStatlines.ContainsKey(stat)) { return 0; }
        return visibleStatlines[stat][0];
    }

    // Add the following stat into the character's statlines.
    public void addStat(StatDescriptors statname, int number)
    {
		if (statlines.ContainsKey(statname)) { 
            statlines[statname][0] = number;
            statlines[statname][1] = 0; 
            statlines[statname][2] = (int)(Mathf.Pow(number+1,2)*200); 
        }
        statlines.Add(statname, new int[] {number, 0,(int)Mathf.Pow(number+1,2)*200});
        Debug.Log("Added "+number+" to "+ name);
    }

    public void addStatExperience(StatDescriptors statname, int number)
    {
        statlines[statname][1]+=number;
        //levelling up
        while(statlines[statname][1]>=statlines[statname][2]){
            statlines[statname][0]++;   //level
            statlines[statname][1] -= statlines[statname][2]; //change to remainder
            statlines[statname][2] = (int)Mathf.Pow(statlines[statname][0],2)*200;
        }
    }

    /// <summary>
    /// Gets all the statnames and current stat information.
    /// </summary>
    /// <returns>A whole dictionary of the statname and the current stat number.</returns>
    public Dictionary<StatDescriptors,int> GetStatSheet()
	{
        Dictionary<StatDescriptors, int> justStats = new Dictionary<StatDescriptors, int>();
        foreach(KeyValuePair<StatDescriptors, int[]> kvp in statlines){
            justStats.Add(kvp.Key, kvp.Value[0]);
        }
        return justStats;
	}

    /// <summary>
    /// Visible stat information to actually show on the sheet
    /// </summary>
    /// <returns>A whole dictionary of the statname and the visible stat number.</returns>
    public Dictionary<StatDescriptors,int[]> GetVisibleStatSheet()
	{
        return visibleStatlines;
	}

    public void UpdateVisibleStatSheet(){
        visibleStatlines = new Dictionary<StatDescriptors, int[]>(statlines); 
    }

}
