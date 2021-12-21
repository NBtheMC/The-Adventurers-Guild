using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGymTestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
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

    // Gym 2 will create a quest with a number of event connections, then make multiple calls to progress time.
    public void Gym2EventDataConnection()
	{

	}

    // Gym 3 will
    public void Gym3() { }
}
