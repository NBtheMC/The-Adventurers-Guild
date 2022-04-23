using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    private ItemDisplayManager displayManager;
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public bool isDisplayed = false;

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

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (PauseMenu.gamePaused)
            return;
        float deltaX = Input.mousePosition.x - clickPos.x;
        float deltaY = Input.mousePosition.y - clickPos.y;

        if ((Mathf.Abs(deltaX) < movementDelta && Mathf.Abs(deltaY) < movementDelta))
        {
            if (!isDisplayed || !displayManager.characterDisplay.activeInHierarchy)
            {
                displayManager.characterDisplay.SetActive(true);
                displayManager.questDisplay.SetActive(false);
                displayManager.debriefDisplay.SetActive(false);
                if (CharInfoSpawn.transform.childCount != 0)
                {
                    CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
                }

                //CharBook.DisplayCharacter(characterSheet);
                GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab, CharInfoSpawn.transform);
                CharInfoUIObject.transform.parent.SetAsLastSibling();
                CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
                characterInfoUI.charObject = this.gameObject;

                characterInfoUI.SetupCharacterInfoUI(characterSheet);

                //Add character portrait
                if (characterSheet.portrait != null)
                {
                    //CharInfoUIObject.AddComponent<Image>().sprite = character.portrait;
                    CharInfoUIObject.transform.Find("Portrait").GetComponent<Image>().sprite = characterSheet.portrait;
                }

                isDisplayed = true;
            }
        }
    }
}
