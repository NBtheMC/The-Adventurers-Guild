using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreditsTileController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    public CharacterInitialStats stats;
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    public ItemDisplayManager displayManager; // The display manager we'll be using.
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab; // Thre prefab for our character Info.
    [SerializeField] private float movementDelta = 0; // What is this for?
    [HideInInspector] public bool isDisplayed = false;
    bool adventurerBusy = false; // Keeps track of whether the adventurer is busy or not.

    //All the individual bits under this component
    public Image portrait;
    public TMPro.TextMeshProUGUI nameText;

    public void Awake()
    {
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        CharInfoSpawn = displayManager.characterDisplay;
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        //dump stats into character sheet
        characterSheet = new CharacterSheet(stats);
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (PauseMenu.gamePaused)
            return;
        pointerEventData.useDragThreshold = false;
    }

    public void CharacterClicked()
    {
        displayManager.DisplayCharacter(characterSheet);

        // if (!isDisplayed || !displayManager.characterDisplay.activeInHierarchy)
        // {
        //     displayManager.DisplayCharacter(true);
        //     if (CharInfoSpawn.transform.childCount != 0)
        //     {
        //         CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
        //     }

        //     GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab, CharInfoSpawn.transform);
        //     CharInfoUIObject.transform.parent.SetAsLastSibling();
        //     CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
        //     characterInfoUI.charObject = this.gameObject;

        //     characterInfoUI.SetupCharacterInfoUI(characterSheet);

        //     //Add character portrait
        //     if (characterSheet.portrait != null)
        //     {
        //         CharInfoUIObject.transform.Find("Portrait").GetComponent<Image>().sprite = characterSheet.portrait;
        //     }

        //     isDisplayed = true;
        // }
        // else if(isDisplayed && displayManager.characterDisplay.activeInHierarchy)
        // {
        //     if (CharInfoSpawn.transform.childCount != 0)
        //     {
        //         CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
        //     }
        //     displayManager.DisplayCharacter(false);
        //     isDisplayed = false;
        // }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CharacterClicked();
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
        nameText.text = characterSheet.name;
	}
    

    public void GrayOutPortrait() { portrait.color = new Color32(150, 150, 150, 255); }

    /// <summary>
    /// Ungrays the thing.
    /// </summary>
    public void UngrayPortrait() { portrait.color = new Color32(255, 255, 255, 255); }

    /// <summary>
    /// Turns the adventurer to full color.
    /// </summary>
    public void MarkAdventurerAsFree()
    {
        adventurerBusy = false;
        portrait.color = new Color32(255, 255, 255, 255);
    }

    /// <summary>
    /// tints the color black.
    /// </summary>
    public void MarkAdventurerAsBusy()
    {
        adventurerBusy = true;
        portrait.color = new Color32(0, 0, 0, 200);
    }
}
