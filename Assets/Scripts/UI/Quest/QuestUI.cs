using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler
{
    private QuestSheet attachedSheet;
    private Transform attachedObject;
    private Text questName;
    private Text questDescription;
    private Text questReward;
    private GameObject partyFormation;
    private GameObject sendPartyButton;
    private GameObject dropPointPrefab;
    private QuestingManager questingManager;
    private CharacterPoolController characterPool;
    private CharacterSheetManager charSheetManager;
    private DropHandler dropHandler;
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    [HideInInspector] public GameObject questBanner;
    private bool questSent = false;

    // Start is called before the first frame update
    void Awake()
    {
        attachedObject = this.transform;

        //quest info objects
        questName = attachedObject.Find("Title").gameObject.GetComponent<Text>();
        questDescription = attachedObject.Find("Description").gameObject.GetComponent<Text>();
        questReward = attachedObject.Find("Rewards/Text").gameObject.GetComponent<Text>();

        //party formation objects
        partyFormation = attachedObject.Find("Party").gameObject;
        sendPartyButton = attachedObject.Find("Send Party").gameObject;
        TogglePartyFormationVisibility();

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        characterPool = GameObject.Find("CharacterPool").GetComponent<CharacterPoolController>();

        charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();
        dropPointPrefab = Resources.Load<GameObject>("SampleDropPoint");

        transformer = this.GetComponent<RectTransform>();

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
        questReward.text = string.Format("Quest Reward: 0-{0}", attachedSheet.EstimatedRewardTotal());
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

            //add drop point to dropHandler
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

        //find all characters on QuestUI object and add to partyToSend
        foreach (Transform child in partyFormation.transform)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                CharacterSheet charSheet = character.gameObject.GetComponent<CharacterInfoDisplay>().characterSheet;
                partyToSend.addMember(charSheet);
            }
        }
        
        //assign partyToSend to the current quest
        attachedSheet.assignParty(partyToSend);
        charSheetManager.SendPartyOnQuest(this,attachedSheet);
        questingManager.StartQuest(attachedSheet);

        questSent = true;
        DestroyUI();

        Destroy(questBanner);
    }

    public void DestroyUI()
    {
        foreach (Transform child in partyFormation.transform)
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
        Debug.Log("Dragging");
        transformer.position += new Vector3(eventData.delta.x,eventData.delta.y);
    }

    /// <summary>
    /// For when this UI object is beginning to be dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetAsLastSibling();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        this.transform.SetAsLastSibling();
    }

}
