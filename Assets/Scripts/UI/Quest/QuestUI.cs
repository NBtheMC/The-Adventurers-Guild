using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour
{
    private QuestSheet attachedSheet; // associated QuestSheet for this quest object 
    private Text questName; // quest name from attachedSheet
    private Text questDescription; // description of quest from atachedSheet
    private Text questReward; // reward for quest from attachedSheet
    private GameObject partyFormation; // Object containing drop points for party selection
    private GameObject sendPartyButton; // reference to button that assigns a party to a quest
    public GameObject dropPointPrefab; // reference to character drop point prefab
    [HideInInspector] public GameObject questBanner; // associated quest banner that created this UI object
    private QuestingManager questingManager; // reference to questingManager object
    private CharacterPoolController characterPool; // reference to characterPool
    private DropHandler dropHandler;

    // Start is called before the first frame update
    void Awake()
    {
        //quest info objects
        questName = this.transform.Find("Canvas/Title").gameObject.GetComponent<Text>();
        questDescription = this.transform.Find("Canvas/Description").gameObject.GetComponent<Text>();
        questReward = this.transform.Find("Canvas/Rewards/Text").gameObject.GetComponent<Text>();

        Canvas canv = this.transform.Find("Canvas").gameObject.GetComponent<Canvas>();
        canv.worldCamera = Camera.main;
        //party formation objects
        partyFormation = this.transform.Find("Canvas/Party").gameObject;
        sendPartyButton = this.transform.Find("Canvas/Send Party").gameObject;
        TogglePartyFormationVisibility();

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        characterPool = GameObject.Find("CharacterPool").GetComponent<CharacterPoolController>();

        dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();
    }

    //Creates Quest as a UI GameObject
    public void SetupQuestUI(QuestSheet questSheet)
    {
        attachedSheet = questSheet;
        
        //Setup name
        questName.text = attachedSheet.questName;
        //setup description
        questDescription.text = attachedSheet.getNewEventInfo().description;
        //setup first event

        //setup reward
        questReward.text = string.Format("Quest Reward: {0}", attachedSheet.EstimatedRewardTotal());
        //setup party formation drop points
        Rect rect = partyFormation.GetComponent<RectTransform>().rect;
        float dropPointOffset = rect.width / (questSheet.partySize + 1);

        for (int i = 1; i <= questSheet.partySize; i++)
        {
            //make the drop point, set to be a child of Party object
            GameObject dropPoint = Instantiate(dropPointPrefab);
            dropPoint.transform.SetParent(partyFormation.transform, false);

            //set anchor points
            RectTransform dropPointRT = dropPoint.GetComponent<RectTransform>();
            dropPointRT.anchorMin = new Vector2(0, 0.5f);
            dropPointRT.anchorMax = new Vector2(0, 0.5f);
            dropPointRT.pivot = new Vector2(0.5f, 0.5f);

            //set position
            dropPoint.transform.localPosition = new Vector3(150 - dropPointOffset * i, 5, 0);

            //add dropPoint to dropHandler
            dropHandler.AddDropPoint(dropPoint.GetComponent<ObjectDropPoint>());
        }
    }


    //To be used by QuestSheet in order to update with new quests
    public void UpdateQuestUI(QuestSheet.EventInfo newEvent)
    {
        //Add on new group

        //Add description, stat, and dc

        return;
    }

    /// <summary>
    /// Toggles visibility of the party formation box on a quest card
    /// </summary>
    public void TogglePartyFormationVisibility()
    {
        //hide any characters that are held by  a drop point on the quest card
        foreach (Transform child in partyFormation.transform)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
                character.gameObject.SetActive(!character.gameObject.activeSelf);

        }
        //hide party formation section and drop points
        partyFormation.SetActive(!partyFormation.activeSelf);
        sendPartyButton.SetActive(!sendPartyButton.activeSelf);
    }

    /// <summary>
    /// Takes all selected characters and assigns them to the current quest.
    /// </summary>
    public void SendParty()
    {
        //create new partySheet and add all selected adventurers
        PartySheet partyToSend = new PartySheet();

        foreach (Transform child in partyFormation.transform)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                CharacterSheet charSheet = character.gameObject.GetComponent<CharacterObject>().characterSheet;
                //add character to party sheet and remove from the character pool
                partyToSend.addMember(charSheet);
                characterPool.removeMember(charSheet);
            }
        }

        //assign partyToSend to the current quest
        attachedSheet.assignParty(partyToSend);
        questingManager.StartQuest(attachedSheet);

        //remove associated questBanner
        Destroy(questBanner);

        DestroyUI();
    }

    /// <summary>
    /// Removes all associated UI elements from the screen
    /// </summary>
    public void DestroyUI()
    {
        //Delete any character objects dropped onto this UI object
        foreach (Transform child in partyFormation.transform)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                Destroy(character.gameObject);
            }
        }

        //REMOVE DROP POINTS FROM DROPHANDLER

        //Destory gameobject and then refresh character pool
        Destroy(this.gameObject);
        characterPool.RefreshCharacterPool();
    }


}
