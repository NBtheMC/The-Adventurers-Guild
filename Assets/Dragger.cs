using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dragger : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private Vector2 deltaValue = Vector2.zero;

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData){
        Debug.Log("Pointer down");
    }

    public void OnBeginDrag(PointerEventData eventData){
        deltaValue = Vector2.zero;
        Debug.Log("Begin Drag");
        //rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData){
        Debug.Log("Dragging");
        //this.transform.position += ((Vector3)eventData.delta);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        newPosition.z = 0;
        this.transform.position = newPosition;
        //rectTransform.anchoredPosition += (eventData.delta);
        //rectTransform.anchoredPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData){
        deltaValue = Vector2.zero;
        Debug.Log("End Drag");
        //rectTransform.anchoredPosition = Vector2.zero;
    }
}
