using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Test_Buttons : MonoBehaviour
{
    public CharacterPoolController poolController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Tells the UI Canvas to add a random character to the character Pool.
    /// </summary>
    public void AddRandomCharacterToPool()
	{
        // Making a sample character to test things out on.
        Dictionary<string, int> sampleStats = new Dictionary<string, int>()
        {
            {"diplomacy", Random.Range(1,20) },
            {"combat", Random.Range(1,20) },
            {"stamina",Random.Range(1,20) },
            {"exploration",Random.Range(1,20) }
        };
        CharacterSheet testCharacter = new CharacterSheet("bob", sampleStats);
        poolController.AddCharacter(testCharacter);
        //poolController.RefreshCharacterPool();
    }
}
