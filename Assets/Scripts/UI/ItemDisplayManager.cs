using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    private GameObject spawnPoint; //Where are we spawning this thing?

    // The prefabs we use to instantiate things.
    public GameObject questDisplay; 
    public GameObject characterDisplay;
    public GameObject debriefDisplay;

    private int currentDisplay = 0; // The current display. Nothing is 0, Quest is 1, character is 2, debrief is 3.

    private GameObject currentlyDisplaying; // The thing we're currently displaying.


    public GameObject lastActiveDisplay = null;
	private void Awake()
	{
        spawnPoint = this.transform.Find("SpawnPoint").gameObject; // Get our spawnpoint
    }

    /// <summary>
    /// Call when you want to display a character.
    /// </summary>
    /// <param name="display">The character you'd like displayed.</param>
    public void DisplayCharacter(CharacterSheet character)
    {
        // Internally track what thing we're displaying.
        currentDisplay = 2;

        // Destroy whatever is currently on there.
        Destroy(currentlyDisplaying);

        // Instantiate a new version of the charactersheet.
        currentlyDisplaying = Instantiate(characterDisplay,spawnPoint.transform);
        //currentlyDisplaying.transform.parent.SetAsLastSibling(); // Move it to the front

        // Get the script and put the character sheet in.
        CharacterInfoUI characterInfoUI = currentlyDisplaying.GetComponent<CharacterInfoUI>();
        characterInfoUI.charObject = this.gameObject;
        characterInfoUI.SetupCharacterInfoUI(character);

        // Plays some sounds
        var i = UnityEngine.Random.Range(0, 3);
        if (i == 0) { SoundManagerScript.PlaySound("parchment1"); }
        else if (i == 1) { SoundManagerScript.PlaySound("parchment2"); }
        else { SoundManagerScript.PlaySound("parchment3"); }
    }

    /// <summary>
    /// Call when you want to display a quest.
    /// </summary>
    /// <param name="display"></param>
    public void DisplayQuest(QuestSheet quest, GameObject caller)
    {
        // Internally track what thing we're showing.
        currentDisplay = 1;

        // Destroy whatever is currently on there.
        Destroy(currentlyDisplaying);

        // Instantiate a new version of the QuestUI
        currentlyDisplaying = Instantiate(questDisplay, spawnPoint.transform);
        //currentlyDisplaying.transform.parent.SetAsLastSibling(); // Move it to the front.

        // Give it the quest banner.
        currentlyDisplaying.GetComponent<QuestUI>().questBanner = caller;
        Debug.Log($"Gave {quest.questName} a questBanner");

        //pass quest info to quest UI
        QuestUI questUI = currentlyDisplaying.GetComponent<QuestUI>();
        questUI.SetupQuestUI(quest);

        // Plays some sounds
        var i = UnityEngine.Random.Range(0, 3);
        if (i == 0) { SoundManagerScript.PlaySound("parchment1"); }
        else if (i == 1) { SoundManagerScript.PlaySound("parchment2"); }
        else { SoundManagerScript.PlaySound("parchment3"); }
    }

    public void DisplayDebrief(bool display)
    {
        // Internally track what thing we're showing
        if (display) { currentDisplay = 3; }
        else { currentDisplay = 0; }

        Destroy(currentlyDisplaying);

        debriefDisplay.SetActive(display);
    }

    private void SetLastActiveDisplay()
    {
        if (questDisplay.activeSelf)
            lastActiveDisplay = questDisplay;
        else if (characterDisplay.activeSelf)
            lastActiveDisplay = characterDisplay;
        else if (debriefDisplay.activeSelf)
            lastActiveDisplay = debriefDisplay;
    }

    private void DisplayLastActive(GameObject g)
    {
        if(g != null)
            g.SetActive(true);
        lastActiveDisplay = null;
    }

    /// <summary>
    /// Tries to add the adventurer into the current quest sheet.
    /// </summary>
    /// <param name="character"></param>
    /// <returns>True if assigned, false if not.</returns>
    public bool AssignAdventurer(CharacterSheet character)
    {
        if(currentDisplay == 1)
		{
            // Try to get the quest UI for assignment.
            QuestUI questReference;
            if (currentlyDisplaying.TryGetComponent<QuestUI>(out questReference)){
                return questReference.AddCharacter(character);
            }
            else { return false; }
		}
        return false;
	}
}
