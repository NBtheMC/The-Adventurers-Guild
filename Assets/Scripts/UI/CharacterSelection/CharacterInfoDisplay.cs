using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInfoDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    private GameObject QuestDisplay;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public GameObject CharacterInfo;
    [HideInInspector] public bool isDisplayed;

    public void Awake()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        QuestDisplay = GameObject.Find("QuestDisplay");

        isDisplayed = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        clickPos = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        float deltaX = Input.mousePosition.x - clickPos.x;
        float deltaY = Input.mousePosition.y - clickPos.y;
        //if(canDisplay && !isDisplayed)
        if((Mathf.Abs(deltaX) < movementDelta && Mathf.Abs(deltaY) < movementDelta) && !isDisplayed)
        {
            GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab);
            CharInfoUIObject.GetComponent<CharacterInfoUI>().charObject = this.gameObject;

            CharacterInfo = CharInfoUIObject;

            CharInfoUIObject.transform.SetParent(QuestDisplay.transform, false);
            CharInfoUIObject.transform.localPosition = new Vector3(-230, 80, 0);
            CharInfoUIObject.transform.SetAsLastSibling();

            CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
            characterInfoUI.SetupCharacterInfoUI(characterSheet);

            isDisplayed = true;
        }
    }
}
