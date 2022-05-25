using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    public ItemDisplayManager displayManager; // The display manager we'll be using.
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab; // Thre prefab for our character Info.
    [SerializeField] private float movementDelta = 0; // What is this for?
    [HideInInspector] public bool isDisplayed = false;
    bool adventurerBusy = false; // Keeps track of whether the adventurer is busy or not.

    //All the individual bits under this component
    public Image portrait;
    public TMPro.TextMeshProUGUI combat;
    public TMPro.TextMeshProUGUI exploration;
    public TMPro.TextMeshProUGUI negotation;
    public TMPro.TextMeshProUGUI vitality;


    public void Awake()
    {
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        CharInfoSpawn = displayManager.characterDisplay;
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (PauseMenu.gamePaused)
            return;
        pointerEventData.useDragThreshold = false;
    }

    public void CharacterClicked()
    {
        if (PauseMenu.gamePaused)
            return;

        displayManager.DisplayCharacter(characterSheet);

        /*
        if (!isDisplayed || !displayManager.characterDisplay.activeInHierarchy)
        {
            displayManager.DisplayCharacter(true);
            if (CharInfoSpawn.transform.childCount != 0)
            {
                CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
            }

            GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab, CharInfoSpawn.transform);
            CharInfoUIObject.transform.parent.SetAsLastSibling();
            CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
            characterInfoUI.charObject = this.gameObject;

            characterInfoUI.SetupCharacterInfoUI(characterSheet);

            //Add character portrait
            if (characterSheet.portrait != null)
            {
                CharInfoUIObject.transform.Find("Portrait").GetComponent<Image>().sprite = characterSheet.portrait;
            }

            isDisplayed = true;
        }
        else if(isDisplayed && displayManager.characterDisplay.activeInHierarchy)
        {
            if (CharInfoSpawn.transform.childCount != 0)
            {
                CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
            }
            displayManager.DisplayCharacter(false);
            isDisplayed = false;
        }*/
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"Character Menu for {characterSheet.name} opened");
            CharacterClicked();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Right Click on {characterSheet.name}");
            if (!adventurerBusy && displayManager.AssignAdventurer(characterSheet))
			{
                Debug.Log($"Assigned on {characterSheet.name}");
                GrayOutPortrait();
			}

            /*
            GameObject questUI = GameObject.Find("QuestDisplayManager/QuestDisplay/CurrentItemDisplay/Quest/QuestUI(Clone)");
            if (questUI == null || questUI.GetComponent<QuestUI>().questIsActive() || adventurerBusy) return;

            if (questUI.GetComponent<QuestUI>().AssignedCharacters < 4)
            {
                GrayOutPortrait();
                questUI.GetComponent<QuestUI>().AddCharacter(characterSheet);
            }*/
        }
    }

    /// <summary>
    /// Refreshes this characterTileController to update text and the picture.
    /// Checks charactersheet stats, picture
    /// checks what the status is in the CharacterSheetManager to be done soon.
    /// </summary>
    public void Refresh()
	{
        portrait.sprite = characterSheet.portrait;
        combat.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Combat).ToString();
        exploration.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
        negotation.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Negotiation).ToString();
        vitality.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Constitution).ToString();
	}
    

    public void GrayOutPortrait() { transform.GetComponent<Image>().color = new Color32(150, 150, 150, 255); }

    /// <summary>
    /// Ungrays the thing.
    /// </summary>
    public void UngrayPortrait() { transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255); }

    /// <summary>
    /// Turns the adventurer to full color.
    /// </summary>
    public void MarkAdventurerAsFree()
    {
        adventurerBusy = false;
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    /// <summary>
    /// tints the color black.
    /// </summary>
    public void MarkAdventurerAsBusy()
    {
        adventurerBusy = true;
        transform.GetComponent<Image>().color = new Color32(0, 0, 0, 200);
    }
}
