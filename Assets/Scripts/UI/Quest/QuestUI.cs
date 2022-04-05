using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private QuestSheet attachedSheet;
    private Text questName;
    private Text questDescription;
    private Text questReward;
    //private GameObject partyFormation;
    private GameObject sendPartyButton;
    private GameObject dropPointPrefab;
    private QuestingManager questingManager;
    private CharacterPoolController characterPool;
    private CharacterSheetManager charSheetManager;
    private DropHandler dropHandler;
    private RectTransform DropPoints;
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    [HideInInspector] public GameObject questBanner;
    private bool questSent = false;

    // Start is called before the first frame update
    void Awake()
    {
        transformer = this.GetComponent<RectTransform>();

        //quest info objects
        questName = transformer.Find("Title").gameObject.GetComponent<Text>();
        questDescription = transformer.Find("Description/DescriptionText").gameObject.GetComponent<Text>();
        questReward = transformer.Find("Rewards/RewardText").gameObject.GetComponent<Text>();

        //party formation objects
        DropPoints = transformer.Find("Drop Points").GetComponent<RectTransform>();
        sendPartyButton = transformer.Find("Send Party").gameObject;

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        characterPool = GameObject.Find("CharacterPool").GetComponent<CharacterPoolController>();

        charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();
        dropPointPrefab = Resources.Load<GameObject>("SampleDropPoint");
    }

    //Creates Quest as a UI GameObject
    public void SetupQuestUI(QuestSheet questSheet, bool displayOnly = false)
    {
        attachedSheet = questSheet;

        //Setup name
        questName.text = attachedSheet.questName;
        //setup description
        questDescription.text = attachedSheet.questDescription;
        //setup first event

        //setup reward
        questReward.text = string.Format("Reward: 0-{0}", attachedSheet.EstimatedRewardTotal());

        if (!displayOnly)
        {
            //add drop points to drop handler
            foreach (Transform child in DropPoints)
            {
                //dropHandler.AddDropPoint(child.GetComponent<ObjectDropPoint>());
                dropHandler.dropPoints.Insert(0, child.GetComponent<ObjectDropPoint>());
            }
        }

    }


    //To be used by QuestSheet in order to update with new quests
    public void UpdateQuestUI(QuestSheet.EventInfo newEvent)
    {
        //setup description
        questDescription.text = newEvent.description;
        //setup first event

        //setup reward
        questReward.text = string.Format("Reward: 0-{0}", attachedSheet.EstimatedRewardTotal());

        return;
    }

    /// <summary>
    /// Takes all selected characters and assigns them to the current quest.
    /// </summary>
    public void SendParty()
    {
        //create new partySheet and add all selected adventurers
        PartySheet partyToSend = new PartySheet();

        //find all characters on QuestUI object and add to partyToSend
        foreach (Transform child in DropPoints)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                CharacterSheet charSheet = character.gameObject.GetComponent<CharacterTileController>().characterSheet;
                partyToSend.addMember(charSheet);
            }
        }

        //assign partyToSend to the current quest
        if (partyToSend.Party_Members.Count > 0)
        {
            attachedSheet.assignParty(partyToSend);
            charSheetManager.SendPartyOnQuest(this, attachedSheet);
            questingManager.StartQuest(attachedSheet);

            questSent = true;
            DestroyUI();

            Destroy(questBanner);

            SoundManagerScript.PlaySound("stamp");
        }

    }

    public void DestroyUI()
    {
        foreach (Transform child in DropPoints)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                Destroy(character.gameObject);
            }
            dropHandler.dropPoints.Remove(child.GetComponent<ObjectDropPoint>());
        }

        questBanner.GetComponent<QuestBanner>().isDisplayed = false;

        characterPool.RefreshCharacterPool();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// For when this UI object is being dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        transformer.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    /// <summary>
    /// For when this UI object is clicked.
    /// </summary>
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        pointerEventData.useDragThreshold = false;
        this.transform.SetAsLastSibling();
        //remove drop points from dropHandler, then add them again infront
        foreach(Transform child in DropPoints)
        {
            dropHandler.dropPoints.Remove(child.GetComponent<ObjectDropPoint>());
            dropHandler.dropPoints.Insert(0, child.GetComponent<ObjectDropPoint>());
        }
    }

}
