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
    public int pixelSeperatorWidth; // How much space we want to give to the generated icons.

    private List<CharacterSheet> activeRole; // The current list of characters we're using.
    private int lastPlacedRow;
    private int lastPlacedCol;
    private List<(GameObject dropPoint, GameObject character)> visibleDropAreas; // A List of all the drop areas we currently see.

	private void Awake()
	{
        visibleDropAreas = new List<(GameObject dropPoint, GameObject character)>();
		activeRole = new List<CharacterSheet>();
        // We're assuming some previous point to set the last point.
        lastPlacedCol = -1;
        lastPlacedRow = maxColSize;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    /// <summary>
    /// Deletes all that characters that are in the character pool and generates it again.
    /// Good to pair with filters and live editing later on.
    /// </summary>
    public void RefreshCharacterPool()
	{
        foreach((GameObject,GameObject) thing in visibleDropAreas)
		{
            Destroy(thing.Item1);
            Destroy(thing.Item2);
		}

        foreach(CharacterSheet character in activeRole)
		{
            GenerateNewDropPair(character);
		}

        lastPlacedCol = -1;
        lastPlacedRow = maxColSize;
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

        // Adds both references to our internal tracking script.
        visibleDropAreas.Add((newDropPoint, newCharacter));

        // Reference to their scripts.
        ObjectDropPoint dropPointController = newDropPoint.GetComponent<ObjectDropPoint>();
        DraggerController characterController = newCharacter.GetComponent<DraggerController>();


        // Tells drop point who is suppose to sit on it.
        dropPointController.heldObject = characterController;

        // Places the drop point where it's suppose to go.
        if(lastPlacedRow == maxColSize)
		{
            lastPlacedCol++;
            lastPlacedRow = 0;
		} else { lastPlacedRow++; }
        float calcXPos = 60 + (lastPlacedCol * (newCharacter.GetComponent<RectTransform>().rect.width+pixelSeperatorWidth));
        float calcYPos = -60 + (lastPlacedRow * (newCharacter.GetComponent<RectTransform>().rect.height+pixelSeperatorWidth) * -1);
        dropPointController.GetComponent<RectTransform>().anchoredPosition = new Vector3(calcXPos, calcYPos);

        // Tells dropHandler that we have a new dropPoint.
        dropHandler.AddDropPoint(dropPointController);

        // Tells the dropPointController that it should only take characters.
        dropPointController.dropType = DropHandler.DropType.character;

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
        //this.RefreshCharacterPool();
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
