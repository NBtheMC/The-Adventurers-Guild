using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//Takes in new quests and creates them on screen as UI objects
public class QuestUI : MonoBehaviour
{
    // Active or not
    private bool questActive;

    //Our neccessary UI display items.
    private QuestSheet attachedSheet; // The sheet used to feed data into here.
    private PartySheet adventuringParty; // The adventuring party that's going on the quest.

    private Text questName; // the ui object for the name
    private Text questDescription; // ui object for description
    public Text questGiver;
    public Text questFaction;
    private Text questReward; // ui object for reward.
    private QuestingManager questingManager; // ui object for questing manager.
    private RectTransform dropPoints; // our drop points

    // UI items to display quest breifing details.
    public Text cmbBriefText;
    public Text xpoBriefText;
    public Text ngoBriefText;
    public Text conBriefText;
    //Total party stat details
    public Text cmbTotalParty;
    public Text xpoTotalParty;
    public Text ngoTotalParty;
    public Text conTotalParty;

    // Essential references.
    private CharacterPoolController characterPool; // reference to the characterPool
    private CharacterSheetManager charSheetManager; // reference to the characterSheetManager
    private PlayerInterface playerInterface; 
    //private DropHandler dropHandler; // reference 
    private RectTransform transformer; // defines the rectangle reference for this dragger.
    [HideInInspector] public GameObject questBanner; // the banner that spawned this quest.

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

    // Start is called before the first frame update
    void Awake()
    {
        transformer = this.GetComponent<RectTransform>();
        characterSlots = new CharacterSlot[4];

        //quest info objects
        questName = transformer.Find("Title").gameObject.GetComponent<Text>();
        questDescription = transformer.Find("Description/DescriptionText").gameObject.GetComponent<Text>();
        questReward = transformer.Find("Panel/Rewards/RewardText").gameObject.GetComponent<Text>();

        //party formation objects
        dropPoints = transformer.Find("Drop Points").GetComponent<RectTransform>();

        for (int i = 0; i < 4; i++)
        {
            QuestUICharacterButton child = dropPoints.transform.GetChild(i).gameObject.GetComponent<QuestUICharacterButton>();// Get the child
            child.questUI = this; // Give it reference.
        }

        questingManager = GameObject.Find("QuestingManager").GetComponent<QuestingManager>();
        characterPool = GameObject.Find("CharacterPool").GetComponent<CharacterPoolController>();

        charSheetManager = GameObject.Find("CharacterSheetManager").GetComponent<CharacterSheetManager>();
        playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();
        //dropHandler = GameObject.Find("DropHandler").GetComponent<DropHandler>();

        
    }

	/// <summary>
	/// Puts all the data where it's suppose to be on this sheet. You can use it to refresh.
	/// </summary>
	/// <param name="questSheet"></param>
	/// <param name="displayOnly"></param>
	public void SetupQuestUI(QuestSheet questSheet)
    {
        attachedSheet = questSheet;
        questActive = questSheet.isActive;

        // Make a party if it doesn't have one already.
        if (attachedSheet.adventuring_party == null) { attachedSheet.assignParty(new PartySheet()); }
        adventuringParty = attachedSheet.adventuring_party;

        // Setup name
        questName.text = attachedSheet.questName;

        // setup description
        questDescription.text = attachedSheet.questDescription;

        questGiver.text = attachedSheet.questGiver;
        questFaction.text = attachedSheet.faction;

        // setup reward
        questReward.text = string.Format("{0}", attachedSheet.EstimatedRewardTotal());

        // Get all of our estimates.
        cmbBriefText.text = attachedSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Combat).ToString();
        xpoBriefText.text = attachedSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Exploration).ToString();
        ngoBriefText.text = attachedSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Negotiation).ToString();
        conBriefText.text = attachedSheet.CalcualteNodeRanges(CharacterSheet.StatDescriptors.Constitution).ToString();

        // Turn off the buttons if this thing is active.
        this.transform.Find("Send Party").gameObject.SetActive(!questActive);
        this.transform.Find("Reject").gameObject.SetActive(!questActive);

        // Loop through the character Sheets in party members and assign them dropPoints.
        GameObject dropPoints = this.transform.Find("Drop Points").gameObject; // 
        for (int i = 0; i < questSheet.PartyMembers.Count; i++)
		{
            // Dump them visually in the drop points.
            QuestUICharacterButton child = dropPoints.transform.GetChild(i).gameObject.GetComponent<QuestUICharacterButton>();// Get the child
            child.SlotIn(attachedSheet.PartyMembers[i]); //tell it to add data.
		}

        // Display the summary stats.
        cmbTotalParty.text = adventuringParty.getStatSummed(CharacterSheet.StatDescriptors.Combat).ToString();
        xpoTotalParty.text = adventuringParty.getStatSummed(CharacterSheet.StatDescriptors.Exploration).ToString();
        ngoTotalParty.text = adventuringParty.getStatSummed(CharacterSheet.StatDescriptors.Negotiation).ToString();
        conTotalParty.text = adventuringParty.getStatSummed(CharacterSheet.StatDescriptors.Constitution).ToString();
    }

    /*
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
    }*/

    /// <summary>
    /// Takes all selected characters and assigns them to the current quest. Called by a button
    /// </summary>
    public void SendParty()
    {
        /*
        Debug.Log("Attempted to send party.");
        if (questActive || adventuringParty.Party_Members.Count == 0) { Debug.Log($"No members {adventuringParty.Party_Members.Count}, or quest active {questActive}."); return; }

        // Send the thing on the quest.
        charSheetManager.SendPartyOnQuest(this, attachedSheet);
        questingManager.StartQuest(attachedSheet);

        //display activequestbanner
        questBanner.GetComponent<QuestBanner>().ToggleQuestActiveState();

        // Play Sound.
        SoundManagerScript.PlaySound("stamp");

        // Self Destruct
        Destroy(this.gameObject);
        */
        playerInterface.SendPartyOnQuest(adventuringParty, attachedSheet);
        SoundManagerScript.PlaySound("stamp");
        Destroy(this.gameObject);
    }

    /// <summary>
    /// For our buttons to do things.
    /// </summary>
    public void DestroyUI()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Called when destroyed.
    /// </summary>
	private void OnDestroy()
	{
        Debug.Log($"{attachedSheet.questName} questUI destroyed");
		if (!attachedSheet.isActive)
		{
            attachedSheet.assignParty(null); // null out the party.
            playerInterface.CloseQuesDisplay(adventuringParty);
		}
	}

	/// <summary>
	/// Adds a character to the quest, then refreshes the count.
	/// </summary>
	/// <param name="character"></param>
	public bool AddCharacter(CharacterSheet character)
    {
        Debug.Log($"Attempting to Add {attachedSheet.questName}");
        if (attachedSheet.isActive || attachedSheet.isComplete) return false;
        if (attachedSheet.adventuring_party.Party_Members.Count >= 4) return false;
        if (IsCharacterAssigned(character)) return false;

        adventuringParty.addMember(character);//Add the party member.

        /*
        characterSlots[freeSlot].textObject.SetActive(true);
        characterSlots[freeSlot].name.text = character.name;

        //CharInfoUIObject.transform.Find("PortraitFrame").Find("Portrait").GetComponent<Image>().sprite = character.portrait;
        characterSlots[freeSlot].portraitObject.SetActive(true);
        characterSlots[freeSlot].portrait.sprite = character.portrait;
        characterSlots[freeSlot].character = character;
        characterSlots[freeSlot].characterAssigned = true;
        characterSlots[freeSlot].emptyCharFrame.SetActive(false);
        characterSlots[freeSlot].filledCharFrame.SetActive(true);*/

        SetupQuestUI(attachedSheet);
        return true;
    }

    /// <summary>
    /// Removes a character from the party data, then refreshes the count.
    /// </summary>
    /// <param name="slot"></param>
    public void RemoveCharacter(int slot)
    {
        if (attachedSheet.isActive || attachedSheet.isComplete) return;

        if (slot >= attachedSheet.adventuring_party.Party_Members.Count) { Debug.Log("No members available in this slot, returning."); return; }
        adventuringParty.removeMember(attachedSheet.adventuring_party.Party_Members[slot]); // Remove said party member.

        /*
        characterPool.UnselectCharacter(characterSlots[slot].character);
        characterSlots[slot].name.text = "";
        characterSlots[slot].textObject.SetActive(false);
        characterSlots[slot].portrait.sprite = null;
        characterSlots[slot].portraitObject.SetActive(false);
        characterSlots[slot].character = null;
        characterSlots[slot].characterAssigned = false;
        characterSlots[slot].emptyCharFrame.SetActive(true);
        characterSlots[slot].filledCharFrame.SetActive(false);
        AssignedCharacters--;*/

        SetupQuestUI(attachedSheet);
    }

    public void RemoveCharacter(CharacterSheet character)
    {
        CharacterSheet adventurerToRemove = null;

        foreach (CharacterSheet assignedCharacter in attachedSheet.PartyMembers)
            if (assignedCharacter == character)
                adventurerToRemove = character;

        if (adventurerToRemove != null)
            adventuringParty.removeMember(adventurerToRemove);

        SetupQuestUI(attachedSheet);
    }

    public bool IsCharacterAssigned(CharacterSheet character)
    {
        foreach (CharacterSheet charSlot in attachedSheet.PartyMembers)
        {
            if (character == charSlot) { return true; }
        }
        return false;
    }

    public bool questIsActive()
    {
        return (questBanner.GetComponent<QuestBanner>().questIsActive || attachedSheet.isComplete);
    }
}
