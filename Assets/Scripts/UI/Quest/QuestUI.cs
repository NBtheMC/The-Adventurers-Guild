using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour
{
    private QuestSheet attachedSheet;
    private Text questName;
    private Text questDescription;
    private Text questReward;
    private QuestingManager questingManager;
    private CharacterPoolController characterPool;
    private CharacterSheetManager charSheetManager;
    private DropHandler dropHandler;
    private RectTransform DropPoints;
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    [HideInInspector] public GameObject questBanner;


    public int AssignedCharacters { get; private set; } = 0;

    // UI items to display quest breifing details.
    public Text cmbBriefText;
    public Text xpoBriefText;
    public Text ngoBriefText;
    public Text conBriefText;

    private struct CharacterSlot
    {
        public GameObject gameObject;
        public GameObject textObject;
        public GameObject portraitObject;
        public Text name;
        public Image portrait;

        public CharacterSlot(GameObject obj)
        {
            gameObject = obj;
            textObject = obj.transform.Find("Name").gameObject;
            name = obj.transform.Find("Name").gameObject.GetComponent<Text>();
            portraitObject = obj.transform.Find("Portrait").gameObject;
            portrait = obj.transform.Find("Portrait").gameObject.GetComponent<Image>();
        }
    }

    private CharacterSlot[] characterSlots;

    // Start is called before the first frame update
    void Awake()
    {
        characterSlots = new CharacterSlot[4];
        

        transformer = this.GetComponent<RectTransform>();

        //quest info objects
        questName = transformer.Find("Title").gameObject.GetComponent<Text>();
        questDescription = transformer.Find("Description/DescriptionText").gameObject.GetComponent<Text>();
        questReward = transformer.Find("Rewards/RewardText").gameObject.GetComponent<Text>();

        //party formation objects
        DropPoints = transformer.Find("Drop Points").GetComponent<RectTransform>();

        for(int i = 0; i < 4; i++)
        {
            GameObject temp = DropPoints.GetChild(i).gameObject;
            characterSlots[i] = new CharacterSlot(temp);
        }

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        characterPool = GameObject.Find("CharacterPool").GetComponent<CharacterPoolController>();

        charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();
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

        cmbBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Combat).ToString();
        xpoBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Exploration).ToString();
        ngoBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Negotiation).ToString();
        conBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Constitution).ToString();

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

            //display activequestbanner
            questBanner.GetComponent<QuestBanner>().ToggleQuestActiveState();

            SoundManagerScript.PlaySound("stamp");
            DestroyUI();
        }

    }

    public void DestroyUI()
    {
        /*
        foreach (Transform child in DropPoints)
        {
            DraggerController character = child.GetComponent<ObjectDropPoint>().heldObject;
            if (character)
            {
                Destroy(character.gameObject);
            }
            dropHandler.dropPoints.Remove(child.GetComponent<ObjectDropPoint>());
        }
        */

        if (questBanner != null)
            questBanner.GetComponent<QuestBanner>().isDisplayed = false;

        characterPool.RefreshCharacterPool();
        Destroy(this.gameObject);
    }

    public void AddCharacter(CharacterSheet character)
    {
        if (AssignedCharacters >= 4) return;

        characterSlots[AssignedCharacters].textObject.SetActive(true);
        characterSlots[AssignedCharacters].name.text = character.name;

        //CharInfoUIObject.transform.Find("PortraitFrame").Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        characterSlots[AssignedCharacters].portraitObject.SetActive(true);
        characterSlots[AssignedCharacters].portrait.sprite = character.portrait;
        AssignedCharacters++;
    }

}
