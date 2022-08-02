using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The purpsoe of this class is to handle player interactions. Most of anything the player does should call a method within this interface,
/// which then calls methods in other scripts.
/// </summary>
public class PlayerInterface : MonoBehaviour
{
    private CharacterPoolController adventurerPoolController;
    private CharacterSheetManager adventurerSheetManager;
    private ItemDisplayManager itemDisplayManager;
    private QuestingManager questingManager;

    void Awake()
    {
        adventurerPoolController = GameObject.Find("QuestDisplayManager/QuestDisplay/CharacterPool").GetComponent<CharacterPoolController>();
        adventurerSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        itemDisplayManager = GameObject.Find("QuestDisplayManager/QuestDisplay/CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
    }

    //For when player right clicks on an adventurer in the adventurer pool
    public void AdventurerPoolRightClick(CharacterSheet character)
    {
        if (character == null)
        {
            Debug.LogWarning("AdventurerPoolRightClick() was called with a null value"); return;
        }


        if (adventurerSheetManager.GetAdventurerState(character) == AdventurerState.FREE && itemDisplayManager.CurrentlyDisplayingQuest()
            && !itemDisplayManager.GetQuestUI().GetComponent<QuestUI>().questIsActive())
        {
            adventurerSheetManager.SetAdventurerState(character, AdventurerState.ASSIGNED);
            itemDisplayManager.AssignAdventurer(character);
            adventurerPoolController.GrayOutPortrait(character);
        }
    }

    //For when the player right clicks on an adventurer in the quest display
    public bool QuestDisplayRightClick(CharacterSheet character)
    {
        if (character == null)
        {
            Debug.LogWarning("QuestDisplayRightClick was called with a null value"); return false;
        }

        Debug.Log("Calling QuestDisplayRightClick");
        Debug.Log("Advneturer State: " + adventurerSheetManager.GetAdventurerState(character));
        Debug.Log("Currently Displaying Quest: " + itemDisplayManager.CurrentlyDisplayingQuest());
        if (adventurerSheetManager.GetAdventurerState(character) == AdventurerState.ASSIGNED && itemDisplayManager.CurrentlyDisplayingQuest()
            && !itemDisplayManager.GetQuestUI().GetComponent<QuestUI>().questIsActive())
        {
            adventurerSheetManager.SetAdventurerState(character, AdventurerState.FREE);
            itemDisplayManager.RemoveAdventurer(character);
            //itemDisplayManager.GetQuestUI().GetComponent<QuestUI>().questIsActive;
            adventurerPoolController.ResetPortraitColor(character);
            return true;
        }
        return false;
    }

    public void SendPartyOnQuest(PartySheet party, QuestSheet quest)
    {
        if (party.Party_Members.Count == 0) { Debug.Log($"No members {party.Party_Members.Count}"); return; }

        // Send the thing on the quest.
        adventurerSheetManager.SendPartyOnQuest(this, quest);
        foreach(CharacterSheet adventurer in party.Party_Members)
        {
            adventurerSheetManager.SetAdventurerState(adventurer, AdventurerState.QUESTING);
            adventurerPoolController.BlackOutPortrait(adventurer);
        }
        questingManager.StartQuest(quest);

        //display activequestbanner
        GetQuestBanner(quest).UpdateTimer();
    }

    private QuestBanner GetQuestBanner(QuestSheet quest)
    {
        GameObject bannerList = GameObject.Find("QuestDisplayManager/QuestDisplay/QuestList/QuestListViewport/ListContent");
        if (bannerList == null)
            Debug.LogError("bannerlist is null");
        foreach(Transform transform in bannerList.transform)
        {
            if (transform.GetComponent<QuestBanner>().questSheet == quest)
                return transform.GetComponent<QuestBanner>();
        }
        Debug.LogError("Couldn't find questBanner with quest: " + quest.questName);
        return null;
    }

    public void LeftClickedOnCharacter(CharacterSheet character)
    {
        itemDisplayManager.DisplayCharacter(character);
    }

    public void CloseQuesDisplay(PartySheet party)
    {
        foreach(CharacterSheet adventurer in party.Party_Members)
        {
            adventurerSheetManager.SetAdventurerState(adventurer, AdventurerState.FREE);
            adventurerPoolController.ResetPortraitColor(adventurer);
        }

    }
}
