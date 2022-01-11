using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPoolController : MonoBehaviour
{
    public bool testingScript;

    private List<CharacterSheet> activeRole;

    // Start is called before the first frame update
    void Start()
    {
        // Making a sample character to test things out on.
        Dictionary<string, int> sampleStats = new Dictionary<string, int>()
        {
            {"diplomacy", 10 },
            {"combat", 15 },
            {"stamina",10 },
            {"exploration",12 }
        };
        CharacterSheet testCharacter = new CharacterSheet("bob", sampleStats);
    }

    // Update will be used to refresh the look of this object.
    void Update()
    {
        
    }

    /// <summary>
    /// Accepts a list of charactersheets to compile and generate active characters.
    /// </summary>
    public void CompileActiveCharacters(List<CharacterSheet> listofCharacters)
	{
        // Makes a new list of characters, to be augmentable by themselves.
        activeRole = new List<CharacterSheet>(listofCharacters);
	}
}
