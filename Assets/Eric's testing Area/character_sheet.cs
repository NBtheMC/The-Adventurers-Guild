using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_sheet
{
    // The character's name
    public string name { get; private set; }

    // The character's stats
    private Dictionary<string, int> statlines = new Dictionary<string, int>();

    public Character_sheet(string name, int exploration, int diplomacy, int combat, int stamina)
    {
        statlines.Add("exploration", exploration);
        statlines.Add("diplomacy", diplomacy);
        statlines.Add("combat", combat);
        statlines.Add("stamina", stamina);
    }

    // Getter fuctions for the default 4 stats: exploration, diplomacy, combat, and stamina.
    public int getExploration() { return statlines["exploration"]; }
    public int getDiplomacy() { return statlines["diplomacy"]; }
    public int getCombat() { return statlines["combat"]; }
    public int getStamina() { return statlines["stamina"]; }

    // Add the following stat into the character's statlines.
    public void addStat(string statname, int number)
    {
        statlines.Add(statname, number);
    }
}

public class Party_sheet
{
    public string name;
    public List<Character_sheet> party_members = new List<Character_sheet>();
}

public class event_node
{
    public string name;
}

public class Quest_sheet
{
    public string name;
}