using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPoolController : MonoBehaviour
{
    //public DropHandler dropHandler; // So we can tell it whenever we add or remove dropPoints.
    //public GameObject characterSlot; // For us to instantiate new Drop Points with
    public GameObject sampleCharacter; // For us to instantiate new characters with.
    public int maxColSize; // How many characters can appear vertically before we start a new column.
    public int pixelSeperatorWidth; // How much space we want to give to the generated icons.

    private List<CharacterSheet> activeRole; // The current list of characters we're using.
    private List<GameObject> characterSlots; // A list of all the buttons we expect to have.

    private CharacterSheetManager characterManager;

    private void Awake()
    {
        characterSlots = new List<GameObject>();
        activeRole = new List<CharacterSheet>();
        // We're assuming some previous point to set the last point.

        characterManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
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
            Debug.Log("Adding " + character.name);
            activeRole.Add(character);
        }

        foreach (var item in characterSlots)
        {
            CharacterTileController tileController = item.GetComponent<CharacterTileController>();
            CharacterSheet character = tileController.characterSheet;
            if (activeRole.Contains(character))
                tileController.MarkAdventurerAsFree();
            else
                tileController.MarkAdventurerAsBusy();
        }

        //slap gameobject onto end 
        
        
        //and then do sort?

        RefreshCharacterPool();
    }

    /// <summary>
    /// Deletes all that characters that are in the character pool and generates it again.
    /// Good to pair with filters and live editing later on.
    /// </summary>
    public void RefreshCharacterPool()
    {
        //dropHandler.ClearDropPoints();

        /*foreach ((GameObject, GameObject) thing in characterSlots)
        {
            Destroy(thing.Item1);
            Destroy(thing.Item2);
        }*/

        

        foreach (CharacterSheet character in activeRole)
        {
            Debug.Log("Trying to refresh " + character.name);
            GenerateNewCharacter(character);
        }
    }

    /// <summary>
    /// Used by this code to generate new character buttons.
    /// </summary>
     private void GenerateNewCharacter(CharacterSheet characterToPair)
    {
        // Makes a new character
        GameObject newCharacter = Instantiate(sampleCharacter,this.transform.Find("SpawnArea"));

        characterSlots.Add(newCharacter);

        // Setting the position, using a caculation.
        // It's a lot of complicated math from weird variables. Apologies, but it does make sense.
        float spacing = this.transform.Find("SpawnArea").gameObject.GetComponent<RectTransform>().rect.height / 12;
        float differenceInspace = spacing - newCharacter.GetComponent<RectTransform>().rect.height;
        float top = (characterSlots.Count - 1) * spacing + (differenceInspace / 2) + (newCharacter.GetComponent<RectTransform>().rect.height/2);
        Debug.Log($"spacing {spacing}, differenceInspace {differenceInspace}, top {top}");
        newCharacter.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -top, 0);

        // Get the script
        CharacterTileController theButton = newCharacter.GetComponent<CharacterTileController>();

        // Give it the character sheet.
        theButton.characterSheet = characterToPair;

        // Tell it to refresh
        theButton.Refresh();

        Debug.Log("Generated character sheet: " + newCharacter.GetComponent<CharacterTileController>().characterSheet.name);


        // newCharacter.GetComponent<Button>().onClick.AddListener(delegate { CharacterClickedOnEvent(this, characterToPair); });
        

        //print(newCharacter.transform.localPosition);
    }

    /// <summary>
    /// Adds a character to be chosen through the character pool.
    /// </summary>
    public void AddCharacter(CharacterSheet inputCharacter)
    {
        activeRole.Add(inputCharacter);
        //this.RefreshCharacterPool();
    }

    public void UnselectCharacter(CharacterSheet character)
    {
        foreach (var item in characterSlots)
        {
            if (item.GetComponent<CharacterTileController>().characterSheet == character)
                item.GetComponent<CharacterTileController>().UngrayPortrait();
        }
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

    public List<CharacterSheet> GetActiveCharacters(){
        return activeRole;
    }
}
