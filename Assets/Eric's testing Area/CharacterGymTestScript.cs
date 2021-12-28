using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGymTestScript : MonoBehaviour
{

    // Gym 1 will create a bunch of character with numbers, then check if those numbers are correctly summed by party.
    public void Gym1PartyStatTracking()
	{
        int numSuccess = 0;
        int numTries = 10;

        // This first for loop will be the number of random generators.
        for (int attempt = 0; attempt < numTries; attempt++)
		{
            Dictionary<string, int> totals = new Dictionary<string, int>();

            PartySheet theParty = new PartySheet();

            // Prep the stat dictionary
            for (int i = 0; i < 20; i++)
			{
                totals.Add($"Stat {i}", 0);
			}

            // Then we go about generating a bunch of characters.
            for (int index = 0; index < Mathf.FloorToInt(Random.Range(0,20)); index++)
			{
                CharacterSheet adventurer = new CharacterSheet($"Adventurer {index}");
                theParty.addMember(adventurer);

                // Then we dump a bunch of statlines into it.
                for (int index2 = 0; index2< Mathf.FloorToInt(Random.Range(0, 20)); index2++)
				{
                    int tempStat = Mathf.FloorToInt(Random.Range(0, 15));
                    adventurer.addStat($"Stat {index2}", tempStat);
                    totals[$"Stat {index2}"] = totals[$"Stat {index2}"] + tempStat;
                }
			}

            bool failed = false;
            // Now we check if each stat is correct.
            foreach (string statType in totals.Keys)
			{
                if (totals[statType] != theParty.getStatSummed(statType))
				{
                    failed = true;
                    break;
				}
                Debug.Log($"{statType} matches in total character for iteration {numTries}");
			}

            if (!failed) { numSuccess++; }
		}

        Debug.Log($"Successful {numSuccess} out of {numTries}");
    }

    // Gym 2 will test all the events by creating a bunch of event data.
    public void Gym2EventNode()
	{
        // Creates an event node.
        EventNode testNode = new EventNode();
        testNode.stat = "stamina";
        testNode.DC = 15;
        testNode.time = 80;

        // Need to be able to pull time form the node.
        Debug.Assert(testNode.time == 80);

        // Generate a random party that will be tested with an event.
        PartySheet testParty = new PartySheet();
        Dictionary<string, int> sampleStats = new Dictionary<string, int>
        {
            {"diplomacy", 10 },
            {"stamina", 10},
            {"combat", 10 },
            {"exploration", 10 }
        };
        CharacterSheet characterA = new CharacterSheet("character A", sampleStats);
        CharacterSheet characterB = new CharacterSheet("character B", sampleStats);
        CharacterSheet characterC = new CharacterSheet("character C", sampleStats);
        CharacterSheet characterD = new CharacterSheet("character D", sampleStats);

        testParty.addMember(characterA);
        testParty.addMember(characterB);
        testParty.addMember(characterC);
        testParty.addMember(characterD);

        // Now it should be able to return a package signifying what happened after the check.
        EventNode.EventPackage returnMessage = testNode.resolveEvent(testParty);

        // Assuming it's a summed stat, the event should resolve with a objective passed, followed by a null reference to next node
        Debug.Assert(returnMessage.objectiveComplete == true,"Gym 2 Test 1 Objective Wrong");
        Debug.Assert(returnMessage.nextEvent == null, "Gym 2 Test 1 Event Exists");

        Debug.Log("Test 2 successfully passed");
	}

    // Gym 3 is used to asset stats are being cleared correctly in the character_sheet
    public void Gym3CharacterTesting()
	{
        PartySheet testParty = new PartySheet();
        Dictionary<string, int> sampleStats = new Dictionary<string, int>
        {
            {"diplomacy", 10 },
            {"stamina", 10},
            {"combat", 10 },
            {"exploration", 10 }
        };
        CharacterSheet characterA = new CharacterSheet("character A", sampleStats);
        CharacterSheet characterB = new CharacterSheet("character B", sampleStats);
        CharacterSheet characterC = new CharacterSheet("character C", sampleStats);
        CharacterSheet characterD = new CharacterSheet("character D", sampleStats);

        testParty.addMember(characterA);
        testParty.addMember(characterB);
        testParty.addMember(characterC);
        testParty.addMember(characterD);

        // Now let's make sure the stats actually copied through correctly.
        sampleStats["diplomacy"] = 9;
        Debug.Assert(characterA.getStat("diplomacy") == 10, "Stat didn't clear");
    }
}
