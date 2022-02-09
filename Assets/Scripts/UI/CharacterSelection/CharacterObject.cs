using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public CharacterSheet characterSheet; //refernce to associated CharacterSheet
    private bool canDisplay = true; //Can we display character info when clicked on?
    private bool mouseDown = false; //Are we holding mouse down on this object?
    private GameObject QuestDisplay;

    private GameObject CharInfoUIPrefab;

    public void Start()
    {
        CharInfoUIPrefab = Resources.Load<GameObject>("CharacterInfoUI");
        QuestDisplay = GameObject.Find("QuestDisplay");
    }
    public void Update()
    {
        //if we're dragging something, don't display character info when we release it
        if(mouseDown && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            canDisplay = false;
    }

    //Detect current clicks on the GameObject
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        mouseDown = true;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        mouseDown = false;

        if(canDisplay)
        {
            GameObject CharInfoUIObject = Instantiate(CharInfoUIPrefab);

            CharInfoUIObject.transform.SetParent(QuestDisplay.transform, false);
            CharInfoUIObject.transform.localPosition = new Vector3(-230, 80, 0);

            CharacterInfoUI characterInfoUI = CharInfoUIObject.GetComponent<CharacterInfoUI>();
            characterInfoUI.SetupCharacterInfoUI(characterSheet);
        }

        canDisplay = true;
    }

}
