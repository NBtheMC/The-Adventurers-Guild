using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterPoolController : MonoBehaviour
{
    public bool testingScript; // To see if we're in testing mode or not.
    //public DropHandler dropHandler; // So we can tell it whenever we add or remove dropPoints.
    public GameObject characterSlot; // For us to instantiate new Drop Points with
    public GameObject sampleCharacter; // For us to instantiate new characters with.
    public int maxColSize; // How many characters can appear vertically before we start a new column.
    public int pixelSeperatorWidth; // How much space we want to give to the generated icons.

    private List<CharacterSheet> activeRole; // The current list of characters we're using.
    private int numCharacterSlots; //current number of character slots visible
    private List<(GameObject slot, GameObject character)> characterSlots; // A List of all the drop areas we currently see.

    private CharacterSheetManager characterManager;
    private RectTransform slotPoints;

    private void Awake()
    {
        characterSlots = new List<(GameObject dropPoint, GameObject character)>();
        activeRole = new List<CharacterSheet>();
        // We're assuming some previous point to set the last point.

        characterManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        slotPoints = transform.Find("Drop Points").GetComponent<RectTransform>();
	}

    // Start is called before the first frame update
    void Start()
    {
        foreach (CharacterSheet character in characterManager.FreeAdventurers)
        {
            activeRole.Add(character);
        }
        RefreshCharacterPool();

        GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>().RosterChange += CharacterPoolController_RosterChange;
    }

    private void CharacterPoolController_RosterChange(object source, System.EventArgs e)
    {
        activeRole.Clear();

        foreach (CharacterSheet character in characterManager.FreeAdventurers)
        {
            activeRole.Add(character);
        }

        RefreshCharacterPool();
    }

    /// <summary>
    /// Deletes all that characters that are in the character pool and generates it again.
    /// Good to pair with filters and live editing later on.
    /// </summary>
    public void RefreshCharacterPool()
    {
        //dropHandler.ClearDropPoints();

        foreach ((GameObject, GameObject) thing in characterSlots)
        {
            Destroy(thing.Item1);
            Destroy(thing.Item2);
        }

        foreach (CharacterSheet character in activeRole)
        {
            GenerateNewDropPair(character);
        }
    }

    /// <summary>
    /// Used by this code to generate new drop points at the next appropriate place.
    /// Should not be called constantly, as it instiates new data objects.
    /// </summary>
    private void GenerateNewDropPair(CharacterSheet characterToPair)
    {
        QUestUIController questController = GameObject.Find("QuestDisplayManager").transform
            .Find("QuestDisplay").Find("CurrentItemDisplay").Find("Quest").GetComponent<QUestUIController>();
        // Makes a new drop point and a new character.
        GameObject newCaracterSlot = Instantiate(characterSlot, slotPoints.transform);
        GameObject newCharacter = Instantiate(sampleCharacter,newCaracterSlot.transform);

        //set up name display on drop point
        newCaracterSlot.transform.Find("Name").gameObject.SetActive(true);
        newCaracterSlot.transform.Find("Name").GetComponent<Text>().text = characterToPair.name;

        newCharacter.GetComponent<CharacterTileController>().characterSheet = characterToPair;
        newCharacter.GetComponent<CharacterTileController>().CharacterRightClickedOnEvent += questController.QuestTestMethod;


        //set positions of character slots
        newCaracterSlot.GetComponent<RectTransform>().anchoredPosition = new Vector3(20,-30 - (numCharacterSlots*50),0);
        newCharacter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        // newCharacter.GetComponent<Button>().onClick.AddListener(delegate { CharacterClickedOnEvent(this, characterToPair); });
        
        numCharacterSlots++;
        characterSlots.Add((newCaracterSlot, newCharacter));


        if(characterToPair.portrait != null)
        {
            newCharacter.GetComponent<Image>().sprite = characterToPair.portrait;
            newCharacter.GetComponent<Image>().color = Color.white;
        }
    }

    public void TestEvent(object src, CharacterSheet sheet)
    {
        Debug.Log("Clicked on: " + sheet.name);
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
    /// Removes a character from the character pool
    /// </summary>
    /// <param name="adventurer">CharacterSheet to remove</param>
    /// <returns>Returns true if successful and false otherwise
    public bool removeMember(CharacterSheet adventurer)
    {
        if (!activeRole.Contains(adventurer)) { return false; }
        activeRole.Remove(adventurer);
        return true;
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
