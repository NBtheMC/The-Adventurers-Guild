using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayManager : MonoBehaviour
{
    private GameObject spawnPoint; //Where are we spawning this thing?

    private bool isEndgame;

    // The prefabs we use to instantiate things.
    public GameObject questDisplay; 
    public GameObject characterDisplay;
    public GameObject debriefDisplay;
    public GameObject endGame;
    public GameManager gameManager; // reference to the game manager.
    private GameObject currentlyDisplaying; // The thing we're currently displaying.
    public GameObject lastActiveDisplay = null;

    public ItemDisplay currentDisplay;

	private void Awake()
	{
        spawnPoint = this.transform.Find("SpawnPoint").gameObject; // Get our spawnpoint
    }


    /// <summary>
    /// Call when you want to display a character.
    /// </summary>
    /// <param name="character">The character you'd like displayed.</param>
    /// <returns>true if same character as last is being displayed, false otherwise. Returns null if isEndgame</returns>

    public bool DisplayCharacter(CharacterSheet character)
    {
        string currentCharacter = "";
        if (currentDisplay == ItemDisplay.CHARACTER)
            currentCharacter = currentlyDisplaying.GetComponent<CharacterInfoUI>().characterName.text;

        if (isEndgame) { return false; }
        // Internally track what thing we're displaying.
        currentDisplay = ItemDisplay.CHARACTER;

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

        string newCharacter = currentlyDisplaying.GetComponent<CharacterInfoUI>().characterName.text;

        return (currentCharacter.Equals(newCharacter));
    }

    /// <summary>
    /// Call when you want to display a quest.
    /// </summary>
    /// <param name="display"></param>
    public void DisplayQuest(QuestSheet quest, GameObject caller)
    {
        if (isEndgame) { return; }

        // Internally track what thing we're showing.
        currentDisplay = ItemDisplay.QUEST;
        GameObject.Find("TimeSystem").GetComponent<TimeSystem>().AddTicks(2);

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
        if (isEndgame) { return; }

        // Internally track what thing we're showing
        if (display)
            currentDisplay = ItemDisplay.DEBRIEF;
        else
            currentDisplay = 0;

        Destroy(currentlyDisplaying);

        debriefDisplay.SetActive(display);
    }

    /// <summary>
    /// Creates an endgame object that cannot be exited out of. Forces the player to quit.
    /// </summary>
    public void DisplayEndGame(Storylet endStatement)
	{
        // Destroy whatever's already here.
        Destroy(currentlyDisplaying);

        // Instantiate a new version of the EndGame.
        currentlyDisplaying = Instantiate(endGame, spawnPoint.transform);

        // Tell it the button and the info.
        EndGameDisplay item = currentlyDisplaying.GetComponent<EndGameDisplay>();
        item.SetExitAccess(gameManager);
        item.ShowInfo(endStatement);
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
        if(currentDisplay == ItemDisplay.QUEST)
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

    public void RemoveAdventurer(CharacterSheet character)
    {
        if (currentDisplay == ItemDisplay.QUEST)
        {
            QuestUI questReference;
            if (currentlyDisplaying.TryGetComponent<QuestUI>(out questReference))
            {
                questReference.RemoveCharacter(character);
            }
        }
    }

    public void ClearDisplay()
	{
        if (currentDisplay == ItemDisplay.NOTHING)
            return;
        Destroy(currentlyDisplaying);
        currentDisplay = ItemDisplay.NOTHING;
    }


    public bool CurrentlyDisplayingQuest()
    {
        return transform.Find("SpawnPoint/QuestUI(Clone)") != null;
    }
    
    /// <summary>
    /// returns currently displayed quest as a gameobject, or null if no quest is currently displayed
    /// </summary>
    /// <returns></returns>
    public GameObject GetQuestUI()
    {
        if (CurrentlyDisplayingQuest())
            return transform.Find("SpawnPoint/QuestUI(Clone)").gameObject;
        else
            return null;
    }
}
