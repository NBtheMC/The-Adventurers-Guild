using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInfoDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CharacterSheet characterSheet; //refernce to associated CharacterSheet
    private bool canDisplay = true; //Can we display character info when clicked on?
    private bool mouseDown = false; //Are we holding mouse down on this object?
    private GameObject QuestDisplay;
    private GameObject CharInfoUIPrefab;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;
    [HideInInspector] public GameObject charInfoUI;
    [HideInInspector] public bool isDisplayed;

    public void Awake()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        QuestDisplay = GameObject.Find("QuestDisplay");

        isDisplayed = false;
    }
    public void Update()
    {
        //calculate change in position from when we clicked
        float deltaX = Input.mousePosition.x - clickPos.x;
        float deltaY = Input.mousePosition.y - clickPos.y;

        //if change in position is too big, don't display character info
        if(mouseDown && (Mathf.Abs(deltaX) > movementDelta || Mathf.Abs(deltaY) > movementDelta))
            canDisplay = false;
    }

    //Detect current clicks on the GameObject
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        mouseDown = true;
        clickPos = Input.mousePosition;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        mouseDown = false;

        if(canDisplay && !isDisplayed)
        {
            GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab);
            CharInfoUIObject.GetComponent<CharacterInfoUI>().charObject = this.gameObject;

            CharInfoUIObject.transform.SetParent(QuestDisplay.transform, false);
            CharInfoUIObject.transform.localPosition = new Vector3(-230, 80, 0);
            CharInfoUIObject.transform.SetAsLastSibling();

            CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
            characterInfoUI.SetupCharacterInfoUI(characterSheet);

            isDisplayed = true;
        }

        canDisplay = true;
    }
}
