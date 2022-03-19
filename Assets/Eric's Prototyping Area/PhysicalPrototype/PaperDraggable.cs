using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaperDraggable : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform transformer;

    // Start is called before the first frame update
    void Start()
    {
        transformer = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
	{
        transformer.localScale += new Vector3(0.05f, 0.05f, 0.05f);
	}

    public void OnPointerExit(PointerEventData eventData)
	{
        transformer.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
    }

	public void OnDrag(PointerEventData eventData)
	{
        transformer.position += new Vector3(eventData.delta.x, eventData.delta.y);
    }
}
