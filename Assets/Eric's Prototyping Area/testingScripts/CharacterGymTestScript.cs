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
                Dictionary<string,int> generatedCharacterStats = new Dictionary<string, int>();

                // Then we dump a bunch of statlines into it.
                for (int index2 = 0; index2< Mathf.FloorToInt(Random.Range(0, 20)); index2++)
				{
                    int tempStat = Mathf.FloorToInt(Random.Range(0, 15));
                    generatedCharacterStats.Add($"Stat {index2}", tempStat);
                    totals[$"Stat {index2}"] = totals[$"Stat {index2}"] + tempStat;
                }
                CharacterSheet adventurer = new CharacterSheet($"Adventurer {index}", generatedCharacterStats);
                theParty.addMember(adventurer);
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
    public void Gym3QuestTraversal()
    {
        // Make a simple adventuring party, all with stats of 5 across the board.
        PartySheet testParty = new PartySheet();
        for (int i = 0; i < 4; i++)
        {
            Dictionary<string, int> generatedStats = new Dictionary<string, int>();
            generatedStats.Add("stamina", 5);
            CharacterSheet testCharacter = new CharacterSheet($"Character {i}", generatedStats);
            testParty.addMember(testCharacter);
        }

        // Generate a simple "defeating a boss" event graph.
        EventNode travelToBoss = new EventNode("stamina", 0, 50);
        EventNode defeatBoss = new EventNode("combat", 15, 50);

        // Add connections using the addConnection() function
        travelToBoss.addConnection(defeatBoss);

        // Set Event Types directly. You do not need to set traveltoBoss as EventTypes are by default head.
        defeatBoss.eventType = EventNode.EventTypes.successful;

        // Create a Quest Sheet and assign them the head of an event.
        QuestSheet defeatBossQuest = new QuestSheet(travelToBoss, "Test Quest");
        defeatBossQuest.assignParty(testParty);

        // AdvancebyTick should be able to return a 0 for a quest continuing, and a 1 for a quest finishing.
        int ticktimer = 0;
        while(defeatBossQuest.advancebyTick() == 0)
		{
            ticktimer++;
            if (ticktimer > 160)
			{
                break;
			}
		}

        // Make sure that we used up only until 150 ticks.
        Debug.Assert(ticktimer == 100,$"Test 1 Failure, ticktimer retuned {ticktimer}.");
        Debug.Assert(defeatBossQuest.QuestComplete == true, "Incomplete Quest");

        Debug.Log("Gym 3 - Quest Traversal passed.");
    }

    // Gives a mostly simple quest, and tries to make sure that the quest comes out right.
    public void Gym4SimpleEventReturn()
	{
        // Make a simple adventuring party, all with stats of 5 across the board.
        PartySheet testParty = new PartySheet();
        for (int i = 0; i < 4; i++)
        {
            Dictionary<string, int> generatedStats = new Dictionary<string, int>();
            generatedStats.Add("stamina", 5);
            CharacterSheet testCharacter = new CharacterSheet($"Character {i}", generatedStats);
            testParty.addMember(testCharacter);
        }

        // Generate a simple "defeating a boss" event graph.
        EventNode travelToBoss = new EventNode("stamina", 0, 50);
        EventNode defeatBoss = new EventNode("combat", 15, 50);

        // Add connections using the addConnection() function
        travelToBoss.addConnection(defeatBoss);

        // Set Event Types directly. You do not need to set traveltoBoss as EventTypes are by default head.
        defeatBoss.eventType = EventNode.EventTypes.successful;

        // The returned Event Package 
        EventNode.EventPackage returnMessage = travelToBoss.resolveEvent(testParty);
        Debug.Assert(returnMessage.objectiveComplete == true);
        Debug.Assert(returnMessage.nextEvent == defeatBoss);

        Debug.Log("Gym 4 - Simple Event Return Passed");
    }
}