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
        Dictionary<CharacterSheet.StatDescriptors, int> sampleStats = new Dictionary<CharacterSheet.StatDescriptors, int>()
        {
            {CharacterSheet.StatDescriptors.Charisma, Random.Range(1,20) },
            {CharacterSheet.StatDescriptors.Combat, Random.Range(1,20) },
            //{CharacterSheet.StatDescriptors.Constitution,Random.Range(1,20) },
            {CharacterSheet.StatDescriptors.Exploration,Random.Range(1,20) }
        };
        CharacterSheet testCharacter = new CharacterSheet("bob", sampleStats);
        poolController.AddCharacter(testCharacter);
        //poolController.RefreshCharacterPool();
    }
}
