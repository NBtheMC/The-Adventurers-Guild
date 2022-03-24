using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartySheet
{
    public string name;
    private List<CharacterSheet> party_members;
    public IReadOnlyCollection<CharacterSheet> Party_Members { get { return party_members.AsReadOnly(); } }
    public List<string> relationshipNarrative; //list of things that happened to adventurers on a quest

    public PartySheet()
	{
        name = "";
        party_members = new List<CharacterSheet>();
        relationshipNarrative = new List<string>();
	}

    public void addMember(CharacterSheet adventurer)
    {
        if (party_members.Contains(adventurer)) { return; }
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
    public int getStatSummed(CharacterSheet.StatDescriptors stat)
    {
        int statTotal = 0;
        foreach (CharacterSheet adventurer in party_members)
        {
            statTotal += adventurer.getStat(stat);
        }
        return statTotal;
    }

    public void UpdateRelationshipStory(List<string> relationshipSubstory){
        relationshipNarrative.AddRange(relationshipSubstory);
        return;
    }


}
