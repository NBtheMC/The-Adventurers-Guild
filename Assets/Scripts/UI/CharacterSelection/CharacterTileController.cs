using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    private CharacterBookManager CharBook;
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public bool isDisplayed = false;

    public void Awake()
    {
        CharBook = GameObject.Find("CharInfoBook").GetComponent<CharacterBookManager>();
        CharInfoSpawn = GameObject.Find("CurrentItemDisplay/CharInfo");
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

        if ((Mathf.Abs(deltaX) < movementDelta && Mathf.Abs(deltaY) < movementDelta) && !isDisplayed)
        {
            if (CharInfoSpawn.transform.childCount != 0)
            {
                CharInfoSpawn.transform.GetChild(0).GetComponent<CharacterInfoUI>().DestroyUI();
            }

            //CharBook.DisplayCharacter(characterSheet);
            GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab, CharInfoSpawn.transform);
            CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
            characterInfoUI.charObject = this.gameObject;
            characterInfoUI.SetupCharacterInfoUI(characterSheet);

            //Add character portrait
            if (characterSheet.portrait != null)
            {
                //CharInfoUIObject.AddComponent<Image>().sprite = character.portrait;
                CharInfoUIObject.transform.Find("PortraitFrame").Find("Portrait").GetComponent<Image>().sprite = characterSheet.portrait;
            }

            isDisplayed = true;
        }
    }
}
