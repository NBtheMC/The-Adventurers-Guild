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

    // Gym 2 will test binary event passage.
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

        // sets nodes to be passable, and adds testNode2 to it.
        testNode.DC = 39;
        EventNode testNode2 = new EventNode("diplomacy",41,30);
        testNode.eventType = EventNode.EventTypes.successful;
        testNode.addConnection(testNode2);

        // Requesting new package
        returnMessage = testNode.resolveEvent(testParty);
        Debug.Assert(returnMessage.objectiveComplete == true, "Gym 2 Test 2 Objective Wrong");
        Debug.Assert(returnMessage.nextEvent == testNode2, "Gym 2 Test 2 Incorrect Event returned.");

        // Now to make them fail the first event, and return a failure event.
        testNode.DC = 41;
        EventNode testNode3 = new EventNode("combat", 50, 54);
        testNode3.eventType = EventNode.EventTypes.fail;
        testNode.addConnection(testNode3);

        returnMessage = testNode.resolveEvent(testParty);
        Debug.Assert(returnMessage.objectiveComplete == false, "Gym 2 Test 3 Objective Wrong");
        Debug.Assert(returnMessage.nextEvent == testNode3, "Gym 2 Test 3 Incorrect Event returned.");

        Debug.Log("Gym 2 successfully passed");
	}

    // Gym will test if a quest will update itself over and over again, navigating the quest event data correctly.
    public void GymQuestTraversal()
    {
        // Make a random adventuring party.
        PartySheet testParty = new PartySheet();
        testParty.GenerateExampleParty();


        Debug.Log("Gym - Quest Traversal passed.");
    }
}
