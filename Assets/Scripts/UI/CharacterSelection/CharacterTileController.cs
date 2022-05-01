using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterTileController : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    private GameObject CharInfoSpawn;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public bool isDisplayed = false;

    public event EventHandler<CharacterSheet> CharacterLeftClickedOnEvent;
    public event EventHandler<CharacterSheet> CharacterRightClickedOnEvent;

    public void Awake()
    {
        CharInfoSpawn = GameObject.Find("CurrentItemDisplay/CharInfo");
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
    }



    public void CharacterClicked()
    {
        if (PauseMenu.gamePaused)
            return;
        if (!isDisplayed)
        {
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
                //CharInfoUIObject.AddComponent<Image>().sprite = character.portrait;
                CharInfoUIObject.transform.Find("Portrait").GetComponent<Image>().sprite = characterSheet.portrait;
            }

            isDisplayed = true;
        }
        else
        {
            CharInfoSpawn.transform.SetAsLastSibling();
        }
    }

    public void GrayOutPortrait()
    {
        transform.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CharacterClicked();
            CharacterLeftClickedOnEvent?.Invoke(this, characterSheet);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            GrayOutPortrait();
            CharacterRightClickedOnEvent?.Invoke(this, characterSheet);
        }
    }
}
