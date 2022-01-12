using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPoolController : MonoBehaviour
{
    public bool testingScript; // To see if we're in testing mode or not.
    public DropHandler dropHandler; // So we can tell it whenever we add or remove dropPoints.
    public GameObject sampleDropPoint; // For us to instantiate new Drop Points with
    public GameObject sampleCharacter; // For us to instantiate new characters with.
    public int maxColSize; // How many characters can appear vertically before we start a new column.

    private List<CharacterSheet> activeRole; // The current list of characters we're using.
    private int lastPlacedRow;
    private int lastPlacedCol;

	private void Awake()
	{
		activeRole = new List<CharacterSheet>();
        lastPlacedCol = 1;
        lastPlacedRow = 0;
	}

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
        AddCharacter(testCharacter);
        GenerateNewDropPair(testCharacter);
    }

    // Update will be used to refresh the look of this object.
    void Update()
    {
        
    }

    /// <summary>
    /// Used by this code to generate new drop points at the next appropriate place.
    /// Should not be called constantly, as it instiates new data objects.
    /// </summary>
    private void GenerateNewDropPair(CharacterSheet characterToPair)
	{
        // Makes a new drop point and a new character.
        GameObject newDropPoint = Instantiate(sampleDropPoint,this.transform);
        GameObject newCharacter = Instantiate(sampleCharacter, this.transform);

        // Reference to their scripts.
        ObjectDropPoint dropPointController = newDropPoint.GetComponent<ObjectDropPoint>();
        DraggerController characterController = newCharacter.GetComponent<DraggerController>();

        // Tells drop point who is suppose to sit on it.
        dropPointController.heldObject = characterController;

        // Places the drop point where it's suppose to go.
        if(lastPlacedRow == maxColSize)
		{
            lastPlacedCol++;
            lastPlacedRow = 1;
		} else { lastPlacedRow++; }
        dropPointController.GetComponent<RectTransform>().anchoredPosition = new Vector3(lastPlacedCol * 60, lastPlacedRow * -60);

        // Tells dropHandler that we have a new dropPoint.
        dropHandler.AddDropPoint(dropPointController);

        // Gives the new character a home drop point, hands it the new dropHandler, and tells it what it's purpose is.
        characterController.objectDropPoint = dropPointController;
        characterController.dropHandler = dropHandler;
        characterController.dropType = DropHandler.DropType.character;

	}

    /// <summary>
    /// Adds a character to be chosen through the character pool.
    /// </summary>
    public void AddCharacter(CharacterSheet inputCharacter)
	{
        activeRole.Add(inputCharacter);
	}

    /// <summary>
    /// Accepts a list of charactersheets to compile and generate active characters. Wipes current characters.
    /// </summary>
    public void CompileActiveCharacters(List<CharacterSheet> listofCharacters)
	{
        // Makes a new list of characters, to be augmentable by themselves.
        activeRole = new List<CharacterSheet>(listofCharacters);
	}
}
