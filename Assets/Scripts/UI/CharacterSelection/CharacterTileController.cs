using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    public ItemDisplayManager displayManager; // The display manager we'll be using.
    [HideInInspector] public bool isDisplayed = false;
    bool adventurerBusy = false; // Keeps track of whether the adventurer is busy or not.

    //All the individual bits under this component
    public Image portrait;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI combat;
    public TMPro.TextMeshProUGUI exploration;
    public TMPro.TextMeshProUGUI negotation;
    public TMPro.TextMeshProUGUI vitality;

    private PlayerInterface playerInterface;


    public void Awake()
    {
        displayManager = GameObject.Find("CurrentItemDisplay").GetComponent<ItemDisplayManager>();
        playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();
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

        //displayManager.DisplayCharacter(characterSheet);
        playerInterface.LeftClickedOnCharacter(characterSheet);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CharacterClicked();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right clicked on portrait");
            //if (!adventurerBusy && displayManager.AssignAdventurer(characterSheet))
			//{
                GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>().AdventurerPoolRightClick(characterSheet);
                //GrayOutPortrait();
			//}

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
        nameText.text = characterSheet.name;
        combat.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Combat).ToString();
        exploration.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Exploration).ToString();
        negotation.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Negotiation).ToString();
        vitality.text = characterSheet.getStat(CharacterSheet.StatDescriptors.Constitution).ToString();
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
