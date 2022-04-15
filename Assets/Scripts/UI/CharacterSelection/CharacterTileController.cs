using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterTileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public CharacterSheet characterSheet; //reference to associated CharacterSheet
    private CharacterBookManager CharBook;
    [SerializeField] private float movementDelta = 0;
    private Vector3 clickPos;

    public void Awake()
    {
        CharBook = GameObject.Find("CharInfoBook").GetComponent<CharacterBookManager>();
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
            CharBook.DisplayCharacter(characterSheet);
        }
    }
}
