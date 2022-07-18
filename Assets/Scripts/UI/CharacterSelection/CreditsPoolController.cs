using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPoolController : MonoBehaviour
{
    //public DropHandler dropHandler; // So we can tell it whenever we add or remove dropPoints.
    //public GameObject characterSlot; // For us to instantiate new Drop Points with
    public GameObject sampleCharacter; // For us to instantiate new characters with.
    public int maxColSize; // How many characters can appear vertically before we start a new column.
    public int pixelSeperatorWidth; // How much space we want to give to the generated icons.

    private List<CharacterSheet> activeRole; // All the charactersheets of the characters that we have.
    private List<GameObject> characterSlots; // A list of all the buttons we expect to have.
    private Dictionary<CharacterSheet, GameObject> roleCharacterLookup; // The specific lookup between those two ordered lists.

    CharacterInitialStats[] characters;
    private List<CharacterSheet> characterSheets;

    float spacing;
    float differenceInspace;


    private void Awake()
    {
        characters = Resources.LoadAll<CharacterInitialStats>("Credits");
        characterSheets = new List<CharacterSheet>();
        //replace characters

        foreach (CharacterInitialStats character in characters)
        {
            CharacterSheet charSheet = new CharacterSheet(character);
            characterSheets.Add(charSheet);
            Debug.Log($"Added credits {charSheet.name}");
        }

        characterSlots = new List<GameObject>();
        activeRole = new List<CharacterSheet>();
        roleCharacterLookup = new Dictionary<CharacterSheet, GameObject>();
        // We're assuming some previous point to set the last point.

        // Standard calculations
        spacing = this.transform.Find("SpawnArea").gameObject.GetComponent<RectTransform>().rect.height / 12;
        differenceInspace = spacing - sampleCharacter.GetComponent<RectTransform>().rect.height;
	}

    // Start is called before the first frame update
    void Start()
    {
        RefreshCharacterPool();
    }

    // private void CharacterPoolController_RosterChange(object source, System.EventArgs e)
    // {
    //     RefreshCharacterPool();
    // }

    /// <summary>
    /// Checks for all the hired adventurers and compares them against the active role.
    /// Only use for when there's expected to be new adventurers (such as after quests).
    /// </summary>
    public void RefreshCharacterPool()
    {
        // Surgically remove any adventurers that are gone.
        // foreach (CharacterSheet character in activeRole)
		// {
        //     // Check if they're not in there anymore.
		// 	if (!characterManager.HiredAdventurers.Contains(character))
		// 	{
        //         // Find out what the button is.
        //         GameObject itemToGo = roleCharacterLookup[character];

        //         // Move everything Up.
        //         for(int i = characterSlots.IndexOf(itemToGo); i < characterSlots.Count; i++)
        //         {
        //             characterSlots[i].GetComponent<RectTransform>().anchoredPosition += new Vector2(0, spacing);
        //         }

        //         // Remove the item.
        //         activeRole.Remove(character);
        //         characterSlots.Remove(itemToGo);
        //         roleCharacterLookup.Remove(character);
        //         Destroy(itemToGo);
		// 	}
		// }

        // Add anything new.
        foreach(CharacterSheet character in characterSheets)
		{
            if (!activeRole.Contains(character))
			{
                // make a new one.
                GameObject newlyGenerated = GenerateNewCharacter(character);

                // Add it to the list.
                characterSlots.Add(newlyGenerated);
                activeRole.Add(character);
                roleCharacterLookup.Add(character, newlyGenerated);
                Debug.Log($"Added {character.name}");
            }
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
    }

    /// <summary>
    /// Used by this code to generate new character buttons.
    /// </summary>
     private GameObject GenerateNewCharacter(CharacterSheet characterToPair)
    {
        // Makes a new character
        GameObject newCharacter = Instantiate(sampleCharacter,this.transform.Find("SpawnArea"));

        // Setting the position, using a caculation.
        // It's a lot of complicated math from weird variables. Apologies, but it does make sense.
        float top = (characterSlots.Count) * spacing + (differenceInspace / 2) + (newCharacter.GetComponent<RectTransform>().rect.height / 2);
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
        return newCharacter;
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

    public void GrayOutPortrait(CharacterSheet adventurer)
    {
        roleCharacterLookup[adventurer].transform.Find("Portrait").GetComponent<Image>().color = new Color32(150, 150, 150, 255);
    }

    public void BlackOutPortrait(CharacterSheet adventurer)
    {
        roleCharacterLookup[adventurer].transform.Find("Portrait").GetComponent<Image>().color = new Color32(0, 0, 0, 200);
    }

    public void ResetPortraitColor(CharacterSheet adventurer)
    {
        roleCharacterLookup[adventurer].transform.Find("Portrait").GetComponent<Image>().color = new Color32(255, 255, 255, 200);
    }

    private void ResetPortraitColor(object src, QuestSheet quest)
    {
        foreach(CharacterSheet adventurer in quest.adventuring_party.Party_Members)
            ResetPortraitColor(adventurer);
    }
}
