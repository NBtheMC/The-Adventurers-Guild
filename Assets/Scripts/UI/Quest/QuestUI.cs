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
        public GameObject emptyCharFrame;
        public GameObject filledCharFrame;
        public Text name;
        public Image portrait;
        public CharacterSheet character;
        public bool characterAssigned;

        public CharacterSlot(GameObject obj)
        {
            gameObject = obj;
            textObject = obj.transform.Find("Name").gameObject;
            name = obj.transform.Find("Name").gameObject.GetComponent<Text>();
            portraitObject = obj.transform.Find("Portrait").gameObject;
            portrait = obj.transform.Find("Portrait").gameObject.GetComponent<Image>();
            emptyCharFrame = obj.transform.Find("EmptyCharacter").gameObject;
            filledCharFrame = obj.transform.Find("FilledCharacter").gameObject;
            character = null;
            characterAssigned = false;
        }
    }

    private CharacterSlot[] characterSlots;
    public int AssignedCharacters { get; private set; } = 0;
    //Total party stat details
    public Text cmbTotalParty;
    public Text xpoTotalParty;
    public Text ngoTotalParty;
    public Text conTotalParty;

    // Start is called before the first frame update
    void Awake()
    {
        transformer = this.GetComponent<RectTransform>();
        characterSlots = new CharacterSlot[4];

        //quest info objects
        questName = transformer.Find("Title").gameObject.GetComponent<Text>();
        questDescription = transformer.Find("Description/DescriptionText").gameObject.GetComponent<Text>();
        questReward = transformer.Find("Rewards/RewardText").gameObject.GetComponent<Text>();

        //party formation objects
        DropPoints = transformer.Find("Drop Points").GetComponent<RectTransform>();

        for (int i = 0; i < 4; i++)
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
        questReward.text = string.Format("{0}", attachedSheet.EstimatedRewardTotal());

        cmbBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Combat).ToString();
        xpoBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Exploration).ToString();
        ngoBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Negotiation).ToString();
        conBriefText.text = questSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Constitution).ToString();

        if (!questSheet.isActive)
        {
            //add drop points to drop handler
            foreach (Transform child in DropPoints)
            {
                //dropHandler.AddDropPoint(child.GetComponent<ObjectDropPoint>());
                dropHandler.dropPoints.Insert(0, child.GetComponent<ObjectDropPoint>());
            }
        }

    }


    //Party stat totals
    public void UpdateQuestUI()
    {
        //update total stats
        int cmbTotal = 0;
        int xpoTotal = 0;
        int ngoTotal = 0;
        int conTotal = 0;
        foreach (CharacterSlot charSlot in characterSlots)
        {
            if (charSlot.characterAssigned)
            {
                CharacterSheet charSheet = charSlot.character;
                cmbTotal += charSheet.getStat(CharacterSheet.StatDescriptors.Combat);
                xpoTotal += charSheet.getStat(CharacterSheet.StatDescriptors.Exploration);
                ngoTotal += charSheet.getStat(CharacterSheet.StatDescriptors.Negotiation);
                conTotal += charSheet.getStat(CharacterSheet.StatDescriptors.Constitution);
            }
        }

        cmbTotalParty.text = cmbTotal.ToString();
        xpoTotalParty.text = xpoTotal.ToString();
        ngoTotalParty.text = ngoTotal.ToString();
        conTotalParty.text = conTotal.ToString();
    
        return;
    }

    /// <summary>
    /// Takes all selected characters and assigns them to the current quest.
    /// </summary>
    public void SendParty()
    {
        if (AssignedCharacters == 0) return;
        //create new partySheet and add all selected adventurers
        PartySheet partyToSend = new PartySheet();

        //find all characters on QuestUI object and add to partyToSend
        foreach (CharacterSlot charSlot in characterSlots)
        {
            if (charSlot.characterAssigned)
                partyToSend.addMember(charSlot.character);
        }

        attachedSheet.assignParty(partyToSend);
        charSheetManager.SendPartyOnQuest(this, attachedSheet);
        questingManager.StartQuest(attachedSheet);

        //display activequestbanner
        questBanner.GetComponent<QuestBanner>().ToggleQuestActiveState();

        SoundManagerScript.PlaySound("stamp");
        DestroyUI();
    }

    public void DestroyUI()
    {
        if (questBanner != null)
        {
            QuestBanner q = questBanner.GetComponent<QuestBanner>();
            q.isDisplayed = false;
            q.displayManager.DisplayQuest(false);

            for (int i = 0; i < 4; i++)
            {
                if (characterSlots[i].characterAssigned)
                {
                    RemoveCharacter(i);
                }
            }
        }

        Destroy(this.gameObject);
    }

    public void AddCharacter(CharacterSheet character)
    {
        if (questBanner.GetComponent<QuestBanner>().questIsActive || attachedSheet.isComplete) return;
        if (AssignedCharacters >= 4) return;
        if (IsCharacterAssigned(character)) return;

        int freeSlot = 0;
        for (int i = 0; i < 4; i++)
        {
            if (!characterSlots[i].characterAssigned)
            {
                freeSlot = i;
                break;
            }
        }

        characterSlots[freeSlot].textObject.SetActive(true);
        characterSlots[freeSlot].name.text = character.name;

        //CharInfoUIObject.transform.Find("PortraitFrame").Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        characterSlots[freeSlot].portraitObject.SetActive(true);
        characterSlots[freeSlot].portrait.sprite = character.portrait;
        characterSlots[freeSlot].character = character;
        characterSlots[freeSlot].characterAssigned = true;
        characterSlots[freeSlot].emptyCharFrame.SetActive(false);
        characterSlots[freeSlot].filledCharFrame.SetActive(true);
        AssignedCharacters++;

        UpdateQuestUI();
    }

    public void RemoveCharacter(int slot)
    {
        if (questBanner.GetComponent<QuestBanner>().questIsActive || attachedSheet.isComplete) return;

        characterPool.UnselectCharacter(characterSlots[slot].character);
        characterSlots[slot].name.text = "";
        characterSlots[slot].textObject.SetActive(false);
        characterSlots[slot].portrait.sprite = null;
        characterSlots[slot].portraitObject.SetActive(false);
        characterSlots[slot].character = null;
        characterSlots[slot].characterAssigned = false;
        characterSlots[slot].emptyCharFrame.SetActive(true);
        characterSlots[slot].filledCharFrame.SetActive(false);
        AssignedCharacters--;
        UpdateQuestUI();
    }

    public bool IsCharacterAssigned(CharacterSheet character)
    {
        foreach (CharacterSlot charSlot in characterSlots)
        {
            if (charSlot.character == character)
                return true;
        }
        return false;
    }

    public bool questIsActive()
    {
        return (questBanner.GetComponent<QuestBanner>().questIsActive || attachedSheet.isComplete);
    }
}
