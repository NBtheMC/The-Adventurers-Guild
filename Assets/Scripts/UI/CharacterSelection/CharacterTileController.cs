using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    [HideInInspector] public ItemDisplayManager displayManager;
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public bool isDisplayed = false;
    bool adventurerBusy = false;

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
        clickPos = Input.mousePosition;
    }

    /*
    public void OnPointerUp(PointerEventData pointerEventData)

    */
    public void CharacterClicked()
    {
        if (PauseMenu.gamePaused)
            return;

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
            displayManager.DisplayCharacter(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CharacterClicked();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameObject questUI = GameObject.Find("QuestDisplayManager/QuestDisplay/CurrentItemDisplay/Quest/QuestUI(Clone)");
            if (questUI == null || questUI.GetComponent<QuestUI>().questIsActive() || adventurerBusy) return;

            if (questUI.GetComponent<QuestUI>().AssignedCharacters < 4)
            {
                GrayOutPortrait();
                questUI.GetComponent<QuestUI>().AddCharacter(characterSheet);
            }
        }
    }


    public void GrayOutPortrait()
    {
        transform.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
    }

    public void UngrayPortrait()
    {
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void MarkAdventurerAsFree()
    {
        adventurerBusy = false;
        transform.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void MarkAdventurerAsBusy()
    {
        adventurerBusy = true;
        transform.GetComponent<Image>().color = new Color32(150, 150, 150, 200);
    }
}
