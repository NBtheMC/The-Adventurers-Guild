using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This controller allows a player to select and drag a controller around.
/// </summary>
public class CharacterSelectionController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // Give the location of a bunch of drop points to be used for characters.
    public List<GameObject> dropPoints;

    private Vector2 lockedPosition;
    private RectTransform transformer;
    private DropHandler.DropType dropType = DropHandler.DropType.character;

    void Awake()
    {
        transformer = this.GetComponent<RectTransform>();
        lockedPosition = transformer.anchoredPosition;
    }

    /// <summary>
    /// For when this UI object is beginning to be dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag");
        lockedPosition = transformer.anchoredPosition;
    }

    /// <summary>
    /// For when this UI object is being dragged.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        transformer.anchoredPosition += eventData.delta;
    }

    /// <summary>
    /// For when this object is finished dragging.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        transformer.anchoredPosition = lockedPosition;
    }
}
