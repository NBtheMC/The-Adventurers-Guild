using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


public class PartySheet
{
    public string name;
    private List<CharacterSheet> party_members;
    public ReadOnlyCollection<CharacterSheet> Party_Members { get { return party_members.AsReadOnly(); } }

    public List<string> relationshipNarrative; //list of things that happened to adventurers on a quest

    public PartySheet()
	{
        name = "";
        party_members = new List<CharacterSheet>();
        relationshipNarrative = new List<string>();
	}

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
    public int getStatSummed(CharacterSheet.StatDescriptors stat)
    {
        int statTotal = 0;
        foreach (CharacterSheet adventurer in party_members)
        {
            statTotal += adventurer.getStat(stat);
        }
        return statTotal;
    }

    public void AddExperience(CharacterSheet.StatDescriptors stat, int level){
        foreach(CharacterSheet adventurer in party_members){
            int experienceToAdd = 0;
            //Ceil((DC*100/StatTotalPartySum) * AdventurerStat) + Bondbonus
            try{
                experienceToAdd = (level*100*adventurer.getStat(stat)/getStatSummed(stat)) + BondBonus(adventurer);
                adventurer.addStatExperience(stat, experienceToAdd);
            }
            catch{
                adventurer.addStatExperience(stat, 0);
            }
            
        }
    }

    public int BondBonus(CharacterSheet bonder){
        int totalBond = 0;
        foreach(CharacterSheet member in party_members){
            if(member!=bonder){
                totalBond+=bonder.adventurer.GetFriendship(member.adventurer);
            }
        }
        return totalBond/party_members.Count;
    }

    public bool Contains(CharacterSheet adventurer)
	{
        return party_members.Contains(adventurer);
	}

    public void UpdateVisibleStats(){
        foreach(CharacterSheet adventurer in party_members){
			adventurer.UpdateVisibleStatSheet();
		}
    }

    public void UpdateRelationshipStory(List<string> relationshipSubstory){
        relationshipNarrative.AddRange(relationshipSubstory);
        return;
    }


}
